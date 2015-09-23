namespace GLSLang.SpirV

open System.IO

type Width = int
type ShaderType =
    | Int of Width * bool
    | Float of Width
    | Bool 
    | Vector of ShaderType * int
    | Matrix of ShaderType * int 
    | Ptr of ShaderType
    | Array of ShaderType * int
    | Image of sampledType : ShaderType * dim : Dim * isDepth : bool * isArray : bool * isMultisampled : bool 
    | Sampler
    | Struct of string * list<ShaderType * string * list<Decoration * uint32[]>>
    | Void
  

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ShaderType =

    let glslDecoration (decorations : list<Decoration * uint32[]>) =
        if List.isEmpty decorations then
            ""
        else
            let decorations =
                decorations |> List.map (fun t ->
                    match t with
                        | Decoration.Location, [|loc|] -> sprintf "location = %d" (int loc)
                        | Decoration.Binding, [|b|] -> sprintf "binding = %d" (int b)
                        | Decoration.DescriptorSet, [|s|] -> sprintf "set = %d" (int s)
                        | Decoration.Index, [|i|] -> sprintf "index = %d" (int i)
                        | Decoration.BuiltIn, [|id|] -> sprintf "builtin = %A" (unbox<BuiltIn> id)

                        | Decoration.Nonreadable, _ -> "write_only"
                        | Decoration.Nonwritable, _ -> "read_only"
                        | Decoration.NoStaticUse, _ -> "nostaticuse"
                        | Decoration.RowMajor, _ -> "row_major"
                        | Decoration.ColMajor, _ -> "col_major"
                        | Decoration.Noperspective, _ -> "noperspective"
                        | Decoration.Flat, _ -> "flat"

                        | d, [||] -> sprintf "%A" d
                        | d, args -> sprintf "%A = { %s }" d (args |> Array.map string |> String.concat ", ")
                )
            decorations |> String.concat ", " |> sprintf "layout(%s)"

    let rec glslName (t : ShaderType) =
        match t with
            | Ptr t -> glslName t
            | Array(t, l) -> sprintf "%s[%d]" (glslName t) l

            | Bool -> "bool"
            | Int(32, true) -> "int"
            | Int(32, false) -> "uint"
            | Float 32 -> "float"
            | Float 64 -> "double"

            | Vector(Bool, d) -> sprintf "bvec%d" d
            | Vector(Int(32, true), d) -> sprintf "ivec%d" d
            | Vector(Int(32, false), d) -> sprintf "uvec%d" d
            | Vector(Float 32, d) -> sprintf "vec%d" d
            | Vector(Float 64, d) -> sprintf "dvec%d" d

            | Matrix(Vector(Float 32, r), c) -> 
                if r = c then sprintf "mat%d" r
                else sprintf "mat%dx%d" r c

            | Matrix(Vector(Float 64, r), c) -> 
                if r = c then sprintf "dmat%d" r
                else sprintf "dmat%dx%d" r c

            | Image(st,dim,d,a,m) -> 
                let st =
                    match st with
                        | Float 32 -> ""
                        | Int(32, true) -> "i"
                        | Int(32, false) -> "u"
                        | _ -> "g"

                let dim =
                    match dim with
                        | Dim.Dim1D -> "1D"
                        | Dim.Dim2D -> "2D"
                        | Dim.Dim3D -> "3D"
                        | Dim.DimBuffer -> "Buffer"
                        | Dim.DimCube -> "Cube"
                        | Dim.DimRect -> "Rect"
                        | _ -> failwithf "unsupported sampler dimension: %A" dim
                let d = if d then "Shadow" else ""
                let a = if a then "Array" else ""
                let m = if m then "MS" else ""
                st + "sampler" + dim + m + a + d



            | Struct(name, fields) ->
                
                let strs =
                    fields |> List.map (fun (t,n,dec) ->
                        let t = glslName t
                        let layout = glslDecoration dec

                        if layout.Length = 0 then
                            sprintf "    %s %s;" t n
                        else
                            sprintf "    %s %s %s;" layout t n
                    )

                strs |> String.concat "\r\n" |> sprintf "%s {\r\n%s\r\n }" name

            | _ -> sprintf "unknown type: %A" t

type Parameter = 
    { 
        paramName : string
        paramType : ShaderType
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
                        let! sampledType = typeForId sampledType
                        return Image(sampledType, unbox<Dim> dim, depth,arrayed,ms)
                    | TypeSampler(_) -> return Sampler
                    | TypeArray(_,elem,len) -> 
                        let! elem = typeForId elem
                        return Array(elem,int len)
                    | TypeStruct(res, fields) ->
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

                        let! n = Map.tryFind res names

                        return Struct (n, List.zip3 fieldTypes fieldNames fieldDecorations)

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


        let ioSort (p : Parameter) =
            match Parameter.tryGetLocation p with
                | Some l -> (l, p.paramName)
                | None -> (System.Int32.MaxValue, p.paramName)

        let uniformSort (p : Parameter) =
            let set = 
                match Parameter.tryGetDescriptorSet p with
                    | Some l -> l
                    | None -> System.Int32.MaxValue
   
            let binding = 
                match Parameter.tryGetDescriptorSet p with
                    | Some l -> l
                    | None -> System.Int32.MaxValue

            (set, binding, p.paramName)
        
        let (execModel, entryPoint) = instructions |> List.pick (function EntryPoint(m,_,name) -> Some (m,name) | _ -> None)
        let inputs = extractParamteters StorageClass.Input |> List.sortBy ioSort
        let outputs = extractParamteters StorageClass.Output |> List.sortBy ioSort
        let uniforms = extractParamteters StorageClass.Uniform |> List.sortBy uniformSort
        let images = extractParamteters StorageClass.Image |> List.sortBy uniformSort

        { executionModel = execModel; entryPoint = entryPoint ;inputs = inputs; outputs = outputs; uniforms = uniforms; images = images }