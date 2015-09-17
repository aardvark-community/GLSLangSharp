namespace GLSLang.SpirV

open System.IO

type Instruction = 
    | Nop
    | Undef of resType : uint32 * resId : uint32
    | Source of language : SourceLanguage * version : uint32
    | SourceExtension of extension : string
    | Name of target : uint32 * name : string
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
    | TypeImage of resId : uint32 * sampledType : uint32 * dim : uint32 * depth : bool * arrayed : bool * ms : bool * sampled : SampleMode * format : ImageFormat * access : uint32[]
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
    | Store of pointer : uint32 * ob : uint32 * memoryAccess : uint32[]
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
    | SNegate of resType : uint32 * resId : uint32 * value : uint32
    | FNegate of resType : uint32 * resId : uint32 * value : uint32
    | IAdd of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FAdd of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | ISub of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FSub of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | IMul of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FMul of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | UDiv of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | SDiv of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FDiv of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | UMod of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | SRem of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | SMod of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FRem of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FMod of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | VectorTimesScalar of resType : uint32 * resId : uint32 * v : uint32 * s : uint32
    | MatrixTimesScalar of resType : uint32 * resId : uint32 * m : uint32 * s : uint32
    | VectorTimesMatrix of resType : uint32 * resId : uint32 * v : uint32 * m : uint32
    | MatrixTimesVector of resType : uint32 * resId : uint32 * m : uint32 * v : uint32
    | MatrixTimesMatrix of resType : uint32 * resId : uint32 * m0 : uint32 * m1 : uint32
    | OuterProduct of resType : uint32 * resId : uint32 * v0 : uint32 * v1 : uint32
    | Dot of resType : uint32 * resId : uint32 * v0 : uint32 * v1 : uint32
    | IAddCarry of resType : uint32 * resId : uint32
    | ISubBorrow of resType : uint32 * resId : uint32
    | IMulExtended of resType : uint32 * resId : uint32
    | ShiftRightLogical of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | ShiftRightArithmetic of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | ShiftLeftLogical of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | BitwiseOr of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | BitwiseXor of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | BitwiseAnd of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | Not of resType : uint32 * resId : uint32 * v : uint32
    | BitFieldInsert of resType : uint32 * resId : uint32 * _base : uint32 * insert : uint32 * offset : uint32 * count : uint32
    | BitFieldSExtract of resType : uint32 * resId : uint32 * _base : uint32 * offset : uint32 * count : uint32
    | BitFieldUExtract of resType : uint32 * resId : uint32 * _base : uint32 * offset : uint32 * count : uint32
    | BitReverse of resType : uint32 * resId : uint32 * _base : uint32
    | BitCount of resType : uint32 * resId : uint32 * _base : uint32
    | Any of resType : uint32 * resId : uint32 * vec : uint32
    | All of resType : uint32 * resId : uint32 * vec : uint32
    | IsNan of resType : uint32 * resId : uint32 * v : uint32
    | IsInf of resType : uint32 * resId : uint32 * v : uint32
    | IsFinite of resType : uint32 * resId : uint32 * v : uint32
    | IsNormal of resType : uint32 * resId : uint32 * v : uint32
    | SignBitSet of resType : uint32 * resId : uint32 * v : uint32
    | LessOrGreater of resType : uint32 * resId : uint32 * x : uint32 * y : uint32
    | Ordered of resType : uint32 * resId : uint32 * x : uint32 * y : uint32
    | Unordered of resType : uint32 * resId : uint32 * x : uint32 * y : uint32
    | LogicalEqual of resType : uint32 * resId : uint32 * x : uint32 * y : uint32
    | LogicalNotEqual of resType : uint32 * resId : uint32 * x : uint32 * y : uint32
    | LogicalOr of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | LogicalAnd of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | LogicalNot of resType : uint32 * resId : uint32 * v : uint32
    | Select of resType : uint32 * resId : uint32 * guard : uint32 * vtrue : uint32 * vfalse : uint32
    | IEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | INotEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | UGreaterThan of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | SGreaterThan of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | UGreaterThanEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | SGreaterThanEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | ULessThan of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | SLessThan of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | ULessThanEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | SLessThanEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FOrdEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FUnordEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FOrdNotEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FUnordNotEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FOrdLessThan of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FUnordLessThan of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FOrdGreaterThan of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FUnordGreaterThan of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FOrdLessThanEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FUnordLessThanEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FOrdGreaterThanEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | FUnordGreaterThanEqual of resType : uint32 * resId : uint32 * l : uint32 * r : uint32
    | DPdx of resType : uint32 * resId : uint32 * p : uint32
    | DPdy of resType : uint32 * resId : uint32 * p : uint32
    | Fwidth of resType : uint32 * resId : uint32 * p : uint32
    | DPdxFine of resType : uint32 * resId : uint32 * p : uint32
    | DPdyFine of resType : uint32 * resId : uint32 * p : uint32
    | FwidthFine of resType : uint32 * resId : uint32 * p : uint32
    | DPdxCoarse of resType : uint32 * resId : uint32 * p : uint32
    | DPdyCoarse of resType : uint32 * resId : uint32 * p : uint32
    | FwidthCoarse of resType : uint32 * resId : uint32 * p : uint32
    | Phi of resType : uint32 * resId : uint32 * variableParentPairs : uint32[]
    | LoopMerge of mergeBlock : uint32 * ctrl : LoopControl
    | SelectionMerge of mergeBlock : uint32 * ctrl : SelectionControl
    | Label of resId : uint32
    | Branch of target : uint32
    | BranchConditional of condition : uint32 * trueLabel : uint32 * falseLabel : uint32 * branchWeights : uint32[]
    | Switch of selector : uint32 * _default : uint32 * literalLabelPairs : uint32[]
    | Kill
    | Return
    | ReturnValue of value : uint32
    | Unreachable
    | LifetimeStart of ptr : uint32 * size : uint32
    | LifetimeStop of ptr : uint32 * size : uint32
    | AtomicLoad of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics
    | AtomicStore of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * value : uint32
    | AtomicExchange of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics * value : uint32
    | AtomicCompareExchange of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * _equal : MemorySemantics * _unequal : MemorySemantics * value : uint32 * cmp : uint32
    | AtomicCompareExchangeWeak of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * _equal : MemorySemantics * _unequal : MemorySemantics * value : uint32 * cmp : uint32
    | AtomicIIncrement of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics
    | AtomicIDecrement of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics
    | AtomicIAdd of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics * value : uint32
    | AtomicISub of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics * value : uint32
    | AtomicSMin of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics * value : uint32
    | AtomicUMin of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics * value : uint32
    | AtomicSMax of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics * value : uint32
    | AtomicUMax of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics * value : uint32
    | AtomicAnd of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics * value : uint32
    | AtomicOr of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics * value : uint32
    | AtomicXor of resType : uint32 * resId : uint32 * ptr : uint32 * scope : Scope * sem : MemorySemantics * value : uint32
    | EmitVertex
    | EndPrimitive
    | EmitStreamVertex of stream : uint32
    | EndStreamPrimitive of stream : uint32
    | ControlBarrier of exec : Scope * mem : Scope * sem : MemorySemantics
    | MemoryBarrier of mem : Scope * sem : MemorySemantics


module private SpirVUtilities = 
    let private ofRawInstruction (i : RawInstruction) = 
        let args = i.operands
        match i.opCode with
            | OpCode.Nop -> Nop
            | OpCode.Undef -> Undef(args.UInt32 0, args.UInt32 1)
            | OpCode.Source -> Source(args.UInt32 0 |> unbox<SourceLanguage>, args.UInt32 1)
            | OpCode.SourceExtension -> SourceExtension(args.String 0)
            | OpCode.Name -> Name(args.UInt32 0, args.String 4)
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
            | OpCode.TypeImage -> TypeImage(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 = 1u, args.UInt32 4 = 1u, args.UInt32 5 = 1u, args.UInt32 6 |> unbox<SampleMode>, args.UInt32 7 |> unbox<ImageFormat>, args.UInt32Array 8)
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
            | OpCode.Store -> Store(args.UInt32 0, args.UInt32 1, args.UInt32Array 2)
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
            | OpCode.SNegate -> SNegate(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.FNegate -> FNegate(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.IAdd -> IAdd(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FAdd -> FAdd(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.ISub -> ISub(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FSub -> FSub(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.IMul -> IMul(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FMul -> FMul(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.UDiv -> UDiv(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.SDiv -> SDiv(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FDiv -> FDiv(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.UMod -> UMod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.SRem -> SRem(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.SMod -> SMod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FRem -> FRem(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FMod -> FMod(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.VectorTimesScalar -> VectorTimesScalar(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.MatrixTimesScalar -> MatrixTimesScalar(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.VectorTimesMatrix -> VectorTimesMatrix(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.MatrixTimesVector -> MatrixTimesVector(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.MatrixTimesMatrix -> MatrixTimesMatrix(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.OuterProduct -> OuterProduct(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.Dot -> Dot(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.IAddCarry -> IAddCarry(args.UInt32 0, args.UInt32 1)
            | OpCode.ISubBorrow -> ISubBorrow(args.UInt32 0, args.UInt32 1)
            | OpCode.IMulExtended -> IMulExtended(args.UInt32 0, args.UInt32 1)
            | OpCode.ShiftRightLogical -> ShiftRightLogical(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.ShiftRightArithmetic -> ShiftRightArithmetic(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.ShiftLeftLogical -> ShiftLeftLogical(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.BitwiseOr -> BitwiseOr(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.BitwiseXor -> BitwiseXor(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.BitwiseAnd -> BitwiseAnd(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.Not -> Not(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.BitFieldInsert -> BitFieldInsert(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4, args.UInt32 5)
            | OpCode.BitFieldSExtract -> BitFieldSExtract(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4)
            | OpCode.BitFieldUExtract -> BitFieldUExtract(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4)
            | OpCode.BitReverse -> BitReverse(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.BitCount -> BitCount(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.Any -> Any(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.All -> All(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.IsNan -> IsNan(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.IsInf -> IsInf(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.IsFinite -> IsFinite(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.IsNormal -> IsNormal(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.SignBitSet -> SignBitSet(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.LessOrGreater -> LessOrGreater(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.Ordered -> Ordered(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.Unordered -> Unordered(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.LogicalEqual -> LogicalEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.LogicalNotEqual -> LogicalNotEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.LogicalOr -> LogicalOr(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.LogicalAnd -> LogicalAnd(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.LogicalNot -> LogicalNot(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.Select -> Select(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3, args.UInt32 4)
            | OpCode.IEqual -> IEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.INotEqual -> INotEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.UGreaterThan -> UGreaterThan(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.SGreaterThan -> SGreaterThan(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.UGreaterThanEqual -> UGreaterThanEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.SGreaterThanEqual -> SGreaterThanEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.ULessThan -> ULessThan(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.SLessThan -> SLessThan(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.ULessThanEqual -> ULessThanEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.SLessThanEqual -> SLessThanEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FOrdEqual -> FOrdEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FUnordEqual -> FUnordEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FOrdNotEqual -> FOrdNotEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FUnordNotEqual -> FUnordNotEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FOrdLessThan -> FOrdLessThan(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FUnordLessThan -> FUnordLessThan(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FOrdGreaterThan -> FOrdGreaterThan(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FUnordGreaterThan -> FUnordGreaterThan(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FOrdLessThanEqual -> FOrdLessThanEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FUnordLessThanEqual -> FUnordLessThanEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FOrdGreaterThanEqual -> FOrdGreaterThanEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.FUnordGreaterThanEqual -> FUnordGreaterThanEqual(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3)
            | OpCode.DPdx -> DPdx(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.DPdy -> DPdy(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.Fwidth -> Fwidth(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.DPdxFine -> DPdxFine(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.DPdyFine -> DPdyFine(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.FwidthFine -> FwidthFine(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.DPdxCoarse -> DPdxCoarse(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.DPdyCoarse -> DPdyCoarse(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.FwidthCoarse -> FwidthCoarse(args.UInt32 0, args.UInt32 1, args.UInt32 2)
            | OpCode.Phi -> Phi(args.UInt32 0, args.UInt32 1, args.UInt32Array 2)
            | OpCode.LoopMerge -> LoopMerge(args.UInt32 0, args.UInt32 1 |> unbox<LoopControl>)
            | OpCode.SelectionMerge -> SelectionMerge(args.UInt32 0, args.UInt32 1 |> unbox<SelectionControl>)
            | OpCode.Label -> Label(args.UInt32 0)
            | OpCode.Branch -> Branch(args.UInt32 0)
            | OpCode.BranchConditional -> BranchConditional(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32Array 3)
            | OpCode.Switch -> Switch(args.UInt32 0, args.UInt32 1, args.UInt32Array 2)
            | OpCode.Kill -> Kill
            | OpCode.Return -> Return
            | OpCode.ReturnValue -> ReturnValue(args.UInt32 0)
            | OpCode.Unreachable -> Unreachable
            | OpCode.LifetimeStart -> LifetimeStart(args.UInt32 0, args.UInt32 1)
            | OpCode.LifetimeStop -> LifetimeStop(args.UInt32 0, args.UInt32 1)
            | OpCode.AtomicLoad -> AtomicLoad(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>)
            | OpCode.AtomicStore -> AtomicStore(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4)
            | OpCode.AtomicExchange -> AtomicExchange(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5)
            | OpCode.AtomicCompareExchange -> AtomicCompareExchange(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5 |> unbox<MemorySemantics>, args.UInt32 6, args.UInt32 7)
            | OpCode.AtomicCompareExchangeWeak -> AtomicCompareExchangeWeak(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5 |> unbox<MemorySemantics>, args.UInt32 6, args.UInt32 7)
            | OpCode.AtomicIIncrement -> AtomicIIncrement(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>)
            | OpCode.AtomicIDecrement -> AtomicIDecrement(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>)
            | OpCode.AtomicIAdd -> AtomicIAdd(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5)
            | OpCode.AtomicISub -> AtomicISub(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5)
            | OpCode.AtomicSMin -> AtomicSMin(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5)
            | OpCode.AtomicUMin -> AtomicUMin(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5)
            | OpCode.AtomicSMax -> AtomicSMax(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5)
            | OpCode.AtomicUMax -> AtomicUMax(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5)
            | OpCode.AtomicAnd -> AtomicAnd(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5)
            | OpCode.AtomicOr -> AtomicOr(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5)
            | OpCode.AtomicXor -> AtomicXor(args.UInt32 0, args.UInt32 1, args.UInt32 2, args.UInt32 3 |> unbox<Scope>, args.UInt32 4 |> unbox<MemorySemantics>, args.UInt32 5)
            | OpCode.EmitVertex -> EmitVertex
            | OpCode.EndPrimitive -> EndPrimitive
            | OpCode.EmitStreamVertex -> EmitStreamVertex(args.UInt32 0)
            | OpCode.EndStreamPrimitive -> EndStreamPrimitive(args.UInt32 0)
            | OpCode.ControlBarrier -> ControlBarrier(args.UInt32 0 |> unbox<Scope>, args.UInt32 1 |> unbox<Scope>, args.UInt32 2 |> unbox<MemorySemantics>)
            | OpCode.MemoryBarrier -> MemoryBarrier(args.UInt32 0 |> unbox<Scope>, args.UInt32 1 |> unbox<MemorySemantics>)
            | code -> failwithf "unknown OpCode: %A" code

    let readStream (i : Stream) = 
        let m = RawReader.read i
        m.instructions |> List.map ofRawInstruction


    let tryGetId (i : Instruction) = 
        match i with
            | Undef(_, resId) -> Some resId
            | String(resId, _) -> Some resId
            | DecorationGroup(resId) -> Some resId
            | ExtInstImport(resId, _) -> Some resId
            | ExtInst(_, resId, _, _, _) -> Some resId
            | TypeVoid(resId) -> Some resId
            | TypeBool(resId) -> Some resId
            | TypeInt(resId, _, _) -> Some resId
            | TypeFloat(resId, _) -> Some resId
            | TypeVector(resId, _, _) -> Some resId
            | TypeMatrix(resId, _, _) -> Some resId
            | TypeImage(resId, _, _, _, _, _, _, _, _) -> Some resId
            | TypeSampler(resId) -> Some resId
            | TypeSampledImage(resId, _) -> Some resId
            | TypeArray(resId, _, _) -> Some resId
            | TypeRuntimeArray(resId, _) -> Some resId
            | TypeStruct(resId, _) -> Some resId
            | TypeOpaque(resId, _) -> Some resId
            | TypePointer(resId, _, _) -> Some resId
            | TypeFunction(resId, _, _) -> Some resId
            | TypeEvent(resId) -> Some resId
            | TypeDeviceEvent(resId) -> Some resId
            | TypeReserveId(resId) -> Some resId
            | TypeQueue(resId) -> Some resId
            | TypePipe(resId, _, _) -> Some resId
            | ConstantTrue(_, resId) -> Some resId
            | ConstantFalse(_, resId) -> Some resId
            | Constant(_, resId, _) -> Some resId
            | ConstantComposite(_, resId, _) -> Some resId
            | ConstantSampler(_, resId, _, _, _) -> Some resId
            | ConstantNull(_, resId) -> Some resId
            | SpecConstantTrue(_, resId) -> Some resId
            | SpecConstantFalse(_, resId) -> Some resId
            | SpecConstant(_, resId, _) -> Some resId
            | SpecConstantComposite(_, resId, _) -> Some resId
            | SpecConstantOp(_, resId, _, _) -> Some resId
            | Variable(_, resId, _, _) -> Some resId
            | ImageTexelPointer(_, resId, _, _, _) -> Some resId
            | Load(_, resId, _, _) -> Some resId
            | AccessChain(_, resId, _, _) -> Some resId
            | InBoundsAccessChain(_, resId, _, _) -> Some resId
            | PtrAccessChain(_, resId, _, _, _) -> Some resId
            | ArrayLength(_, resId, _, _) -> Some resId
            | GenericPtrMemSemantics(_, resId, _) -> Some resId
            | Function(_, resId, _, _) -> Some resId
            | FunctionParameter(_, resId) -> Some resId
            | FunctionCall(_, resId, _, _) -> Some resId
            | SampledImage(_, resId, _, _) -> Some resId
            | ImageSampleImplicitLod(_, resId, _, _, _) -> Some resId
            | ImageSampleExplicitLod(_, resId, _, _, _) -> Some resId
            | ImageSampleDrefImplicitLod(_, resId, _, _, _, _) -> Some resId
            | ImageSampleDrefExplicitLod(_, resId, _, _, _, _) -> Some resId
            | ImageSampleProjImplicitLod(_, resId, _, _, _) -> Some resId
            | ImageSampleProjExplicitLod(_, resId, _, _, _) -> Some resId
            | ImageSampleProjDrefImplicitLod(_, resId, _, _, _, _) -> Some resId
            | ImageSampleProjDrefExplicitLod(_, resId, _, _, _, _) -> Some resId
            | ImageFetch(_, resId, _, _, _) -> Some resId
            | ImageGather(_, resId, _, _, _, _) -> Some resId
            | ImageDrefGather(_, resId, _, _, _, _) -> Some resId
            | ImageRead(_, resId, _, _) -> Some resId
            | ImageQueryDim(_, resId, _) -> Some resId
            | ImageQueryFormat(_, resId, _) -> Some resId
            | ImageQueryOrder(_, resId, _) -> Some resId
            | ImageQuerySizeLod(_, resId, _, _) -> Some resId
            | ImageQuerySize(_, resId, _) -> Some resId
            | ImageQueryLod(_, resId, _, _) -> Some resId
            | ImageQueryLevels(_, resId, _) -> Some resId
            | ImageQuerySamples(_, resId, _) -> Some resId
            | ConvertFToU(_, resId, _) -> Some resId
            | ConvertFToS(_, resId, _) -> Some resId
            | ConvertSToF(_, resId, _) -> Some resId
            | ConvertUToF(_, resId, _) -> Some resId
            | UConvert(_, resId, _) -> Some resId
            | SConvert(_, resId, _) -> Some resId
            | FConvert(_, resId, _) -> Some resId
            | QuantizeToF16(_, resId, _) -> Some resId
            | ConvertPtrToU(_, resId, _) -> Some resId
            | SatConvertSToU(_, resId, _) -> Some resId
            | SatConvertUToS(_, resId, _) -> Some resId
            | ConvertUToPtr(_, resId, _) -> Some resId
            | PtrCastToGeneric(_, resId, _) -> Some resId
            | GenericCastToPtr(_, resId, _) -> Some resId
            | GenericCastToPtrExplicit(_, resId, _, _) -> Some resId
            | Bitcast(_, resId, _) -> Some resId
            | VectorExtractDynamic(_, resId, _, _) -> Some resId
            | VectorInsertDynamic(_, resId, _, _, _) -> Some resId
            | VectorShuffle(_, resId, _, _, _) -> Some resId
            | CompositeConstruct(_, resId, _) -> Some resId
            | CompositeExtract(_, resId, _, _) -> Some resId
            | CompositeInsert(_, resId, _, _, _) -> Some resId
            | CopyObject(_, resId, _) -> Some resId
            | Transpose(_, resId, _) -> Some resId
            | SNegate(_, resId, _) -> Some resId
            | FNegate(_, resId, _) -> Some resId
            | IAdd(_, resId, _, _) -> Some resId
            | FAdd(_, resId, _, _) -> Some resId
            | ISub(_, resId, _, _) -> Some resId
            | FSub(_, resId, _, _) -> Some resId
            | IMul(_, resId, _, _) -> Some resId
            | FMul(_, resId, _, _) -> Some resId
            | UDiv(_, resId, _, _) -> Some resId
            | SDiv(_, resId, _, _) -> Some resId
            | FDiv(_, resId, _, _) -> Some resId
            | UMod(_, resId, _, _) -> Some resId
            | SRem(_, resId, _, _) -> Some resId
            | SMod(_, resId, _, _) -> Some resId
            | FRem(_, resId, _, _) -> Some resId
            | FMod(_, resId, _, _) -> Some resId
            | VectorTimesScalar(_, resId, _, _) -> Some resId
            | MatrixTimesScalar(_, resId, _, _) -> Some resId
            | VectorTimesMatrix(_, resId, _, _) -> Some resId
            | MatrixTimesVector(_, resId, _, _) -> Some resId
            | MatrixTimesMatrix(_, resId, _, _) -> Some resId
            | OuterProduct(_, resId, _, _) -> Some resId
            | Dot(_, resId, _, _) -> Some resId
            | IAddCarry(_, resId) -> Some resId
            | ISubBorrow(_, resId) -> Some resId
            | IMulExtended(_, resId) -> Some resId
            | ShiftRightLogical(_, resId, _, _) -> Some resId
            | ShiftRightArithmetic(_, resId, _, _) -> Some resId
            | ShiftLeftLogical(_, resId, _, _) -> Some resId
            | BitwiseOr(_, resId, _, _) -> Some resId
            | BitwiseXor(_, resId, _, _) -> Some resId
            | BitwiseAnd(_, resId, _, _) -> Some resId
            | Not(_, resId, _) -> Some resId
            | BitFieldInsert(_, resId, _, _, _, _) -> Some resId
            | BitFieldSExtract(_, resId, _, _, _) -> Some resId
            | BitFieldUExtract(_, resId, _, _, _) -> Some resId
            | BitReverse(_, resId, _) -> Some resId
            | BitCount(_, resId, _) -> Some resId
            | Any(_, resId, _) -> Some resId
            | All(_, resId, _) -> Some resId
            | IsNan(_, resId, _) -> Some resId
            | IsInf(_, resId, _) -> Some resId
            | IsFinite(_, resId, _) -> Some resId
            | IsNormal(_, resId, _) -> Some resId
            | SignBitSet(_, resId, _) -> Some resId
            | LessOrGreater(_, resId, _, _) -> Some resId
            | Ordered(_, resId, _, _) -> Some resId
            | Unordered(_, resId, _, _) -> Some resId
            | LogicalEqual(_, resId, _, _) -> Some resId
            | LogicalNotEqual(_, resId, _, _) -> Some resId
            | LogicalOr(_, resId, _, _) -> Some resId
            | LogicalAnd(_, resId, _, _) -> Some resId
            | LogicalNot(_, resId, _) -> Some resId
            | Select(_, resId, _, _, _) -> Some resId
            | IEqual(_, resId, _, _) -> Some resId
            | INotEqual(_, resId, _, _) -> Some resId
            | UGreaterThan(_, resId, _, _) -> Some resId
            | SGreaterThan(_, resId, _, _) -> Some resId
            | UGreaterThanEqual(_, resId, _, _) -> Some resId
            | SGreaterThanEqual(_, resId, _, _) -> Some resId
            | ULessThan(_, resId, _, _) -> Some resId
            | SLessThan(_, resId, _, _) -> Some resId
            | ULessThanEqual(_, resId, _, _) -> Some resId
            | SLessThanEqual(_, resId, _, _) -> Some resId
            | FOrdEqual(_, resId, _, _) -> Some resId
            | FUnordEqual(_, resId, _, _) -> Some resId
            | FOrdNotEqual(_, resId, _, _) -> Some resId
            | FUnordNotEqual(_, resId, _, _) -> Some resId
            | FOrdLessThan(_, resId, _, _) -> Some resId
            | FUnordLessThan(_, resId, _, _) -> Some resId
            | FOrdGreaterThan(_, resId, _, _) -> Some resId
            | FUnordGreaterThan(_, resId, _, _) -> Some resId
            | FOrdLessThanEqual(_, resId, _, _) -> Some resId
            | FUnordLessThanEqual(_, resId, _, _) -> Some resId
            | FOrdGreaterThanEqual(_, resId, _, _) -> Some resId
            | FUnordGreaterThanEqual(_, resId, _, _) -> Some resId
            | DPdx(_, resId, _) -> Some resId
            | DPdy(_, resId, _) -> Some resId
            | Fwidth(_, resId, _) -> Some resId
            | DPdxFine(_, resId, _) -> Some resId
            | DPdyFine(_, resId, _) -> Some resId
            | FwidthFine(_, resId, _) -> Some resId
            | DPdxCoarse(_, resId, _) -> Some resId
            | DPdyCoarse(_, resId, _) -> Some resId
            | FwidthCoarse(_, resId, _) -> Some resId
            | Phi(_, resId, _) -> Some resId
            | Label(resId) -> Some resId
            | AtomicLoad(_, resId, _, _, _) -> Some resId
            | AtomicStore(_, resId, _, _, _) -> Some resId
            | AtomicExchange(_, resId, _, _, _, _) -> Some resId
            | AtomicCompareExchange(_, resId, _, _, _, _, _, _) -> Some resId
            | AtomicCompareExchangeWeak(_, resId, _, _, _, _, _, _) -> Some resId
            | AtomicIIncrement(_, resId, _, _, _) -> Some resId
            | AtomicIDecrement(_, resId, _, _, _) -> Some resId
            | AtomicIAdd(_, resId, _, _, _, _) -> Some resId
            | AtomicISub(_, resId, _, _, _, _) -> Some resId
            | AtomicSMin(_, resId, _, _, _, _) -> Some resId
            | AtomicUMin(_, resId, _, _, _, _) -> Some resId
            | AtomicSMax(_, resId, _, _, _, _) -> Some resId
            | AtomicUMax(_, resId, _, _, _, _) -> Some resId
            | AtomicAnd(_, resId, _, _, _, _) -> Some resId
            | AtomicOr(_, resId, _, _, _, _) -> Some resId
            | AtomicXor(_, resId, _, _, _, _) -> Some resId
            | _ -> None

    let tryGetResultTypeId (i : Instruction) = 
        match i with
            | Undef(resType, _) -> Some resType
            | ExtInst(resType, _, _, _, _) -> Some resType
            | ConstantTrue(resType, _) -> Some resType
            | ConstantFalse(resType, _) -> Some resType
            | Constant(resType, _, _) -> Some resType
            | ConstantComposite(resType, _, _) -> Some resType
            | ConstantSampler(resType, _, _, _, _) -> Some resType
            | ConstantNull(resType, _) -> Some resType
            | SpecConstantTrue(resType, _) -> Some resType
            | SpecConstantFalse(resType, _) -> Some resType
            | SpecConstant(resType, _, _) -> Some resType
            | SpecConstantComposite(resType, _, _) -> Some resType
            | SpecConstantOp(resType, _, _, _) -> Some resType
            | Variable(resType, _, _, _) -> Some resType
            | ImageTexelPointer(resType, _, _, _, _) -> Some resType
            | Load(resType, _, _, _) -> Some resType
            | AccessChain(resType, _, _, _) -> Some resType
            | InBoundsAccessChain(resType, _, _, _) -> Some resType
            | PtrAccessChain(resType, _, _, _, _) -> Some resType
            | ArrayLength(resType, _, _, _) -> Some resType
            | GenericPtrMemSemantics(resType, _, _) -> Some resType
            | Function(resType, _, _, _) -> Some resType
            | FunctionParameter(resType, _) -> Some resType
            | FunctionCall(resType, _, _, _) -> Some resType
            | SampledImage(resType, _, _, _) -> Some resType
            | ImageSampleImplicitLod(resType, _, _, _, _) -> Some resType
            | ImageSampleExplicitLod(resType, _, _, _, _) -> Some resType
            | ImageSampleDrefImplicitLod(resType, _, _, _, _, _) -> Some resType
            | ImageSampleDrefExplicitLod(resType, _, _, _, _, _) -> Some resType
            | ImageSampleProjImplicitLod(resType, _, _, _, _) -> Some resType
            | ImageSampleProjExplicitLod(resType, _, _, _, _) -> Some resType
            | ImageSampleProjDrefImplicitLod(resType, _, _, _, _, _) -> Some resType
            | ImageSampleProjDrefExplicitLod(resType, _, _, _, _, _) -> Some resType
            | ImageFetch(resType, _, _, _, _) -> Some resType
            | ImageGather(resType, _, _, _, _, _) -> Some resType
            | ImageDrefGather(resType, _, _, _, _, _) -> Some resType
            | ImageRead(resType, _, _, _) -> Some resType
            | ImageQueryDim(resType, _, _) -> Some resType
            | ImageQueryFormat(resType, _, _) -> Some resType
            | ImageQueryOrder(resType, _, _) -> Some resType
            | ImageQuerySizeLod(resType, _, _, _) -> Some resType
            | ImageQuerySize(resType, _, _) -> Some resType
            | ImageQueryLod(resType, _, _, _) -> Some resType
            | ImageQueryLevels(resType, _, _) -> Some resType
            | ImageQuerySamples(resType, _, _) -> Some resType
            | ConvertFToU(resType, _, _) -> Some resType
            | ConvertFToS(resType, _, _) -> Some resType
            | ConvertSToF(resType, _, _) -> Some resType
            | ConvertUToF(resType, _, _) -> Some resType
            | UConvert(resType, _, _) -> Some resType
            | SConvert(resType, _, _) -> Some resType
            | FConvert(resType, _, _) -> Some resType
            | QuantizeToF16(resType, _, _) -> Some resType
            | ConvertPtrToU(resType, _, _) -> Some resType
            | SatConvertSToU(resType, _, _) -> Some resType
            | SatConvertUToS(resType, _, _) -> Some resType
            | ConvertUToPtr(resType, _, _) -> Some resType
            | PtrCastToGeneric(resType, _, _) -> Some resType
            | GenericCastToPtr(resType, _, _) -> Some resType
            | GenericCastToPtrExplicit(resType, _, _, _) -> Some resType
            | Bitcast(resType, _, _) -> Some resType
            | VectorExtractDynamic(resType, _, _, _) -> Some resType
            | VectorInsertDynamic(resType, _, _, _, _) -> Some resType
            | VectorShuffle(resType, _, _, _, _) -> Some resType
            | CompositeConstruct(resType, _, _) -> Some resType
            | CompositeExtract(resType, _, _, _) -> Some resType
            | CompositeInsert(resType, _, _, _, _) -> Some resType
            | CopyObject(resType, _, _) -> Some resType
            | Transpose(resType, _, _) -> Some resType
            | SNegate(resType, _, _) -> Some resType
            | FNegate(resType, _, _) -> Some resType
            | IAdd(resType, _, _, _) -> Some resType
            | FAdd(resType, _, _, _) -> Some resType
            | ISub(resType, _, _, _) -> Some resType
            | FSub(resType, _, _, _) -> Some resType
            | IMul(resType, _, _, _) -> Some resType
            | FMul(resType, _, _, _) -> Some resType
            | UDiv(resType, _, _, _) -> Some resType
            | SDiv(resType, _, _, _) -> Some resType
            | FDiv(resType, _, _, _) -> Some resType
            | UMod(resType, _, _, _) -> Some resType
            | SRem(resType, _, _, _) -> Some resType
            | SMod(resType, _, _, _) -> Some resType
            | FRem(resType, _, _, _) -> Some resType
            | FMod(resType, _, _, _) -> Some resType
            | VectorTimesScalar(resType, _, _, _) -> Some resType
            | MatrixTimesScalar(resType, _, _, _) -> Some resType
            | VectorTimesMatrix(resType, _, _, _) -> Some resType
            | MatrixTimesVector(resType, _, _, _) -> Some resType
            | MatrixTimesMatrix(resType, _, _, _) -> Some resType
            | OuterProduct(resType, _, _, _) -> Some resType
            | Dot(resType, _, _, _) -> Some resType
            | IAddCarry(resType, _) -> Some resType
            | ISubBorrow(resType, _) -> Some resType
            | IMulExtended(resType, _) -> Some resType
            | ShiftRightLogical(resType, _, _, _) -> Some resType
            | ShiftRightArithmetic(resType, _, _, _) -> Some resType
            | ShiftLeftLogical(resType, _, _, _) -> Some resType
            | BitwiseOr(resType, _, _, _) -> Some resType
            | BitwiseXor(resType, _, _, _) -> Some resType
            | BitwiseAnd(resType, _, _, _) -> Some resType
            | Not(resType, _, _) -> Some resType
            | BitFieldInsert(resType, _, _, _, _, _) -> Some resType
            | BitFieldSExtract(resType, _, _, _, _) -> Some resType
            | BitFieldUExtract(resType, _, _, _, _) -> Some resType
            | BitReverse(resType, _, _) -> Some resType
            | BitCount(resType, _, _) -> Some resType
            | Any(resType, _, _) -> Some resType
            | All(resType, _, _) -> Some resType
            | IsNan(resType, _, _) -> Some resType
            | IsInf(resType, _, _) -> Some resType
            | IsFinite(resType, _, _) -> Some resType
            | IsNormal(resType, _, _) -> Some resType
            | SignBitSet(resType, _, _) -> Some resType
            | LessOrGreater(resType, _, _, _) -> Some resType
            | Ordered(resType, _, _, _) -> Some resType
            | Unordered(resType, _, _, _) -> Some resType
            | LogicalEqual(resType, _, _, _) -> Some resType
            | LogicalNotEqual(resType, _, _, _) -> Some resType
            | LogicalOr(resType, _, _, _) -> Some resType
            | LogicalAnd(resType, _, _, _) -> Some resType
            | LogicalNot(resType, _, _) -> Some resType
            | Select(resType, _, _, _, _) -> Some resType
            | IEqual(resType, _, _, _) -> Some resType
            | INotEqual(resType, _, _, _) -> Some resType
            | UGreaterThan(resType, _, _, _) -> Some resType
            | SGreaterThan(resType, _, _, _) -> Some resType
            | UGreaterThanEqual(resType, _, _, _) -> Some resType
            | SGreaterThanEqual(resType, _, _, _) -> Some resType
            | ULessThan(resType, _, _, _) -> Some resType
            | SLessThan(resType, _, _, _) -> Some resType
            | ULessThanEqual(resType, _, _, _) -> Some resType
            | SLessThanEqual(resType, _, _, _) -> Some resType
            | FOrdEqual(resType, _, _, _) -> Some resType
            | FUnordEqual(resType, _, _, _) -> Some resType
            | FOrdNotEqual(resType, _, _, _) -> Some resType
            | FUnordNotEqual(resType, _, _, _) -> Some resType
            | FOrdLessThan(resType, _, _, _) -> Some resType
            | FUnordLessThan(resType, _, _, _) -> Some resType
            | FOrdGreaterThan(resType, _, _, _) -> Some resType
            | FUnordGreaterThan(resType, _, _, _) -> Some resType
            | FOrdLessThanEqual(resType, _, _, _) -> Some resType
            | FUnordLessThanEqual(resType, _, _, _) -> Some resType
            | FOrdGreaterThanEqual(resType, _, _, _) -> Some resType
            | FUnordGreaterThanEqual(resType, _, _, _) -> Some resType
            | DPdx(resType, _, _) -> Some resType
            | DPdy(resType, _, _) -> Some resType
            | Fwidth(resType, _, _) -> Some resType
            | DPdxFine(resType, _, _) -> Some resType
            | DPdyFine(resType, _, _) -> Some resType
            | FwidthFine(resType, _, _) -> Some resType
            | DPdxCoarse(resType, _, _) -> Some resType
            | DPdyCoarse(resType, _, _) -> Some resType
            | FwidthCoarse(resType, _, _) -> Some resType
            | Phi(resType, _, _) -> Some resType
            | AtomicLoad(resType, _, _, _, _) -> Some resType
            | AtomicStore(resType, _, _, _, _) -> Some resType
            | AtomicExchange(resType, _, _, _, _, _) -> Some resType
            | AtomicCompareExchange(resType, _, _, _, _, _, _, _) -> Some resType
            | AtomicCompareExchangeWeak(resType, _, _, _, _, _, _, _) -> Some resType
            | AtomicIIncrement(resType, _, _, _, _) -> Some resType
            | AtomicIDecrement(resType, _, _, _, _) -> Some resType
            | AtomicIAdd(resType, _, _, _, _, _) -> Some resType
            | AtomicISub(resType, _, _, _, _, _) -> Some resType
            | AtomicSMin(resType, _, _, _, _, _) -> Some resType
            | AtomicUMin(resType, _, _, _, _, _) -> Some resType
            | AtomicSMax(resType, _, _, _, _, _) -> Some resType
            | AtomicUMax(resType, _, _, _, _, _) -> Some resType
            | AtomicAnd(resType, _, _, _, _, _) -> Some resType
            | AtomicOr(resType, _, _, _, _, _) -> Some resType
            | AtomicXor(resType, _, _, _, _, _) -> Some resType
            | _ -> None

    let private toRawInstruction (i : Instruction) = 
        match i with
            | Nop -> { opCode = OpCode.Nop; operands = RawOperands() }
            | Undef(resType, resId) -> { opCode = OpCode.Undef; operands = RawOperands(resType, resId) }
            | Source(language, version) -> { opCode = OpCode.Source; operands = RawOperands(language, version) }
            | SourceExtension(extension) -> { opCode = OpCode.SourceExtension; operands = RawOperands(extension) }
            | Name(target, name) -> { opCode = OpCode.Name; operands = RawOperands(target, name) }
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
            | Store(pointer, ob, memoryAccess) -> { opCode = OpCode.Store; operands = RawOperands(pointer, ob, memoryAccess) }
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
            | SNegate(resType, resId, value) -> { opCode = OpCode.SNegate; operands = RawOperands(resType, resId, value) }
            | FNegate(resType, resId, value) -> { opCode = OpCode.FNegate; operands = RawOperands(resType, resId, value) }
            | IAdd(resType, resId, l, r) -> { opCode = OpCode.IAdd; operands = RawOperands(resType, resId, l, r) }
            | FAdd(resType, resId, l, r) -> { opCode = OpCode.FAdd; operands = RawOperands(resType, resId, l, r) }
            | ISub(resType, resId, l, r) -> { opCode = OpCode.ISub; operands = RawOperands(resType, resId, l, r) }
            | FSub(resType, resId, l, r) -> { opCode = OpCode.FSub; operands = RawOperands(resType, resId, l, r) }
            | IMul(resType, resId, l, r) -> { opCode = OpCode.IMul; operands = RawOperands(resType, resId, l, r) }
            | FMul(resType, resId, l, r) -> { opCode = OpCode.FMul; operands = RawOperands(resType, resId, l, r) }
            | UDiv(resType, resId, l, r) -> { opCode = OpCode.UDiv; operands = RawOperands(resType, resId, l, r) }
            | SDiv(resType, resId, l, r) -> { opCode = OpCode.SDiv; operands = RawOperands(resType, resId, l, r) }
            | FDiv(resType, resId, l, r) -> { opCode = OpCode.FDiv; operands = RawOperands(resType, resId, l, r) }
            | UMod(resType, resId, l, r) -> { opCode = OpCode.UMod; operands = RawOperands(resType, resId, l, r) }
            | SRem(resType, resId, l, r) -> { opCode = OpCode.SRem; operands = RawOperands(resType, resId, l, r) }
            | SMod(resType, resId, l, r) -> { opCode = OpCode.SMod; operands = RawOperands(resType, resId, l, r) }
            | FRem(resType, resId, l, r) -> { opCode = OpCode.FRem; operands = RawOperands(resType, resId, l, r) }
            | FMod(resType, resId, l, r) -> { opCode = OpCode.FMod; operands = RawOperands(resType, resId, l, r) }
            | VectorTimesScalar(resType, resId, v, s) -> { opCode = OpCode.VectorTimesScalar; operands = RawOperands(resType, resId, v, s) }
            | MatrixTimesScalar(resType, resId, m, s) -> { opCode = OpCode.MatrixTimesScalar; operands = RawOperands(resType, resId, m, s) }
            | VectorTimesMatrix(resType, resId, v, m) -> { opCode = OpCode.VectorTimesMatrix; operands = RawOperands(resType, resId, v, m) }
            | MatrixTimesVector(resType, resId, m, v) -> { opCode = OpCode.MatrixTimesVector; operands = RawOperands(resType, resId, m, v) }
            | MatrixTimesMatrix(resType, resId, m0, m1) -> { opCode = OpCode.MatrixTimesMatrix; operands = RawOperands(resType, resId, m0, m1) }
            | OuterProduct(resType, resId, v0, v1) -> { opCode = OpCode.OuterProduct; operands = RawOperands(resType, resId, v0, v1) }
            | Dot(resType, resId, v0, v1) -> { opCode = OpCode.Dot; operands = RawOperands(resType, resId, v0, v1) }
            | IAddCarry(resType, resId) -> { opCode = OpCode.IAddCarry; operands = RawOperands(resType, resId) }
            | ISubBorrow(resType, resId) -> { opCode = OpCode.ISubBorrow; operands = RawOperands(resType, resId) }
            | IMulExtended(resType, resId) -> { opCode = OpCode.IMulExtended; operands = RawOperands(resType, resId) }
            | ShiftRightLogical(resType, resId, l, r) -> { opCode = OpCode.ShiftRightLogical; operands = RawOperands(resType, resId, l, r) }
            | ShiftRightArithmetic(resType, resId, l, r) -> { opCode = OpCode.ShiftRightArithmetic; operands = RawOperands(resType, resId, l, r) }
            | ShiftLeftLogical(resType, resId, l, r) -> { opCode = OpCode.ShiftLeftLogical; operands = RawOperands(resType, resId, l, r) }
            | BitwiseOr(resType, resId, l, r) -> { opCode = OpCode.BitwiseOr; operands = RawOperands(resType, resId, l, r) }
            | BitwiseXor(resType, resId, l, r) -> { opCode = OpCode.BitwiseXor; operands = RawOperands(resType, resId, l, r) }
            | BitwiseAnd(resType, resId, l, r) -> { opCode = OpCode.BitwiseAnd; operands = RawOperands(resType, resId, l, r) }
            | Not(resType, resId, v) -> { opCode = OpCode.Not; operands = RawOperands(resType, resId, v) }
            | BitFieldInsert(resType, resId, _base, insert, offset, count) -> { opCode = OpCode.BitFieldInsert; operands = RawOperands(resType, resId, _base, insert, offset, count) }
            | BitFieldSExtract(resType, resId, _base, offset, count) -> { opCode = OpCode.BitFieldSExtract; operands = RawOperands(resType, resId, _base, offset, count) }
            | BitFieldUExtract(resType, resId, _base, offset, count) -> { opCode = OpCode.BitFieldUExtract; operands = RawOperands(resType, resId, _base, offset, count) }
            | BitReverse(resType, resId, _base) -> { opCode = OpCode.BitReverse; operands = RawOperands(resType, resId, _base) }
            | BitCount(resType, resId, _base) -> { opCode = OpCode.BitCount; operands = RawOperands(resType, resId, _base) }
            | Any(resType, resId, vec) -> { opCode = OpCode.Any; operands = RawOperands(resType, resId, vec) }
            | All(resType, resId, vec) -> { opCode = OpCode.All; operands = RawOperands(resType, resId, vec) }
            | IsNan(resType, resId, v) -> { opCode = OpCode.IsNan; operands = RawOperands(resType, resId, v) }
            | IsInf(resType, resId, v) -> { opCode = OpCode.IsInf; operands = RawOperands(resType, resId, v) }
            | IsFinite(resType, resId, v) -> { opCode = OpCode.IsFinite; operands = RawOperands(resType, resId, v) }
            | IsNormal(resType, resId, v) -> { opCode = OpCode.IsNormal; operands = RawOperands(resType, resId, v) }
            | SignBitSet(resType, resId, v) -> { opCode = OpCode.SignBitSet; operands = RawOperands(resType, resId, v) }
            | LessOrGreater(resType, resId, x, y) -> { opCode = OpCode.LessOrGreater; operands = RawOperands(resType, resId, x, y) }
            | Ordered(resType, resId, x, y) -> { opCode = OpCode.Ordered; operands = RawOperands(resType, resId, x, y) }
            | Unordered(resType, resId, x, y) -> { opCode = OpCode.Unordered; operands = RawOperands(resType, resId, x, y) }
            | LogicalEqual(resType, resId, x, y) -> { opCode = OpCode.LogicalEqual; operands = RawOperands(resType, resId, x, y) }
            | LogicalNotEqual(resType, resId, x, y) -> { opCode = OpCode.LogicalNotEqual; operands = RawOperands(resType, resId, x, y) }
            | LogicalOr(resType, resId, l, r) -> { opCode = OpCode.LogicalOr; operands = RawOperands(resType, resId, l, r) }
            | LogicalAnd(resType, resId, l, r) -> { opCode = OpCode.LogicalAnd; operands = RawOperands(resType, resId, l, r) }
            | LogicalNot(resType, resId, v) -> { opCode = OpCode.LogicalNot; operands = RawOperands(resType, resId, v) }
            | Select(resType, resId, guard, vtrue, vfalse) -> { opCode = OpCode.Select; operands = RawOperands(resType, resId, guard, vtrue, vfalse) }
            | IEqual(resType, resId, l, r) -> { opCode = OpCode.IEqual; operands = RawOperands(resType, resId, l, r) }
            | INotEqual(resType, resId, l, r) -> { opCode = OpCode.INotEqual; operands = RawOperands(resType, resId, l, r) }
            | UGreaterThan(resType, resId, l, r) -> { opCode = OpCode.UGreaterThan; operands = RawOperands(resType, resId, l, r) }
            | SGreaterThan(resType, resId, l, r) -> { opCode = OpCode.SGreaterThan; operands = RawOperands(resType, resId, l, r) }
            | UGreaterThanEqual(resType, resId, l, r) -> { opCode = OpCode.UGreaterThanEqual; operands = RawOperands(resType, resId, l, r) }
            | SGreaterThanEqual(resType, resId, l, r) -> { opCode = OpCode.SGreaterThanEqual; operands = RawOperands(resType, resId, l, r) }
            | ULessThan(resType, resId, l, r) -> { opCode = OpCode.ULessThan; operands = RawOperands(resType, resId, l, r) }
            | SLessThan(resType, resId, l, r) -> { opCode = OpCode.SLessThan; operands = RawOperands(resType, resId, l, r) }
            | ULessThanEqual(resType, resId, l, r) -> { opCode = OpCode.ULessThanEqual; operands = RawOperands(resType, resId, l, r) }
            | SLessThanEqual(resType, resId, l, r) -> { opCode = OpCode.SLessThanEqual; operands = RawOperands(resType, resId, l, r) }
            | FOrdEqual(resType, resId, l, r) -> { opCode = OpCode.FOrdEqual; operands = RawOperands(resType, resId, l, r) }
            | FUnordEqual(resType, resId, l, r) -> { opCode = OpCode.FUnordEqual; operands = RawOperands(resType, resId, l, r) }
            | FOrdNotEqual(resType, resId, l, r) -> { opCode = OpCode.FOrdNotEqual; operands = RawOperands(resType, resId, l, r) }
            | FUnordNotEqual(resType, resId, l, r) -> { opCode = OpCode.FUnordNotEqual; operands = RawOperands(resType, resId, l, r) }
            | FOrdLessThan(resType, resId, l, r) -> { opCode = OpCode.FOrdLessThan; operands = RawOperands(resType, resId, l, r) }
            | FUnordLessThan(resType, resId, l, r) -> { opCode = OpCode.FUnordLessThan; operands = RawOperands(resType, resId, l, r) }
            | FOrdGreaterThan(resType, resId, l, r) -> { opCode = OpCode.FOrdGreaterThan; operands = RawOperands(resType, resId, l, r) }
            | FUnordGreaterThan(resType, resId, l, r) -> { opCode = OpCode.FUnordGreaterThan; operands = RawOperands(resType, resId, l, r) }
            | FOrdLessThanEqual(resType, resId, l, r) -> { opCode = OpCode.FOrdLessThanEqual; operands = RawOperands(resType, resId, l, r) }
            | FUnordLessThanEqual(resType, resId, l, r) -> { opCode = OpCode.FUnordLessThanEqual; operands = RawOperands(resType, resId, l, r) }
            | FOrdGreaterThanEqual(resType, resId, l, r) -> { opCode = OpCode.FOrdGreaterThanEqual; operands = RawOperands(resType, resId, l, r) }
            | FUnordGreaterThanEqual(resType, resId, l, r) -> { opCode = OpCode.FUnordGreaterThanEqual; operands = RawOperands(resType, resId, l, r) }
            | DPdx(resType, resId, p) -> { opCode = OpCode.DPdx; operands = RawOperands(resType, resId, p) }
            | DPdy(resType, resId, p) -> { opCode = OpCode.DPdy; operands = RawOperands(resType, resId, p) }
            | Fwidth(resType, resId, p) -> { opCode = OpCode.Fwidth; operands = RawOperands(resType, resId, p) }
            | DPdxFine(resType, resId, p) -> { opCode = OpCode.DPdxFine; operands = RawOperands(resType, resId, p) }
            | DPdyFine(resType, resId, p) -> { opCode = OpCode.DPdyFine; operands = RawOperands(resType, resId, p) }
            | FwidthFine(resType, resId, p) -> { opCode = OpCode.FwidthFine; operands = RawOperands(resType, resId, p) }
            | DPdxCoarse(resType, resId, p) -> { opCode = OpCode.DPdxCoarse; operands = RawOperands(resType, resId, p) }
            | DPdyCoarse(resType, resId, p) -> { opCode = OpCode.DPdyCoarse; operands = RawOperands(resType, resId, p) }
            | FwidthCoarse(resType, resId, p) -> { opCode = OpCode.FwidthCoarse; operands = RawOperands(resType, resId, p) }
            | Phi(resType, resId, variableParentPairs) -> { opCode = OpCode.Phi; operands = RawOperands(resType, resId, variableParentPairs) }
            | LoopMerge(mergeBlock, ctrl) -> { opCode = OpCode.LoopMerge; operands = RawOperands(mergeBlock, ctrl) }
            | SelectionMerge(mergeBlock, ctrl) -> { opCode = OpCode.SelectionMerge; operands = RawOperands(mergeBlock, ctrl) }
            | Label(resId) -> { opCode = OpCode.Label; operands = RawOperands(resId) }
            | Branch(target) -> { opCode = OpCode.Branch; operands = RawOperands(target) }
            | BranchConditional(condition, trueLabel, falseLabel, branchWeights) -> { opCode = OpCode.BranchConditional; operands = RawOperands(condition, trueLabel, falseLabel, branchWeights) }
            | Switch(selector, _default, literalLabelPairs) -> { opCode = OpCode.Switch; operands = RawOperands(selector, _default, literalLabelPairs) }
            | Kill -> { opCode = OpCode.Kill; operands = RawOperands() }
            | Return -> { opCode = OpCode.Return; operands = RawOperands() }
            | ReturnValue(value) -> { opCode = OpCode.ReturnValue; operands = RawOperands(value) }
            | Unreachable -> { opCode = OpCode.Unreachable; operands = RawOperands() }
            | LifetimeStart(ptr, size) -> { opCode = OpCode.LifetimeStart; operands = RawOperands(ptr, size) }
            | LifetimeStop(ptr, size) -> { opCode = OpCode.LifetimeStop; operands = RawOperands(ptr, size) }
            | AtomicLoad(resType, resId, ptr, scope, sem) -> { opCode = OpCode.AtomicLoad; operands = RawOperands(resType, resId, ptr, scope, sem) }
            | AtomicStore(resType, resId, ptr, scope, value) -> { opCode = OpCode.AtomicStore; operands = RawOperands(resType, resId, ptr, scope, value) }
            | AtomicExchange(resType, resId, ptr, scope, sem, value) -> { opCode = OpCode.AtomicExchange; operands = RawOperands(resType, resId, ptr, scope, sem, value) }
            | AtomicCompareExchange(resType, resId, ptr, scope, _equal, _unequal, value, cmp) -> { opCode = OpCode.AtomicCompareExchange; operands = RawOperands(resType, resId, ptr, scope, _equal, _unequal, value, cmp) }
            | AtomicCompareExchangeWeak(resType, resId, ptr, scope, _equal, _unequal, value, cmp) -> { opCode = OpCode.AtomicCompareExchangeWeak; operands = RawOperands(resType, resId, ptr, scope, _equal, _unequal, value, cmp) }
            | AtomicIIncrement(resType, resId, ptr, scope, sem) -> { opCode = OpCode.AtomicIIncrement; operands = RawOperands(resType, resId, ptr, scope, sem) }
            | AtomicIDecrement(resType, resId, ptr, scope, sem) -> { opCode = OpCode.AtomicIDecrement; operands = RawOperands(resType, resId, ptr, scope, sem) }
            | AtomicIAdd(resType, resId, ptr, scope, sem, value) -> { opCode = OpCode.AtomicIAdd; operands = RawOperands(resType, resId, ptr, scope, sem, value) }
            | AtomicISub(resType, resId, ptr, scope, sem, value) -> { opCode = OpCode.AtomicISub; operands = RawOperands(resType, resId, ptr, scope, sem, value) }
            | AtomicSMin(resType, resId, ptr, scope, sem, value) -> { opCode = OpCode.AtomicSMin; operands = RawOperands(resType, resId, ptr, scope, sem, value) }
            | AtomicUMin(resType, resId, ptr, scope, sem, value) -> { opCode = OpCode.AtomicUMin; operands = RawOperands(resType, resId, ptr, scope, sem, value) }
            | AtomicSMax(resType, resId, ptr, scope, sem, value) -> { opCode = OpCode.AtomicSMax; operands = RawOperands(resType, resId, ptr, scope, sem, value) }
            | AtomicUMax(resType, resId, ptr, scope, sem, value) -> { opCode = OpCode.AtomicUMax; operands = RawOperands(resType, resId, ptr, scope, sem, value) }
            | AtomicAnd(resType, resId, ptr, scope, sem, value) -> { opCode = OpCode.AtomicAnd; operands = RawOperands(resType, resId, ptr, scope, sem, value) }
            | AtomicOr(resType, resId, ptr, scope, sem, value) -> { opCode = OpCode.AtomicOr; operands = RawOperands(resType, resId, ptr, scope, sem, value) }
            | AtomicXor(resType, resId, ptr, scope, sem, value) -> { opCode = OpCode.AtomicXor; operands = RawOperands(resType, resId, ptr, scope, sem, value) }
            | EmitVertex -> { opCode = OpCode.EmitVertex; operands = RawOperands() }
            | EndPrimitive -> { opCode = OpCode.EndPrimitive; operands = RawOperands() }
            | EmitStreamVertex(stream) -> { opCode = OpCode.EmitStreamVertex; operands = RawOperands(stream) }
            | EndStreamPrimitive(stream) -> { opCode = OpCode.EndStreamPrimitive; operands = RawOperands(stream) }
            | ControlBarrier(exec, mem, sem) -> { opCode = OpCode.ControlBarrier; operands = RawOperands(exec, mem, sem) }
            | MemoryBarrier(mem, sem) -> { opCode = OpCode.MemoryBarrier; operands = RawOperands(mem, sem) }

    let writeStream (o : Stream) (instructions : list<Instruction>) = 
        let raw = instructions |> List.map toRawInstruction
        let maxId = instructions |> List.choose tryGetId |> List.max
        RawWriter.write o raw maxId


