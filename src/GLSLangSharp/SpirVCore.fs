namespace GLSLang.SpirV


open System
open System.IO


type OpCode =
    | Nop = 0us
    | Undef = 1us
    | Source = 3us
    | SourceExtension = 4us
    | Name = 5us
    | MemberName = 6us
    | String = 7us
    | Line = 8us
    | Extension = 10us
    | ExtInstImport = 11us
    | ExtInst = 12us
    | MemoryModel = 14us
    | EntryPoint = 15us
    | ExecutionMode = 16us
    | Capability = 17us
    | TypeVoid = 19us
    | TypeBool = 20us
    | TypeInt = 21us
    | TypeFloat = 22us
    | TypeVector = 23us
    | TypeMatrix = 24us
    | TypeImage = 25us
    | TypeSampler = 26us
    | TypeSampledImage = 27us
    | TypeArray = 28us
    | TypeRuntimeArray = 29us
    | TypeStruct = 30us
    | TypeOpaque = 31us
    | TypePointer = 32us
    | TypeFunction = 33us
    | TypeEvent = 34us
    | TypeDeviceEvent = 35us
    | TypeReserveId = 36us
    | TypeQueue = 37us
    | TypePipe = 38us
    | ConstantTrue = 41us
    | ConstantFalse = 42us
    | Constant = 43us
    | ConstantComposite = 44us
    | ConstantSampler = 45us
    | ConstantNull = 46us
    | SpecConstantTrue = 48us
    | SpecConstantFalse = 49us
    | SpecConstant = 50us
    | SpecConstantComposite = 51us
    | SpecConstantOp = 52us
    | Function = 54us
    | FunctionParameter = 55us
    | FunctionEnd = 56us
    | FunctionCall = 57us
    | Variable = 59us
    | ImageTexelPointer = 60us
    | Load = 61us
    | Store = 62us
    | CopyMemory = 63us
    | CopyMemorySized = 64us
    | AccessChain = 65us
    | InBoundsAccessChain = 66us
    | PtrAccessChain = 67us
    | ArrayLength = 68us
    | GenericPtrMemSemantics = 69us
    | Decorate = 71us
    | MemberDecorate = 72us
    | DecorationGroup = 73us
    | GroupDecorate = 74us
    | GroupMemberDecorate = 75us
    | VectorExtractDynamic = 77us
    | VectorInsertDynamic = 78us
    | VectorShuffle = 79us
    | CompositeConstruct = 80us
    | CompositeExtract = 81us
    | CompositeInsert = 82us
    | CopyObject = 83us
    | Transpose = 84us
    | SampledImage = 86us
    | ImageSampleImplicitLod = 87us
    | ImageSampleExplicitLod = 88us
    | ImageSampleDrefImplicitLod = 89us
    | ImageSampleDrefExplicitLod = 90us
    | ImageSampleProjImplicitLod = 91us
    | ImageSampleProjExplicitLod = 92us
    | ImageSampleProjDrefImplicitLod = 93us
    | ImageSampleProjDrefExplicitLod = 94us
    | ImageFetch = 95us
    | ImageGather = 96us
    | ImageDrefGather = 97us
    | ImageRead = 98us
    | ImageWrite = 99us
    | ImageQueryDim = 100us
    | ImageQueryFormat = 101us
    | ImageQueryOrder = 102us
    | ImageQuerySizeLod = 103us
    | ImageQuerySize = 104us
    | ImageQueryLod = 105us
    | ImageQueryLevels = 106us
    | ImageQuerySamples = 107us
    | ConvertFToU = 109us
    | ConvertFToS = 110us
    | ConvertSToF = 111us
    | ConvertUToF = 112us
    | UConvert = 113us
    | SConvert = 114us
    | FConvert = 115us
    | QuantizeToF16 = 116us
    | ConvertPtrToU = 117us
    | SatConvertSToU = 118us
    | SatConvertUToS = 119us
    | ConvertUToPtr = 120us
    | PtrCastToGeneric = 121us
    | GenericCastToPtr = 122us
    | GenericCastToPtrExplicit = 123us
    | Bitcast = 124us
    | SNegate = 126us
    | FNegate = 127us
    | IAdd = 128us
    | FAdd = 129us
    | ISub = 130us
    | FSub = 131us
    | IMul = 132us
    | FMul = 133us
    | UDiv = 134us
    | SDiv = 135us
    | FDiv = 136us
    | UMod = 137us
    | SRem = 138us
    | SMod = 139us
    | FRem = 140us
    | FMod = 141us
    | VectorTimesScalar = 142us
    | MatrixTimesScalar = 143us
    | VectorTimesMatrix = 144us
    | MatrixTimesVector = 145us
    | MatrixTimesMatrix = 146us
    | OuterProduct = 147us
    | Dot = 148us
    | IAddCarry = 149us
    | ISubBorrow = 150us
    | IMulExtended = 151us
    | Any = 154us
    | All = 155us
    | IsNan = 156us
    | IsInf = 157us
    | IsFinite = 158us
    | IsNormal = 159us
    | SignBitSet = 160us
    | LessOrGreater = 161us
    | Ordered = 162us
    | Unordered = 163us
    | LogicalEqual = 164us
    | LogicalNotEqual = 165us
    | LogicalOr = 166us
    | LogicalAnd = 167us
    | LogicalNot = 168us
    | Select = 169us
    | IEqual = 170us
    | INotEqual = 171us
    | UGreaterThan = 172us
    | SGreaterThan = 173us
    | UGreaterThanEqual = 174us
    | SGreaterThanEqual = 175us
    | ULessThan = 176us
    | SLessThan = 177us
    | ULessThanEqual = 178us
    | SLessThanEqual = 179us
    | FOrdEqual = 180us
    | FUnordEqual = 181us
    | FOrdNotEqual = 182us
    | FUnordNotEqual = 183us
    | FOrdLessThan = 184us
    | FUnordLessThan = 185us
    | FOrdGreaterThan = 186us
    | FUnordGreaterThan = 187us
    | FOrdLessThanEqual = 188us
    | FUnordLessThanEqual = 189us
    | FOrdGreaterThanEqual = 190us
    | FUnordGreaterThanEqual = 191us
    | ShiftRightLogical = 194us
    | ShiftRightArithmetic = 195us
    | ShiftLeftLogical = 196us
    | BitwiseOr = 197us
    | BitwiseXor = 198us
    | BitwiseAnd = 199us
    | Not = 200us
    | BitFieldInsert = 201us
    | BitFieldSExtract = 202us
    | BitFieldUExtract = 203us
    | BitReverse = 204us
    | BitCount = 205us
    | DPdx = 207us
    | DPdy = 208us
    | Fwidth = 209us
    | DPdxFine = 210us
    | DPdyFine = 211us
    | FwidthFine = 212us
    | DPdxCoarse = 213us
    | DPdyCoarse = 214us
    | FwidthCoarse = 215us
    | EmitVertex = 218us
    | EndPrimitive = 219us
    | EmitStreamVertex = 220us
    | EndStreamPrimitive = 221us
    | ControlBarrier = 224us
    | MemoryBarrier = 225us
    | AtomicLoad = 227us
    | AtomicStore = 228us
    | AtomicExchange = 229us
    | AtomicCompareExchange = 230us
    | AtomicCompareExchangeWeak = 231us
    | AtomicIIncrement = 232us
    | AtomicIDecrement = 233us
    | AtomicIAdd = 234us
    | AtomicISub = 235us
    | AtomicSMin = 236us
    | AtomicUMin = 237us
    | AtomicSMax = 238us
    | AtomicUMax = 239us
    | AtomicAnd = 240us
    | AtomicOr = 241us
    | AtomicXor = 242us
    | Phi = 245us
    | LoopMerge = 246us
    | SelectionMerge = 247us
    | Label = 248us
    | Branch = 249us
    | BranchConditional = 250us
    | Switch = 251us
    | Kill = 252us
    | Return = 253us
    | ReturnValue = 254us
    | Unreachable = 255us
    | LifetimeStart = 256us
    | LifetimeStop = 257us
    | AsyncGroupCopy = 259us
    | WaitGroupEvents = 260us
    | GroupAll = 261us
    | GroupAny = 262us
    | GroupBroadcast = 263us
    | GroupIAdd = 264us
    | GroupFAdd = 265us
    | GroupFMin = 266us
    | GroupUMin = 267us
    | GroupSMin = 268us
    | GroupFMax = 269us
    | GroupUMax = 270us
    | GroupSMax = 271us
    | ReadPipe = 274us
    | WritePipe = 275us
    | ReservedReadPipe = 276us
    | ReservedWritePipe = 277us
    | ReserveReadPipePackets = 278us
    | ReserveWritePipePackets = 279us
    | CommitReadPipe = 280us
    | CommitWritePipe = 281us
    | IsValidReserveId = 282us
    | GetNumPipePackets = 283us
    | GetMaxPipePackets = 284us
    | GroupReserveReadPipePackets = 285us
    | GroupReserveWritePipePackets = 286us
    | GroupCommitReadPipe = 287us
    | GroupCommitWritePipe = 288us
    | EnqueueMarker = 291us
    | EnqueueKernel = 292us
    | GetKernelNDrangeSubGroupCount = 293us
    | GetKernelNDrangeMaxSubGroupSize = 294us
    | GetKernelWorkGroupSize = 295us
    | GetKernelPreferredWorkGroupSizeMultiple = 296us
    | RetainEvent = 297us
    | ReleaseEvent = 298us
    | CreateUserEvent = 299us
    | IsValidEvent = 300us
    | SetUserEventStatus = 301us
    | CaptureEventProfilingInfo = 302us
    | GetDefaultQueue = 303us
    | BuildNDRange = 304us

type SourceLanguage =
    | Unknown = 0u
    | ESSL = 1u
    | GLSL = 2u
    | OpenCL = 3u

type Decoration =
    | RelaxedPrecision = 0u
    | SpecId = 1u
    | Block = 2u
    | BufferBlock = 3u
    | RowMajor = 4u
    | ColMajor = 5u
    | ArrayStride = 6u
    | MatrixStride = 7u
    | GLSLShared = 8u
    | GLSLPacked = 9u
    | CPacked = 10u
    | BuiltIn = 11u
    | Smooth = 12u
    | Noperspective = 13u
    | Flat = 14u
    | Patch = 15u
    | Centroid = 16u
    | Sample = 17u
    | Invariant = 18u
    | Restrict = 19u
    | Aliased = 20u
    | Volatile = 21u
    | Constant = 22u
    | Coherent = 23u
    | Nonwritable = 24u
    | Nonreadable = 25u
    | Uniform = 26u
    | NoStaticUse = 27u
    | SaturatedConversion = 28u
    | Stream = 29u
    | Location = 30u
    | Component = 31u
    | Index = 32u
    | Binding = 33u
    | DescriptorSet = 34u
    | Offset = 35u
    | XfbBuffer = 36u
    | XfbStride = 37u
    | FuncParamAttr = 38u
    | FPRoundingMode = 39u
    | FPFastMathMode = 40u
    | LinkageAttributes = 41u

type AddressingModel =
    | Logical = 0u
    | Physical32 = 1u
    | Physical64 = 2u

type MemoryModel =
    | Simple = 0u
    | GLSL450 = 1u
    | OpenCL = 2u

type ExecutionModel =
    | Vertex = 0u
    | TessellationControl = 1u
    | TessellationEvaluation = 2u
    | Geometry = 3u
    | Fragment = 4u
    | GLCompute = 5u
    | Kernel = 6u

type ExecutionMode =
    | Invocations = 0u
    | SpacingEqual = 1u
    | SpacingFractionalEven = 2u
    | SpacingFractionalOdd = 3u
    | VertexOrderCw = 4u
    | VertexOrderCcw = 5u
    | PixelCenterInteger = 6u
    | OriginUpperLeft = 7u
    | OriginLowerLeft = 8u
    | EarlyFragmentTests = 9u
    | PointMode = 10u
    | Xfb = 11u
    | DepthReplacing = 12u
    | DepthAny = 13u
    | DepthGreater = 14u
    | DepthLess = 15u
    | DepthUnchanged = 16u
    | LocalSize = 17u
    | LocalSizeHint = 18u
    | InputPoints = 19u
    | InputLines = 20u
    | InputLinesAdjacency = 21u
    | InputTriangles = 22u
    | InputTrianglesAdjacency = 23u
    | InputQuads = 24u
    | InputIsolines = 25u
    | OutputVertices = 26u
    | OutputPoints = 27u
    | OutputLineStrip = 28u
    | OutputTriangleStrip = 29u
    | VecTypeHint = 30u
    | ContractionOff = 31u

type Capability =
    | Matrix = 0u
    | Shader = 1u
    | Geometry = 2u
    | Tessellation = 3u
    | Addresses = 4u
    | Linkage = 5u
    | Kernel = 6u
    | Vector16 = 7u
    | Float16Buffer = 8u
    | Float16 = 9u
    | Float64 = 10u
    | Int64 = 11u
    | Int64Atomics = 12u
    | ImageBasic = 13u
    | ImageReadWrite = 14u
    | ImageMipmap = 15u
    | ImageSRGBWrite = 16u
    | Pipes = 17u
    | Groups = 18u
    | DeviceEnqueue = 19u
    | LiteralSampler = 20u
    | AtomicStorage = 21u
    | Int16 = 22u
    | TessellationPointSize = 23u
    | GeometryPointSize = 24u
    | ImageGatherExtended = 25u
    | StorageImageExtendedFormats = 26u
    | StorageImageMultisample = 27u
    | UniformBufferArrayDynamicIndexing = 28u
    | SampledImageArrayDynamicIndexing = 29u
    | StorageBufferArrayDynamicIndexing = 30u
    | StorageImageArrayDynamicIndexing = 31u
    | ClipDistance = 32u
    | CullDistance = 33u
    | ImageCubeArray = 34u
    | SampleRateShading = 35u

type SampleMode =
    | Runtime = 0u
    | Sampler = 1u
    | WithoutSampler = 2u

type ImageFormat =
    | Unknown = 0u
    | Rgba32f = 1u
    | Rgba16f = 2u
    | R32f = 3u
    | Rgba8 = 4u
    | Rgba8Snorm = 5u
    | Rg32f = 6u
    | Rg16f = 7u
    | R11fG11fB10f = 8u
    | R16f = 9u
    | Rgba16 = 10u
    | Rgb10A2 = 11u
    | Rg16 = 12u
    | Rg8 = 13u
    | R16 = 14u
    | R8 = 15u
    | Rgba16Snorm = 16u
    | Rg16Snorm = 17u
    | Rg8Snorm = 18u
    | R16Snorm = 19u
    | R8Snorm = 20u
    | Rgba32i = 21u
    | Rgba16i = 22u
    | Rgba8i = 23u
    | R32i = 24u
    | Rg32i = 25u
    | Rg16i = 26u
    | Rg8i = 27u
    | R16i = 28u
    | R8i = 29u
    | Rgba32ui = 30u
    | Rgba16ui = 31u
    | Rgba8ui = 32u
    | R32ui = 33u
    | Rgb10a2ui = 34u
    | Rg32ui = 35u
    | Rg16ui = 36u
    | Rg8ui = 37u
    | R16ui = 38u
    | R8ui = 39u

type StorageClass =
    | UniformConstant = 0u
    | Input = 1u
    | Uniform = 2u
    | Output = 3u
    | WorkgroupLocal = 4u
    | WorkgroupGlobal = 5u
    | PrivateGlobal = 6u
    | Function = 7u
    | Generic = 8u
    | AtomicCounter = 10u
    | Image = 11u

type AccessQualifier =
    | ReadOnly = 0u
    | WriteOnly = 1u
    | ReadWrite = 2u

type SamplerAddressingMode =
    | None = 0u
    | ClampToEdge = 1u
    | Clamp = 2u
    | Repeat = 3u
    | RepeatMirrored = 4u

type SamplerFilterMode =
    | Nearest = 0u
    | Linear = 1u

type Dim =
    | Dim1D = 0u
    | Dim2D = 1u
    | Dim3D = 2u
    | DimCube = 3u
    | DimRect = 4u
    | DimBuffer = 5u

type FunctionControl =
    | MaskNone = 0u
    | InlineMask = 0x00000001u
    | DontInlineMask = 0x00000002u
    | PureMask = 0x00000004u
    | ConstMask = 0x00000008u

type LoopControl =
    | UnrollShift = 0x00000001u
    | DontUnrollShift = 0x00000002u

type SelectionControl =
    | Flatten = 0x00000001u
    | DontFlattenShift = 0x00000002u

type Scope =
    | CrossDevice = 0u
    | Device = 1u
    | Workgroup = 2u
    | Subgroup = 3u
    | Invocation = 4u

type MemorySemantics =
    | MaskNone = 0u
    | RelaxedMask = 0x00000001u
    | SequentiallyConsistentMask = 0x00000002u
    | AcquireMask = 0x00000004u
    | ReleaseMask = 0x00000008u
    | UniformMemoryMask = 0x00000010u
    | SubgroupMemoryMask = 0x00000020u
    | WorkgroupLocalMemoryMask = 0x00000040u
    | WorkgroupGlobalMemoryMask = 0x00000080u
    | AtomicCounterMemoryMask = 0x00000100u
    | ImageMemoryMask = 0x00000200u

type RawOperands (data : byte[]) =
    member x.AsString = 
        let mutable last = data.Length - 1
        while last > 0 && data.[last] = 0uy do
            last <- last - 1

        System.Text.UTF8Encoding.UTF8.GetString(data, 0, last + 1)

    member x.String 
        with get (offset : int) =
            let mutable last = data.Length - 1
            while last > offset && data.[last] = 0uy do
                last <- last - 1

            System.Text.UTF8Encoding.UTF8.GetString(data, offset, last + 1 - offset)

    member x.UInt32
        with get (i : int) = BitConverter.ToUInt32(data, i * 4)

    member x.UInt32Array 
        with get (start : int) =
            let len = data.Length / 4 - start

            Array.init len (fun i -> x.UInt32 (start + i))


    member x.Int32
        with get (i : int) = BitConverter.ToInt32(data, i * 4)

    member x.Int32Array 
        with get (start : int) =
            let len = data.Length / 4 - start
            Array.init len (fun i -> x.Int32 (start + i))

    member x.Bytes = data
    member x.Length = data.Length
        
    new ([<ParamArray>] values : obj[]) =
        let bytes = 
            values |> Array.collect (fun v -> 
                let t = v.GetType()

                if t.IsEnum then BitConverter.GetBytes(unbox<uint32> v)
                elif t = typeof<uint32> then BitConverter.GetBytes(unbox<uint32> v)
                elif t.IsArray && t.GetElementType() = typeof<uint32> then 
                    let arr = v |> unbox<uint32[]>
                    arr |> Array.collect BitConverter.GetBytes
                elif t = typeof<bool> then BitConverter.GetBytes (if unbox v then 1u else 0u)
                elif t = typeof<string> then 
                    let arr = System.Text.ASCIIEncoding.ASCII.GetBytes (unbox<string> v)
                    let arr = Array.append arr [|0uy|]

                    if arr.Length % 4 = 0 then
                        arr
                    else 
                        let missing = 4 - (arr.Length % 4)
                        Array.append arr (Array.zeroCreate missing)

                else failwith "unknown argument type"

            )
        RawOperands(bytes)


type RawInstruction = { opCode : OpCode; operands : RawOperands }

type RawModule = { magic : uint32; version : uint32; generator : uint32; bound : uint32; reserved : uint32; instructions : list<RawInstruction> }





[<AutoOpen>]
module private DefinesAndExtensions =
    let SV_MAGIC = 0x07230203u
    let SV_MAGIC_REV = 0x03022307u

    type Stream with
        member x.ReadUInt16() =
            let data = Array.zeroCreate sizeof<uint16>
            x.Read(data, 0, sizeof<uint16>) |> ignore
            BitConverter.ToUInt16(data, 0)

        member x.ReadUInt32() =
            let data = Array.zeroCreate sizeof<uint32>
            x.Read(data, 0, sizeof<uint32>) |> ignore
            BitConverter.ToUInt32(data, 0)

        member x.Remaining =
            x.Length - x.Position

module private RawReader =

    let readInstruction(data : Stream) =
        if data.Remaining < 1L then
            failwithf "[SpirV] not enough data"
        else
            let num = data.Remaining

            let read = data.ReadUInt32()
            let wordCount = read >>> 16 |> uint16
            let opCode = read &&& 0xFFFFu |> uint16 |> unbox<OpCode>

            if wordCount < 1us then
                failwithf "[SpirV] malformed instruction: %A" wordCount
            elif int64 wordCount > num then
                failwithf "[SpirV] not enough data: %A > %A" wordCount num
            else
                let args = Array.zeroCreate ( 4 * (int wordCount - 1) )
                let mutable read = 0
                while read < args.Length && data.Length - data.Position > 0L do
                    read <- read + data.Read(args, read, args.Length - read)

                { opCode = opCode; operands = RawOperands(args) }

    let read(data : Stream) =
        if data.Length < 5L then
            failwith "[SpirV] not enough data"

        let magic = data.ReadUInt32()
        let version = data.ReadUInt32()
        let generator = data.ReadUInt32()
        let bound = data.ReadUInt32()
        let reserved = data.ReadUInt32()


        if magic <> SV_MAGIC || version <> 99u then
            failwith "[SpirV] invalid SpirV magic or version"

        let instructions =
            [
                while data.Position < data.Length do
                    yield readInstruction data
            ]

        { magic = magic; version = version; generator = generator; bound = bound; reserved = reserved; instructions = instructions }



module private RawWriter =
    
    type Stream with
        member x.WriteUInt32(u : uint32) =
            x.Write(BitConverter.GetBytes(u), 0, sizeof<uint32>)
            

    let write (s : Stream) (instructions : list<RawInstruction>) (maxId : uint32) =
        s.WriteUInt32(SV_MAGIC)
        s.WriteUInt32(99u)
        s.WriteUInt32(0x051A00BBu)
        s.WriteUInt32(maxId + 1u)
        s.WriteUInt32(0u)


        for i in instructions do
            let wordCount = uint16(i.operands.Length / 4) + 1us
            let opCode = i.opCode |> uint16

            let write = ((uint32 wordCount) <<< 16) ||| (uint32 opCode)
            s.WriteUInt32(write)
            s.Write(i.operands.Bytes, 0, i.operands.Length)

        ()