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

    let writeStream (o : Stream) (instructions : list<Instruction>) = 
        let raw = instructions |> List.map toRawInstruction
        RawWriter.write o raw


