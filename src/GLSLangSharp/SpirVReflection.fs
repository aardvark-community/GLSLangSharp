namespace GLSLang.SpirV

open System.IO

type Width = int
type Type =
    | Int of Width * bool
    | Float of Width
    | Bool 
    | Vector of Type * int
    | Matrix of Type * int 
    | Ptr of Type
    | Array of Type * int
    | Image of dim : int * isDepth : bool * isArray : bool * isMultisampled : bool 
    | Sampler
    | Struct of list<Type * string * list<Decoration * uint32[]>>
    | Void
  

type Parameter = 
    { 
        paramName : string
        paramType : Type
        paramDecorations : list<Decoration * uint32[]> 
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Parameter =
    let tryGetLocation (p : Parameter) =
        p.paramDecorations |> List.tryPick (fun (d,args) ->
            match d with
                | Decoration.Location -> Some (args.[0] |> int)
                | _ -> None
        ) 

    let tryGetBinding (p : Parameter) =
        p.paramDecorations |> List.tryPick (fun (d,args) ->
            match d with
                | Decoration.Binding -> Some (args.[0] |> int)
                | _ -> None
        ) 

    let tryGetDescriptorSet (p : Parameter) =
       p.paramDecorations |> List.tryPick (fun (d,args) ->
            match d with
                | Decoration.DescriptorSet -> Some (args.[0] |> int)
                | _ -> None
        ) 

    let tryGetBuiltInSemantic (p : Parameter) =
        p.paramDecorations |> List.tryPick (fun (d,args) ->
            match d with
                | Decoration.BuiltIn -> Some (args.[0] |> unbox<BuiltIn>)
                | _ -> None
        )

    let inline paramName (p : Parameter) = p.paramName
    let inline paramType (p : Parameter) = p.paramType
    let inline paramDecorations (p : Parameter) = p.paramDecorations



type ShaderInterface = 
    { 
        executionModel : ExecutionModel
        entryPoint : string
        inputs : list<Parameter>
        outputs : list<Parameter>
        uniforms : list<Parameter>
        images : list<Parameter> 
    }



module SpirVReflection =

    [<AutoOpen>]
    module Utilites = 
        type OptionBuilder() =
            member x.Bind(v,f) = Option.bind f v
            member x.Return v = Some v
            member x.ReturnFrom o = o

        let opt = OptionBuilder()

        module List =
            let mapOption (f : 'a -> Option<'b> ) (xs : list<'a>) =
                let mapped = xs |> List.map f
                if mapped |> List.forall Option.isSome 
                then mapped |> List.map Option.get |> Some
                else None

        module Map =
            let ofListWithDuplicates(xs : list<'k*'v>) : Map<'k,list<'v>> =
                let mutable map = Map.empty
                for (k,v) in xs do
                    match map |> Map.tryFind k with
                        | Some v' -> map <- Map.add k (v :: v') map
                        | None -> map <- Map.add k [v] map
                map
    

    let getInterface (instructions : list<Instruction>) = 

        let inputVariables = 
            instructions |> List.choose (fun i -> 
                match i with
                    | Variable(typeId,target, kind, _) ->
                        match kind with
                            | StorageClass.Input 
                            | StorageClass.Uniform 
                            | StorageClass.Image 
                            | StorageClass.Output ->
                                Some (typeId, target, kind)
                            | _ -> None
                    | _ -> None
            )

        let names = 
            instructions |> List.choose (fun i ->
                match i with
                    | Name(target,name) -> Some (target,name)
                    | _ -> None
            ) |> Map.ofList

        let memberNames = 
            instructions |> List.choose (fun i ->
                match i with
                    | MemberName(target, index, name) -> 
                        Some ((target,index), name)
                    | _ -> None
            ) |> Map.ofList

        let memberDecorations =   
            instructions |> List.choose (fun i ->
                match i with
                    | MemberDecorate(target, index,dec,args) -> Some ((target, index),(dec,args))
                    | _ -> None
            ) |> Map.ofListWithDuplicates

        let map = 
             instructions |> List.choose (fun i ->
                    match SpirVUtilities.tryGetId i with
                        | Some id -> Some (id,i)
                        | None -> None
                ) |> Map.ofList

        let rec typeForId id = 
            opt {
                let! i = Map.tryFind id map 
                match i with 
                    | TypeVoid _ -> return Void
                    | TypeBool _ -> return Bool
                    | TypeInt(_,width,signed) -> return Int (int width,signed) 
                    | TypeFloat(_,width) -> return Float (int width) 
                    | TypeVector(_, comp, dim) -> 
                        let! comp = typeForId comp
                        return Vector(comp, int dim)
                    | TypeMatrix(_,colType,colCount) ->
                        let! colType = typeForId colType
                        return Matrix(colType,int colCount)
                    | TypeImage(resId,sampledType,dim,depth, arrayed, ms, sampled,format,access) ->
                        return Image(int dim, depth,arrayed,ms)
                    | TypeSampler(_) -> return Sampler
                    | TypeArray(_,elem,len) -> 
                        let! elem = typeForId elem
                        return Array(elem,int len)
                    | TypeStruct(_, fields) ->
                        let! fieldTypes = fields |> Array.toList |> List.mapOption typeForId

                        let fieldDecorations =
                            List.init fields.Length (fun i ->
                                match Map.tryFind (id,uint32 i) memberDecorations with
                                    | Some d -> d
                                    | None -> []
                            )

                        let fieldNames =
                            List.init fields.Length (fun i ->
                                match Map.tryFind (id, uint32 i) memberNames with
                                    | Some name -> name
                                    | _ -> failwith "no field name"
                            )

                        return Struct (List.zip3 fieldTypes fieldNames fieldDecorations)

                    | TypePointer(_,storageClass,baseType) ->
                        let! baseType = typeForId baseType
                        return Ptr baseType
                    | _ -> return! None
                        
            }

        let decorations =   
            instructions |> List.choose (fun i ->
                match i with
                    | Decorate(target,dec,args) -> Some (target,(dec,args))
                    | _ -> None
            ) |> Map.ofListWithDuplicates

        let parameters = 
            inputVariables |> List.map (fun (tid,id,kind) ->
                let t = typeForId tid
                let n = names |> Map.tryFind id
                let decorations = Map.tryFind id decorations
                match t,n with
                 | Some t, Some n -> 
                    let d = match decorations with Some d -> d | None -> []
                    (kind, n, t, d)
                 | _ -> failwith "could not resolve type or name"
            )

        let extractParamteters (c : StorageClass) =
            parameters 
                |> List.choose (fun p ->
                    match p with
                        | ci, n, t, d when ci = c -> Some { paramName = n; paramType = t; paramDecorations = d }
                        | _ -> None
                )


        let (execModel, entryPoint) = instructions |> List.pick (function EntryPoint(m,_,name) -> Some (m,name) | _ -> None)
        let inputs = extractParamteters StorageClass.Input
        let outputs = extractParamteters StorageClass.Output
        let uniforms = extractParamteters StorageClass.Uniform
        let images = extractParamteters StorageClass.Image

        { executionModel = execModel; entryPoint = entryPoint ;inputs = inputs; outputs = outputs; uniforms = uniforms; images = images }