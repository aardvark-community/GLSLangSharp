namespace GLSLang.SpirV

open System.IO

type Instruction = 
    | Nop
    | Undef of resType : uint32 * resId : uint32
    | Source of language : SourceLanguage * version : uint32
    | SourceExtension of extension : string
    | Name of target : uint32
    | MemberName of _type : uint32 * mem : uint32 * name : string
    | String of resId : uint32 * value : string
    | Line of target : uint32 * file : uint32 * line : uint32 * col : uint32
    | Decorate of target : uint32 * decoration : Decoration * args : uint32[]
    | MemberDecorate of structureType : uint32 * mem : uint32 * decoration : Decoration * args : uint32[]
    | DecorationGroup of resId : uint32
    | GroupDecorate of decorationGroup : uint32 * targets : uint32[]
    | GroupMemberDecorate of decorationGroup : uint32 * idLiteralPairs : uint32[]
    | Extension of extName : string
    | ExtInstImport of resId : uint32 * name : string
    | ExtInst of resType : uint32 * resId : uint32 * set : uint32 * instruction : uint32 * args : uint32[]
    | MemoryModel of addressingModel : AddressingModel * memoryModel : MemoryModel
    | EntryPoint of execModel : ExecutionModel * id : uint32 * name : string
    | ExecutionMode of entryPoint : uint32 * mode : ExecutionMode * args : uint32[]
    | Capability of cap : Capability
    | TypeVoid of resId : uint32
    | TypeBool of resId : uint32
    | TypeInt of resId : uint32 * width : uint32 * signed : bool
    | TypeFloat of resId : uint32 * width : uint32
    | TypeVector of resId : uint32 * compType : uint32 * compCount : uint32
    | TypeMatrix of resId : uint32 * colType : uint32 * colCount : uint32
    | TypeImage of resId : uint32 * sampledType : uint32 * dim : uint32 * depth : uint32 * arrayed : bool * ms : bool * sampled : SampleMode * format : ImageFormat * access : uint32[]
    | TypeSampler of resId : uint32
    | TypeSampledImage of resId : uint32 * imageType : uint32
    | TypeArray of resId : uint32 * elemType : uint32 * length : uint32
    | TypeRuntimeArray of resId : uint32 * elemType : uint32
    | TypeStruct of resId : uint32 * fieldTypes : uint32[]
    | TypeOpaque of resId : uint32 * name : string
    | TypePointer of resId : uint32 * sClass : StorageClass * _type : uint32
    | TypeFunction of resId : uint32 * retType : uint32 * argTypes : uint32[]
    | TypeEvent of resId : uint32
    | TypeDeviceEvent of resId : uint32
    | TypeReserveId of resId : uint32
    | TypeQueue of resId : uint32
    | TypePipe of resId : uint32 * _type : uint32 * access : AccessQualifier
    | ConstantTrue of resType : uint32 * resId : uint32
    | ConstantFalse of resType : uint32 * resId : uint32
    | Constant of resType : uint32 * resId : uint32 * values : uint32[]
    | ConstantComposite of resType : uint32 * resId : uint32 * constituents : uint32[]
    | ConstantSampler of resType : uint32 * resId : uint32 * addressingMode : SamplerAddressingMode * normalized : bool * filter : SamplerFilterMode
    | ConstantNull of resType : uint32 * resId : uint32
    | SpecConstantTrue of resType : uint32 * resId : uint32
    | SpecConstantFalse of resType : uint32 * resId : uint32
    | SpecConstant of resType : uint32 * resId : uint32 * values : uint32[]
    | SpecConstantComposite of resType : uint32 * resId : uint32 * constituents : uint32[]
    | SpecConstantOp of resType : uint32 * resId : uint32 * opCode : OpCode * operands : uint32[]
    | Variable of resType : uint32 * resId : uint32 * storageClas : StorageClass * initializers : uint32[]
    | ImageTexelPointer of resType : uint32 * resId : uint32 * image : uint32 * coordinate : Dim * sample : uint32
    | Load of resType : uint32 * resId : uint32 * pointer : uint32 * memoryAccess : uint32[]
    | Store of resType : uint32 * resId : uint32 * pointer : uint32 * ob : uint32 * memoryAccess : uint32[]
    | CopyMemory of target : uint32 * source : uint32 * memoryAccess : uint32[]
    | CopyMemorySized of target : uint32 * source : uint32 * size : uint32 * memoryAccess : uint32[]
    | AccessChain of resType : uint32 * resId : uint32 * _base : uint32 * indices : uint32[]
    | InBoundsAccessChain of resType : uint32 * resId : uint32 * _base : uint32 * indices : uint32[]
    | PtrAccessChain of resType : uint32 * resId : uint32 * _base : uint32 * element : uint32 * indices : uint32[]
    | ArrayLength of resType : uint32 * resId : uint32 * structure : uint32 * arrMember : uint32
    | GenericPtrMemSemantics of resType : uint32 * resId : uint32 * ptr : uint32
    | Function of resType : uint32 * resId : uint32 * ctrl : FunctionControl * _type : uint32
    | FunctionParameter of resType : uint32 * resId : uint32
    | FunctionEnd
    | FunctionCall of resType : uint32 * resId : uint32 * f : uint32 * args : uint32[]
    | SampledImage of resType : uint32 * resId : uint32 * image : uint32 * sampler : uint32
    | ImageSampleImplicitLod of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * operands : uint32[]
    | ImageSampleExplicitLod of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * operands : uint32[]
    | ImageSampleDrefImplicitLod of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * dref : uint32 * operands : uint32[]
    | ImageSampleDrefExplicitLod of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * dref : uint32 * operands : uint32[]
    | ImageSampleProjImplicitLod of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * operands : uint32[]
    | ImageSampleProjExplicitLod of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * operands : uint32[]
    | ImageSampleProjDrefImplicitLod of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * dref : uint32 * operands : uint32[]
    | ImageSampleProjDrefExplicitLod of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * dref : uint32 * operands : uint32[]
    | ImageFetch of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * operands : uint32[]
    | ImageGather of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * comp : uint32 * operands : uint32[]
    | ImageDrefGather of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32 * dref : uint32 * operands : uint32[]
    | ImageRead of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32
    | ImageWrite of image : uint32 * coordinate : uint32 * texel : uint32
    | ImageQueryDim of resType : uint32 * resId : uint32 * image : uint32
    | ImageQueryFormat of resType : uint32 * resId : uint32 * image : uint32
    | ImageQueryOrder of resType : uint32 * resId : uint32 * image : uint32
    | ImageQuerySizeLod of resType : uint32 * resId : uint32 * image : uint32 * level : uint32
    | ImageQuerySize of resType : uint32 * resId : uint32 * image : uint32
    | ImageQueryLod of resType : uint32 * resId : uint32 * image : uint32 * coordinate : uint32
    | ImageQueryLevels of resType : uint32 * resId : uint32 * image : uint32
    | ImageQuerySamples of resType : uint32 * resId : uint32 * image : uint32
    | ConvertFToU of resType : uint32 * resId : uint32 * value : uint32
    | ConvertFToS of resType : uint32 * resId : uint32 * value : uint32
    | ConvertSToF of resType : uint32 * resId : uint32 * value : uint32
    | ConvertUToF of resType : uint32 * resId : uint32 * value : uint32
    | UConvert of resType : uint32 * resId : uint32 * value : uint32
    | SConvert of resType : uint32 * resId : uint32 * value : uint32
    | FConvert of resType : uint32 * resId : uint32 * value : uint32
    | QuantizeToF16 of resType : uint32 * resId : uint32 * value : uint32
    | ConvertPtrToU of resType : uint32 * resId : uint32 * value : uint32
    | SatConvertSToU of resType : uint32 * resId : uint32 * value : uint32
    | SatConvertUToS of resType : uint32 * resId : uint32 * value : uint32
    | ConvertUToPtr of resType : uint32 * resId : uint32 * value : uint32
    | PtrCastToGeneric of resType : uint32 * resId : uint32 * value : uint32
    | GenericCastToPtr of resType : uint32 * resId : uint32 * value : uint32
    | GenericCastToPtrExplicit of resType : uint32 * resId : uint32 * value : uint32 * _class : StorageClass
    | Bitcast of resType : uint32 * resId : uint32 * operand : uint32
    | VectorExtractDynamic of resType : uint32 * resId : uint32 * vector : uint32 * index : uint32
    | VectorInsertDynamic of resType : uint32 * resId : uint32 * vector : uint32 * comp : uint32 * index : uint32
    | VectorShuffle of resType : uint32 * resId : uint32 * vec1 : uint32 * vec2 : uint32 * components : uint32[]
    | CompositeConstruct of resType : uint32 * resId : uint32 * components : uint32[]
    | CompositeExtract of resType : uint32 * resId : uint32 * composite : uint32 * components : uint32[]
    | CompositeInsert of resType : uint32 * resId : uint32 * value : uint32 * composite : uint32 * components : uint32[]
    | CopyObject of resType : uint32 * resId : uint32 * operand : uint32
    | Transpose of resType : uint32 * resId : uint32 * matrix : uint32


module SpirVReader = 
    let private ofRawInstruction (i : RawInstruction) = 
        let args = i.operands
        match i.opCode with
            | OpCode.Nop -> Nop
            | OpCode.Undef -> Undef(args.UInt32 0, args.UInt32 1)
            | OpCode.Source -> Source(args.UInt32 0 |> unbox<SourceLanguage>, args.UInt32 1)
            | OpCode.SourceExtension -> SourceExtension(args.String 0)
            | OpCode.Name -> Name(args.UInt32 0)
            | OpCode.MemberName -> MemberName(args.UInt32 0, args.UInt32 1, args.String 8)
            | OpCode.String -> String(args.UInt32 0, args.String 4)
            | OpCode.Line -> Line(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.Decorate -> Decorate(args.UInt32 0, args.UInt32 1 |> unbox<Decoration>, args.UInt32Array 2)
            | OpCode.MemberDecorate -> MemberDecorate(args.UInt32 0, args.UInt32 1, args.UInt32 2 |> unbox<Decoration>, args.UInt32Array 3)
            | OpCode.DecorationGroup -> DecorationGroup(args.UInt32 0)
            | OpCode.GroupDecorate -> GroupDecorate(args.UInt32 0, args.UInt32Array 1)
            | OpCode.GroupMemberDecorate -> GroupMemberDecorate(args.UInt32 0, args.UInt32Array 1)
            | OpCode.Extension -> Extension(args.String 0)
            | OpCode.ExtInstImport -> ExtInstImport(args.UInt32 0, args.String 4)
            | OpCode.ExtInst -> ExtInst(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32Array 4)
            | OpCode.MemoryModel -> MemoryModel(args.UInt32 0 |> unbox<AddressingModel>, args.UInt32 1 |> unbox<MemoryModel>)
            | OpCode.EntryPoint -> EntryPoint(args.UInt32 0 |> unbox<ExecutionModel>, args.UInt32 1, args.String 8)
            | OpCode.ExecutionMode -> ExecutionMode(args.UInt32 0, args.UInt32 1 |> unbox<ExecutionMode>, args.UInt32Array 2)
            | OpCode.Capability -> Capability(args.UInt32 0 |> unbox<Capability>)
            | OpCode.TypeVoid -> TypeVoid(args.UInt32 0)
            | OpCode.TypeBool -> TypeBool(args.UInt32 0)
            | OpCode.TypeInt -> TypeInt(args.UInt32 0, args.UInt32 1, args.UInt32 2 = 1u)
            | OpCode.TypeFloat -> TypeFloat(args.UInt32 0, args.UInt32 1)
            | OpCode.TypeVector -> TypeVector(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.TypeMatrix -> TypeMatrix(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.TypeImage -> TypeImage(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4 = 1u, args.UInt32 5 = 1u, args.UInt32 6 |> unbox<SampleMode>, args.UInt32 7 |> unbox<ImageFormat>, args.UInt32Array 8)
            | OpCode.TypeSampler -> TypeSampler(args.UInt32 0)
            | OpCode.TypeSampledImage -> TypeSampledImage(args.UInt32 0, args.UInt32 1)
            | OpCode.TypeArray -> TypeArray(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.TypeRuntimeArray -> TypeRuntimeArray(args.UInt32 0, args.UInt32 1)
            | OpCode.TypeStruct -> TypeStruct(args.UInt32 0, args.UInt32Array 1)
            | OpCode.TypeOpaque -> TypeOpaque(args.UInt32 0, args.String 4)
            | OpCode.TypePointer -> TypePointer(args.UInt32 0, args.UInt32 1 |> unbox<StorageClass>, args.UInt32 2)
            | OpCode.TypeFunction -> TypeFunction(args.UInt32 0, args.UInt32 1, args.UInt32Array 2)
            | OpCode.TypeEvent -> TypeEvent(args.UInt32 0)
            | OpCode.TypeDeviceEvent -> TypeDeviceEvent(args.UInt32 0)
            | OpCode.TypeReserveId -> TypeReserveId(args.UInt32 0)
            | OpCode.TypeQueue -> TypeQueue(args.UInt32 0)
            | OpCode.TypePipe -> TypePipe(args.UInt32 0, args.UInt32 1, args.UInt32 2 |> unbox<AccessQualifier>)
            | OpCode.ConstantTrue -> ConstantTrue(args.UInt32 0, args.UInt32 1)
            | OpCode.ConstantFalse -> ConstantFalse(args.UInt32 0, args.UInt32 1)
            | OpCode.Constant -> Constant(args.UInt32 0, args.UInt32 1, args.UInt32Array 2)
            | OpCode.ConstantComposite -> ConstantComposite(args.UInt32 0, args.UInt32 1, args.UInt32Array 2)
            | OpCode.ConstantSampler -> ConstantSampler(args.UInt32 0, args.UInt32 1, args.UInt32 2 |> unbox<SamplerAddressingMode>, args.UInt32 3 = 1u, args.UInt32 4 |> unbox<SamplerFilterMode>)
            | OpCode.ConstantNull -> ConstantNull(args.UInt32 0, args.UInt32 1)
            | OpCode.SpecConstantTrue -> SpecConstantTrue(args.UInt32 0, args.UInt32 1)
            | OpCode.SpecConstantFalse -> SpecConstantFalse(args.UInt32 0, args.UInt32 1)
            | OpCode.SpecConstant -> SpecConstant(args.UInt32 0, args.UInt32 1, args.UInt32Array 2)
            | OpCode.SpecConstantComposite -> SpecConstantComposite(args.UInt32 0, args.UInt32 1, args.UInt32Array 2)
            | OpCode.SpecConstantOp -> SpecConstantOp(args.UInt32 0, args.UInt32 1, args.UInt32 2 |> unbox<OpCode>, args.UInt32Array 3)
            | OpCode.Variable -> Variable(args.UInt32 0, args.UInt32 1, args.UInt32 2 |> unbox<StorageClass>, args.UInt32Array 3)
            | OpCode.ImageTexelPointer -> ImageTexelPointer(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Dim>, args.UInt32 4)
            | OpCode.Load -> Load(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32Array 3)
            | OpCode.Store -> Store(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32Array 4)
            | OpCode.CopyMemory -> CopyMemory(args.UInt32 0, args.UInt32 1, args.UInt32Array 2)
            | OpCode.CopyMemorySized -> CopyMemorySized(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32Array 3)
            | OpCode.AccessChain -> AccessChain(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32Array 3)
            | OpCode.InBoundsAccessChain -> InBoundsAccessChain(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32Array 3)
            | OpCode.PtrAccessChain -> PtrAccessChain(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32Array 4)
            | OpCode.ArrayLength -> ArrayLength(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.GenericPtrMemSemantics -> GenericPtrMemSemantics(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.Function -> Function(args.UInt32 0, args.UInt32 1, args.UInt32 2 |> unbox<FunctionControl>, args.UInt32 3)
            | OpCode.FunctionParameter -> FunctionParameter(args.UInt32 0, args.UInt32 1)
            | OpCode.FunctionEnd -> FunctionEnd
            | OpCode.FunctionCall -> FunctionCall(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32Array 3)
            | OpCode.SampledImage -> SampledImage(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.ImageSampleImplicitLod -> ImageSampleImplicitLod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32Array 4)
            | OpCode.ImageSampleExplicitLod -> ImageSampleExplicitLod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32Array 4)
            | OpCode.ImageSampleDrefImplicitLod -> ImageSampleDrefImplicitLod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4, args.UInt32Array 5)
            | OpCode.ImageSampleDrefExplicitLod -> ImageSampleDrefExplicitLod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4, args.UInt32Array 5)
            | OpCode.ImageSampleProjImplicitLod -> ImageSampleProjImplicitLod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32Array 4)
            | OpCode.ImageSampleProjExplicitLod -> ImageSampleProjExplicitLod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32Array 4)
            | OpCode.ImageSampleProjDrefImplicitLod -> ImageSampleProjDrefImplicitLod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4, args.UInt32Array 5)
            | OpCode.ImageSampleProjDrefExplicitLod -> ImageSampleProjDrefExplicitLod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4, args.UInt32Array 5)
            | OpCode.ImageFetch -> ImageFetch(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32Array 4)
            | OpCode.ImageGather -> ImageGather(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4, args.UInt32Array 5)
            | OpCode.ImageDrefGather -> ImageDrefGather(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4, args.UInt32Array 5)
            | OpCode.ImageRead -> ImageRead(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.ImageWrite -> ImageWrite(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ImageQueryDim -> ImageQueryDim(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ImageQueryFormat -> ImageQueryFormat(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ImageQueryOrder -> ImageQueryOrder(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ImageQuerySizeLod -> ImageQuerySizeLod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.ImageQuerySize -> ImageQuerySize(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ImageQueryLod -> ImageQueryLod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.ImageQueryLevels -> ImageQueryLevels(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ImageQuerySamples -> ImageQuerySamples(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ConvertFToU -> ConvertFToU(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ConvertFToS -> ConvertFToS(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ConvertSToF -> ConvertSToF(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ConvertUToF -> ConvertUToF(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.UConvert -> UConvert(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.SConvert -> SConvert(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.FConvert -> FConvert(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.QuantizeToF16 -> QuantizeToF16(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ConvertPtrToU -> ConvertPtrToU(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.SatConvertSToU -> SatConvertSToU(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.SatConvertUToS -> SatConvertUToS(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.ConvertUToPtr -> ConvertUToPtr(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.PtrCastToGeneric -> PtrCastToGeneric(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.GenericCastToPtr -> GenericCastToPtr(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.GenericCastToPtrExplicit -> GenericCastToPtrExplicit(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<StorageClass>)
            | OpCode.Bitcast -> Bitcast(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.VectorExtractDynamic -> VectorExtractDynamic(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.VectorInsertDynamic -> VectorInsertDynamic(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4)
            | OpCode.VectorShuffle -> VectorShuffle(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32Array 4)
            | OpCode.CompositeConstruct -> CompositeConstruct(args.UInt32 0, args.UInt32 1, args.UInt32Array 2)
            | OpCode.CompositeExtract -> CompositeExtract(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32Array 3)
            | OpCode.CompositeInsert -> CompositeInsert(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32Array 4)
            | OpCode.CopyObject -> CopyObject(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.Transpose -> Transpose(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | code -> failwithf "unknown OpCode: %A" code

    let readStream (i : Stream) = 
        let m = RawReader.read i
        m.instructions |> List.map ofRawInstruction


module SpirVWriter = 
    let private toRawInstruction (i : Instruction) = 
        match i with
            | Nop -> { opCode = OpCode.Nop; operands = RawOperands() }
            | Undef(resType, resId) -> { opCode = OpCode.Undef; operands = RawOperands(resType, resId) }
            | Source(language, version) -> { opCode = OpCode.Source; operands = RawOperands(language, version) }
            | SourceExtension(extension) -> { opCode = OpCode.SourceExtension; operands = RawOperands(extension) }
            | Name(target) -> { opCode = OpCode.Name; operands = RawOperands(target) }
            | MemberName(_type, mem, name) -> { opCode = OpCode.MemberName; operands = RawOperands(_type, mem, name) }
            | String(resId, value) -> { opCode = OpCode.String; operands = RawOperands(resId, value) }
            | Line(target, file, line, col) -> { opCode = OpCode.Line; operands = RawOperands(target, file, line, col) }
            | Decorate(target, decoration, args) -> { opCode = OpCode.Decorate; operands = RawOperands(target, decoration, args) }
            | MemberDecorate(structureType, mem, decoration, args) -> { opCode = OpCode.MemberDecorate; operands = RawOperands(structureType, mem, decoration, args) }
            | DecorationGroup(resId) -> { opCode = OpCode.DecorationGroup; operands = RawOperands(resId) }
            | GroupDecorate(decorationGroup, targets) -> { opCode = OpCode.GroupDecorate; operands = RawOperands(decorationGroup, targets) }
            | GroupMemberDecorate(decorationGroup, idLiteralPairs) -> { opCode = OpCode.GroupMemberDecorate; operands = RawOperands(decorationGroup, idLiteralPairs) }
            | Extension(extName) -> { opCode = OpCode.Extension; operands = RawOperands(extName) }
            | ExtInstImport(resId, name) -> { opCode = OpCode.ExtInstImport; operands = RawOperands(resId, name) }
            | ExtInst(resType, resId, set, instruction, args) -> { opCode = OpCode.ExtInst; operands = RawOperands(resType, resId, set, instruction, args) }
            | MemoryModel(addressingModel, memoryModel) -> { opCode = OpCode.MemoryModel; operands = RawOperands(addressingModel, memoryModel) }
            | EntryPoint(execModel, id, name) -> { opCode = OpCode.EntryPoint; operands = RawOperands(execModel, id, name) }
            | ExecutionMode(entryPoint, mode, args) -> { opCode = OpCode.ExecutionMode; operands = RawOperands(entryPoint, mode, args) }
            | Capability(cap) -> { opCode = OpCode.Capability; operands = RawOperands(cap) }
            | TypeVoid(resId) -> { opCode = OpCode.TypeVoid; operands = RawOperands(resId) }
            | TypeBool(resId) -> { opCode = OpCode.TypeBool; operands = RawOperands(resId) }
            | TypeInt(resId, width, signed) -> { opCode = OpCode.TypeInt; operands = RawOperands(resId, width, signed) }
            | TypeFloat(resId, width) -> { opCode = OpCode.TypeFloat; operands = RawOperands(resId, width) }
            | TypeVector(resId, compType, compCount) -> { opCode = OpCode.TypeVector; operands = RawOperands(resId, compType, compCount) }
            | TypeMatrix(resId, colType, colCount) -> { opCode = OpCode.TypeMatrix; operands = RawOperands(resId, colType, colCount) }
            | TypeImage(resId, sampledType, dim, depth, arrayed, ms, sampled, format, access) -> { opCode = OpCode.TypeImage; operands = RawOperands(resId, sampledType, dim, depth, arrayed, ms, sampled, format, access) }
            | TypeSampler(resId) -> { opCode = OpCode.TypeSampler; operands = RawOperands(resId) }
            | TypeSampledImage(resId, imageType) -> { opCode = OpCode.TypeSampledImage; operands = RawOperands(resId, imageType) }
            | TypeArray(resId, elemType, length) -> { opCode = OpCode.TypeArray; operands = RawOperands(resId, elemType, length) }
            | TypeRuntimeArray(resId, elemType) -> { opCode = OpCode.TypeRuntimeArray; operands = RawOperands(resId, elemType) }
            | TypeStruct(resId, fieldTypes) -> { opCode = OpCode.TypeStruct; operands = RawOperands(resId, fieldTypes) }
            | TypeOpaque(resId, name) -> { opCode = OpCode.TypeOpaque; operands = RawOperands(resId, name) }
            | TypePointer(resId, sClass, _type) -> { opCode = OpCode.TypePointer; operands = RawOperands(resId, sClass, _type) }
            | TypeFunction(resId, retType, argTypes) -> { opCode = OpCode.TypeFunction; operands = RawOperands(resId, retType, argTypes) }
            | TypeEvent(resId) -> { opCode = OpCode.TypeEvent; operands = RawOperands(resId) }
            | TypeDeviceEvent(resId) -> { opCode = OpCode.TypeDeviceEvent; operands = RawOperands(resId) }
            | TypeReserveId(resId) -> { opCode = OpCode.TypeReserveId; operands = RawOperands(resId) }
            | TypeQueue(resId) -> { opCode = OpCode.TypeQueue; operands = RawOperands(resId) }
            | TypePipe(resId, _type, access) -> { opCode = OpCode.TypePipe; operands = RawOperands(resId, _type, access) }
            | ConstantTrue(resType, resId) -> { opCode = OpCode.ConstantTrue; operands = RawOperands(resType, resId) }
            | ConstantFalse(resType, resId) -> { opCode = OpCode.ConstantFalse; operands = RawOperands(resType, resId) }
            | Constant(resType, resId, values) -> { opCode = OpCode.Constant; operands = RawOperands(resType, resId, values) }
            | ConstantComposite(resType, resId, constituents) -> { opCode = OpCode.ConstantComposite; operands = RawOperands(resType, resId, constituents) }
            | ConstantSampler(resType, resId, addressingMode, normalized, filter) -> { opCode = OpCode.ConstantSampler; operands = RawOperands(resType, resId, addressingMode, normalized, filter) }
            | ConstantNull(resType, resId) -> { opCode = OpCode.ConstantNull; operands = RawOperands(resType, resId) }
            | SpecConstantTrue(resType, resId) -> { opCode = OpCode.SpecConstantTrue; operands = RawOperands(resType, resId) }
            | SpecConstantFalse(resType, resId) -> { opCode = OpCode.SpecConstantFalse; operands = RawOperands(resType, resId) }
            | SpecConstant(resType, resId, values) -> { opCode = OpCode.SpecConstant; operands = RawOperands(resType, resId, values) }
            | SpecConstantComposite(resType, resId, constituents) -> { opCode = OpCode.SpecConstantComposite; operands = RawOperands(resType, resId, constituents) }
            | SpecConstantOp(resType, resId, opCode, operands) -> { opCode = OpCode.SpecConstantOp; operands = RawOperands(resType, resId, opCode, operands) }
            | Variable(resType, resId, storageClas, initializers) -> { opCode = OpCode.Variable; operands = RawOperands(resType, resId, storageClas, initializers) }
            | ImageTexelPointer(resType, resId, image, coordinate, sample) -> { opCode = OpCode.ImageTexelPointer; operands = RawOperands(resType, resId, image, coordinate, sample) }
            | Load(resType, resId, pointer, memoryAccess) -> { opCode = OpCode.Load; operands = RawOperands(resType, resId, pointer, memoryAccess) }
            | Store(resType, resId, pointer, ob, memoryAccess) -> { opCode = OpCode.Store; operands = RawOperands(resType, resId, pointer, ob, memoryAccess) }
            | CopyMemory(target, source, memoryAccess) -> { opCode = OpCode.CopyMemory; operands = RawOperands(target, source, memoryAccess) }
            | CopyMemorySized(target, source, size, memoryAccess) -> { opCode = OpCode.CopyMemorySized; operands = RawOperands(target, source, size, memoryAccess) }
            | AccessChain(resType, resId, _base, indices) -> { opCode = OpCode.AccessChain; operands = RawOperands(resType, resId, _base, indices) }
            | InBoundsAccessChain(resType, resId, _base, indices) -> { opCode = OpCode.InBoundsAccessChain; operands = RawOperands(resType, resId, _base, indices) }
            | PtrAccessChain(resType, resId, _base, element, indices) -> { opCode = OpCode.PtrAccessChain; operands = RawOperands(resType, resId, _base, element, indices) }
            | ArrayLength(resType, resId, structure, arrMember) -> { opCode = OpCode.ArrayLength; operands = RawOperands(resType, resId, structure, arrMember) }
            | GenericPtrMemSemantics(resType, resId, ptr) -> { opCode = OpCode.GenericPtrMemSemantics; operands = RawOperands(resType, resId, ptr) }
            | Function(resType, resId, ctrl, _type) -> { opCode = OpCode.Function; operands = RawOperands(resType, resId, ctrl, _type) }
            | FunctionParameter(resType, resId) -> { opCode = OpCode.FunctionParameter; operands = RawOperands(resType, resId) }
            | FunctionEnd -> { opCode = OpCode.FunctionEnd; operands = RawOperands() }
            | FunctionCall(resType, resId, f, args) -> { opCode = OpCode.FunctionCall; operands = RawOperands(resType, resId, f, args) }
            | SampledImage(resType, resId, image, sampler) -> { opCode = OpCode.SampledImage; operands = RawOperands(resType, resId, image, sampler) }
            | ImageSampleImplicitLod(resType, resId, image, coordinate, operands) -> { opCode = OpCode.ImageSampleImplicitLod; operands = RawOperands(resType, resId, image, coordinate, operands) }
            | ImageSampleExplicitLod(resType, resId, image, coordinate, operands) -> { opCode = OpCode.ImageSampleExplicitLod; operands = RawOperands(resType, resId, image, coordinate, operands) }
            | ImageSampleDrefImplicitLod(resType, resId, image, coordinate, dref, operands) -> { opCode = OpCode.ImageSampleDrefImplicitLod; operands = RawOperands(resType, resId, image, coordinate, dref, operands) }
            | ImageSampleDrefExplicitLod(resType, resId, image, coordinate, dref, operands) -> { opCode = OpCode.ImageSampleDrefExplicitLod; operands = RawOperands(resType, resId, image, coordinate, dref, operands) }
            | ImageSampleProjImplicitLod(resType, resId, image, coordinate, operands) -> { opCode = OpCode.ImageSampleProjImplicitLod; operands = RawOperands(resType, resId, image, coordinate, operands) }
            | ImageSampleProjExplicitLod(resType, resId, image, coordinate, operands) -> { opCode = OpCode.ImageSampleProjExplicitLod; operands = RawOperands(resType, resId, image, coordinate, operands) }
            | ImageSampleProjDrefImplicitLod(resType, resId, image, coordinate, dref, operands) -> { opCode = OpCode.ImageSampleProjDrefImplicitLod; operands = RawOperands(resType, resId, image, coordinate, dref, operands) }
            | ImageSampleProjDrefExplicitLod(resType, resId, image, coordinate, dref, operands) -> { opCode = OpCode.ImageSampleProjDrefExplicitLod; operands = RawOperands(resType, resId, image, coordinate, dref, operands) }
            | ImageFetch(resType, resId, image, coordinate, operands) -> { opCode = OpCode.ImageFetch; operands = RawOperands(resType, resId, image, coordinate, operands) }
            | ImageGather(resType, resId, image, coordinate, comp, operands) -> { opCode = OpCode.ImageGather; operands = RawOperands(resType, resId, image, coordinate, comp, operands) }
            | ImageDrefGather(resType, resId, image, coordinate, dref, operands) -> { opCode = OpCode.ImageDrefGather; operands = RawOperands(resType, resId, image, coordinate, dref, operands) }
            | ImageRead(resType, resId, image, coordinate) -> { opCode = OpCode.ImageRead; operands = RawOperands(resType, resId, image, coordinate) }
            | ImageWrite(image, coordinate, texel) -> { opCode = OpCode.ImageWrite; operands = RawOperands(image, coordinate, texel) }
            | ImageQueryDim(resType, resId, image) -> { opCode = OpCode.ImageQueryDim; operands = RawOperands(resType, resId, image) }
            | ImageQueryFormat(resType, resId, image) -> { opCode = OpCode.ImageQueryFormat; operands = RawOperands(resType, resId, image) }
            | ImageQueryOrder(resType, resId, image) -> { opCode = OpCode.ImageQueryOrder; operands = RawOperands(resType, resId, image) }
            | ImageQuerySizeLod(resType, resId, image, level) -> { opCode = OpCode.ImageQuerySizeLod; operands = RawOperands(resType, resId, image, level) }
            | ImageQuerySize(resType, resId, image) -> { opCode = OpCode.ImageQuerySize; operands = RawOperands(resType, resId, image) }
            | ImageQueryLod(resType, resId, image, coordinate) -> { opCode = OpCode.ImageQueryLod; operands = RawOperands(resType, resId, image, coordinate) }
            | ImageQueryLevels(resType, resId, image) -> { opCode = OpCode.ImageQueryLevels; operands = RawOperands(resType, resId, image) }
            | ImageQuerySamples(resType, resId, image) -> { opCode = OpCode.ImageQuerySamples; operands = RawOperands(resType, resId, image) }
            | ConvertFToU(resType, resId, value) -> { opCode = OpCode.ConvertFToU; operands = RawOperands(resType, resId, value) }
            | ConvertFToS(resType, resId, value) -> { opCode = OpCode.ConvertFToS; operands = RawOperands(resType, resId, value) }
            | ConvertSToF(resType, resId, value) -> { opCode = OpCode.ConvertSToF; operands = RawOperands(resType, resId, value) }
            | ConvertUToF(resType, resId, value) -> { opCode = OpCode.ConvertUToF; operands = RawOperands(resType, resId, value) }
            | UConvert(resType, resId, value) -> { opCode = OpCode.UConvert; operands = RawOperands(resType, resId, value) }
            | SConvert(resType, resId, value) -> { opCode = OpCode.SConvert; operands = RawOperands(resType, resId, value) }
            | FConvert(resType, resId, value) -> { opCode = OpCode.FConvert; operands = RawOperands(resType, resId, value) }
            | QuantizeToF16(resType, resId, value) -> { opCode = OpCode.QuantizeToF16; operands = RawOperands(resType, resId, value) }
            | ConvertPtrToU(resType, resId, value) -> { opCode = OpCode.ConvertPtrToU; operands = RawOperands(resType, resId, value) }
            | SatConvertSToU(resType, resId, value) -> { opCode = OpCode.SatConvertSToU; operands = RawOperands(resType, resId, value) }
            | SatConvertUToS(resType, resId, value) -> { opCode = OpCode.SatConvertUToS; operands = RawOperands(resType, resId, value) }
            | ConvertUToPtr(resType, resId, value) -> { opCode = OpCode.ConvertUToPtr; operands = RawOperands(resType, resId, value) }
            | PtrCastToGeneric(resType, resId, value) -> { opCode = OpCode.PtrCastToGeneric; operands = RawOperands(resType, resId, value) }
            | GenericCastToPtr(resType, resId, value) -> { opCode = OpCode.GenericCastToPtr; operands = RawOperands(resType, resId, value) }
            | GenericCastToPtrExplicit(resType, resId, value, _class) -> { opCode = OpCode.GenericCastToPtrExplicit; operands = RawOperands(resType, resId, value, _class) }
            | Bitcast(resType, resId, operand) -> { opCode = OpCode.Bitcast; operands = RawOperands(resType, resId, operand) }
            | VectorExtractDynamic(resType, resId, vector, index) -> { opCode = OpCode.VectorExtractDynamic; operands = RawOperands(resType, resId, vector, index) }
            | VectorInsertDynamic(resType, resId, vector, comp, index) -> { opCode = OpCode.VectorInsertDynamic; operands = RawOperands(resType, resId, vector, comp, index) }
            | VectorShuffle(resType, resId, vec1, vec2, components) -> { opCode = OpCode.VectorShuffle; operands = RawOperands(resType, resId, vec1, vec2, components) }
            | CompositeConstruct(resType, resId, components) -> { opCode = OpCode.CompositeConstruct; operands = RawOperands(resType, resId, components) }
            | CompositeExtract(resType, resId, composite, components) -> { opCode = OpCode.CompositeExtract; operands = RawOperands(resType, resId, composite, components) }
            | CompositeInsert(resType, resId, value, composite, components) -> { opCode = OpCode.CompositeInsert; operands = RawOperands(resType, resId, value, composite, components) }
            | CopyObject(resType, resId, operand) -> { opCode = OpCode.CopyObject; operands = RawOperands(resType, resId, operand) }
            | Transpose(resType, resId, matrix) -> { opCode = OpCode.Transpose; operands = RawOperands(resType, resId, matrix) }

    let writeStream (o : Stream) (instructions : list<Instruction>) = 
        let raw = instructions |> List.map toRawInstruction
        RawWriter.write o raw


