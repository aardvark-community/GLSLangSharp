namespace GLSLang.Raw

open Aardvark.Base
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#nowarn "9"
#nowarn "51"

type ShLanguage =
    | Vertex = 0
    | TessControl = 1
    | TessEvaluation = 2
    | Geometry = 3
    | Fragment = 4
    | Compute = 5

type ShOptimizationLevel =
    | NoGeneration = 0
    | None = 1
    | Simple = 2
    | Full = 3

type ShMessages =
    | Default          = 0x0000000  // default is to give all required errors and extra warnings
    | RelaxedErrors    = 0x0000001  // be liberal in accepting input
    | SuppressWarnings = 0x0000002  // suppress all warnings, except those required by the specification
    | AST              = 0x0000004  // print the AST intermediate representation
    | SpvRules         = 0x0000008  // issue messages for SPIR-V generation
    | VulkanRules      = 0x0000010  // issue messages for Vulkan-requirements of GLSL for SPIR-V
    | OnlyPreprocessor = 0x0000020  // only print out errors produced by the preprocessor

[<StructLayout(LayoutKind.Sequential)>]
type TLimits =
    struct
        val mutable public nonInductiveForLoops : int
        val mutable public whileLoops : int
        val mutable public doWhileLoops : int
        val mutable public generalUniformIndexing : int
        val mutable public generalAttributeMatrixVectorIndexing : int
        val mutable public generalVaryingIndexing : int
        val mutable public generalSamplerIndexing : int
        val mutable public generalVariableIndexing : int
        val mutable public generalConstantMatrixVectorIndexing : int

        static member Default =
            TLimits(
                nonInductiveForLoops = 1,
                whileLoops = 1,
                doWhileLoops = 1,
                generalUniformIndexing = 1,
                generalAttributeMatrixVectorIndexing = 1,
                generalVaryingIndexing = 1,
                generalSamplerIndexing = 1,
                generalVariableIndexing = 1,
                generalConstantMatrixVectorIndexing = 1
            )

    end

[<StructLayout(LayoutKind.Sequential)>]
type TBuiltInResource =
    struct
        val mutable public maxLights : int
        val mutable public maxClipPlanes : int
        val mutable public maxTextureUnits : int
        val mutable public maxTextureCoords : int
        val mutable public maxVertexAttribs : int
        val mutable public maxVertexUniformComponents : int
        val mutable public maxVaryingFloats : int
        val mutable public maxVertexTextureImageUnits : int
        val mutable public maxCombinedTextureImageUnits : int
        val mutable public maxTextureImageUnits : int
        val mutable public maxFragmentUniformComponents : int
        val mutable public maxDrawBuffers : int
        val mutable public maxVertexUniformVectors : int
        val mutable public maxVaryingVectors : int
        val mutable public maxFragmentUniformVectors : int
        val mutable public maxVertexOutputVectors : int
        val mutable public maxFragmentInputVectors : int
        val mutable public minProgramTexelOffset : int
        val mutable public maxProgramTexelOffset : int
        val mutable public maxClipDistances : int
        val mutable public maxComputeWorkGroupCountX : int
        val mutable public maxComputeWorkGroupCountY : int
        val mutable public maxComputeWorkGroupCountZ : int
        val mutable public maxComputeWorkGroupSizeX : int
        val mutable public maxComputeWorkGroupSizeY : int
        val mutable public maxComputeWorkGroupSizeZ : int
        val mutable public maxComputeUniformComponents : int
        val mutable public maxComputeTextureImageUnits : int
        val mutable public maxComputeImageUniforms : int
        val mutable public maxComputeAtomicCounters : int
        val mutable public maxComputeAtomicCounterBuffers : int
        val mutable public maxVaryingComponents : int 
        val mutable public maxVertexOutputComponents : int
        val mutable public maxGeometryInputComponents : int
        val mutable public maxGeometryOutputComponents : int
        val mutable public maxFragmentInputComponents : int
        val mutable public maxImageUnits : int
        val mutable public maxCombinedImageUnitsAndFragmentOutputs : int
        val mutable public maxCombinedShaderOutputResources : int
        val mutable public maxImageSamples : int
        val mutable public maxVertexImageUniforms : int
        val mutable public maxTessControlImageUniforms : int
        val mutable public maxTessEvaluationImageUniforms : int
        val mutable public maxGeometryImageUniforms : int
        val mutable public maxFragmentImageUniforms : int
        val mutable public maxCombinedImageUniforms : int
        val mutable public maxGeometryTextureImageUnits : int
        val mutable public maxGeometryOutputVertices : int
        val mutable public maxGeometryTotalOutputComponents : int
        val mutable public maxGeometryUniformComponents : int
        val mutable public maxGeometryVaryingComponents : int
        val mutable public maxTessControlInputComponents : int
        val mutable public maxTessControlOutputComponents : int
        val mutable public maxTessControlTextureImageUnits : int
        val mutable public maxTessControlUniformComponents : int
        val mutable public maxTessControlTotalOutputComponents : int
        val mutable public maxTessEvaluationInputComponents : int
        val mutable public maxTessEvaluationOutputComponents : int
        val mutable public maxTessEvaluationTextureImageUnits : int
        val mutable public maxTessEvaluationUniformComponents : int
        val mutable public maxTessPatchComponents : int
        val mutable public maxPatchVertices : int
        val mutable public maxTessGenLevel : int
        val mutable public maxViewports : int
        val mutable public maxVertexAtomicCounters : int
        val mutable public maxTessControlAtomicCounters : int
        val mutable public maxTessEvaluationAtomicCounters : int
        val mutable public maxGeometryAtomicCounters : int
        val mutable public maxFragmentAtomicCounters : int
        val mutable public maxCombinedAtomicCounters : int
        val mutable public maxAtomicCounterBindings : int
        val mutable public maxVertexAtomicCounterBuffers : int
        val mutable public maxTessControlAtomicCounterBuffers : int
        val mutable public maxTessEvaluationAtomicCounterBuffers : int
        val mutable public maxGeometryAtomicCounterBuffers : int
        val mutable public maxFragmentAtomicCounterBuffers : int
        val mutable public maxCombinedAtomicCounterBuffers : int
        val mutable public maxAtomicCounterBufferSize : int
        val mutable public maxTransformFeedbackBuffers : int
        val mutable public maxTransformFeedbackInterleavedComponents : int
        val mutable public maxCullDistances : int
        val mutable public maxCombinedClipAndCullDistances : int
        val mutable public maxSamples : int

        val mutable public limits : TLimits

        static member Default =
            TBuiltInResource(
                maxLights = 32,
                maxClipPlanes = 6,
                maxTextureUnits = 32,
                maxTextureCoords = 32,
                maxVertexAttribs = 64,
                maxVertexUniformComponents = 4096,
                maxVaryingFloats = 64,
                maxVertexTextureImageUnits = 32,
                maxCombinedTextureImageUnits = 80,
                maxTextureImageUnits = 32,
                maxFragmentUniformComponents = 4096,
                maxDrawBuffers = 32,
                maxVertexUniformVectors = 128,
                maxVaryingVectors = 8,
                maxFragmentUniformVectors = 16,
                maxVertexOutputVectors = 16,
                maxFragmentInputVectors = 15,
                minProgramTexelOffset = 8,
                maxProgramTexelOffset = 7,
                maxClipDistances = 8,
                maxComputeWorkGroupCountX = 65535,
                maxComputeWorkGroupCountY = 65535,
                maxComputeWorkGroupCountZ = 65535,
                maxComputeWorkGroupSizeX = 1024,
                maxComputeWorkGroupSizeY = 1024,
                maxComputeWorkGroupSizeZ = 64,
                maxComputeUniformComponents = 1024,
                maxComputeTextureImageUnits = 16,
                maxComputeImageUniforms = 8,
                maxComputeAtomicCounters = 8,
                maxComputeAtomicCounterBuffers = 1,
                maxVaryingComponents = 60 ,
                maxVertexOutputComponents = 64,
                maxGeometryInputComponents = 64,
                maxGeometryOutputComponents = 128,
                maxFragmentInputComponents = 128,
                maxImageUnits = 8,
                maxCombinedImageUnitsAndFragmentOutputs = 8,
                maxCombinedShaderOutputResources = 8,
                maxImageSamples = 0,
                maxVertexImageUniforms = 0,
                maxTessControlImageUniforms = 0,
                maxTessEvaluationImageUniforms = 0,
                maxGeometryImageUniforms = 0,
                maxFragmentImageUniforms = 8,
                maxCombinedImageUniforms = 8,
                maxGeometryTextureImageUnits = 16,
                maxGeometryOutputVertices = 256,
                maxGeometryTotalOutputComponents = 1024,
                maxGeometryUniformComponents = 1024,
                maxGeometryVaryingComponents = 64,
                maxTessControlInputComponents = 128,
                maxTessControlOutputComponents = 128,
                maxTessControlTextureImageUnits = 16,
                maxTessControlUniformComponents = 1024,
                maxTessControlTotalOutputComponents = 4096,
                maxTessEvaluationInputComponents = 128,
                maxTessEvaluationOutputComponents = 128,
                maxTessEvaluationTextureImageUnits = 16,
                maxTessEvaluationUniformComponents = 1024,
                maxTessPatchComponents = 120,
                maxPatchVertices = 32,
                maxTessGenLevel = 64,
                maxViewports = 16,
                maxVertexAtomicCounters = 0,
                maxTessControlAtomicCounters = 0,
                maxTessEvaluationAtomicCounters = 0,
                maxGeometryAtomicCounters = 0,
                maxFragmentAtomicCounters = 8,
                maxCombinedAtomicCounters = 8,
                maxAtomicCounterBindings = 1,
                maxVertexAtomicCounterBuffers = 0,
                maxTessControlAtomicCounterBuffers = 0,
                maxTessEvaluationAtomicCounterBuffers = 0,
                maxGeometryAtomicCounterBuffers = 0,
                maxFragmentAtomicCounterBuffers = 1,
                maxCombinedAtomicCounterBuffers = 1,
                maxAtomicCounterBufferSize = 16384,
                maxTransformFeedbackBuffers = 4,
                maxTransformFeedbackInterleavedComponents = 64,
                maxCullDistances = 8,
                maxCombinedClipAndCullDistances = 8,
                maxSamples = 4,
                limits = TLimits.Default
            )


    end

type Shader = nativeint
type Program = nativeint

module GLSLang =
    
    [<Literal>]
    let lib = "GLSLangNative"


    [<DllImport(lib)>]
    extern bool ShInitializeProcess()

    [<DllImport(lib)>]
    extern void ShFinalizeProcess()



    [<DllImport(lib)>]
    extern Shader ShCreateShader(ShLanguage language)

    [<DllImport(lib)>]
    extern void ShDestroyShader(Shader shader)

    [<DllImport(lib, EntryPoint = "ShSetShaderSource")>]
    extern bool ShSetShaderSource(Shader shader, nativeint shaderStrings, nativeint shaderStringLength, int numSources)
    //extern bool ShSetShaderSource(Shader shader, byte* shaderString, int shaderStringLength)

//    let ShSetShaderSource(shader : Shader, shaderString : string) =
//        let arr = System.Text.ASCIIEncoding.ASCII.GetBytes shaderString
//        let ptr = NativePtr.ofNativeInt (Marshal.AllocHGlobal (arr.Length + 1))
//        for i in 0..arr.Length-1 do NativePtr.set ptr i arr.[i]
//        NativePtr.set ptr arr.Length 0uy
//        
//        ShSetShaderSource_(shader, ptr, shaderString.Length)

    [<DllImport(lib)>]
    extern bool ShParseShader(Shader shader, TBuiltInResource* resources, int defaultVersion, bool forwardCompatible, ShMessages messages)

    [<DllImport(lib, EntryPoint = "ShGetShaderInfoLog")>]
    extern nativeint private ShGetShaderInfoLog_(Shader shader)

    let ShGetShaderInfoLog (shader : Shader) =
        let ptr = ShGetShaderInfoLog_(shader)
        Marshal.PtrToStringAnsi ptr

    [<DllImport(lib, EntryPoint = "ShGetShaderInfoDebugLog")>]
    extern nativeint private ShGetShaderInfoDebugLog_(Shader shader)

    let ShGetShaderInfoDebugLog (shader : Shader) =
        let ptr = ShGetShaderInfoDebugLog_(shader)
        Marshal.PtrToStringAnsi ptr



    [<DllImport(lib)>]
    extern Program ShCreateProgram()

    [<DllImport(lib)>]
    extern void ShDestroyProgram(Program program)

    [<DllImport(lib)>]
    extern bool ShAddShader(Program program, Shader shader)

    [<DllImport(lib)>]
    extern bool ShLinkProgram(Program program, ShMessages messages)

    [<DllImport(lib, EntryPoint = "ShGetProgramInfoLog")>]
    extern nativeint private ShGetProgramInfoLog_(Program shader)

    let ShGetProgramInfoLog (program : Program) =
        let ptr = ShGetProgramInfoLog_(program)
        Marshal.PtrToStringAnsi ptr

    [<DllImport(lib, EntryPoint = "ShGetProgramInfoDebugLog")>]
    extern nativeint private ShGetProgramInfoDebugLog_(Program shader)

    let ShGetProgramInfoDebugLog (program : Program) =
        let ptr = ShGetProgramInfoDebugLog_(program)
        Marshal.PtrToStringAnsi ptr


    [<DllImport(lib, EntryPoint = "ShGetSpirVForProgramStage")>]
    extern nativeint private ShGetSpirVForProgramStage_(Program program, ShLanguage stage, uint64* size)

    [<DllImport(lib)>]
    extern void private ShFreeSpirV(nativeint data, uint64 size)
    
    let ShGetSpirVForProgramStage(program : Program, stage : ShLanguage) =
        let mutable size = 0UL
        let ptr = ShGetSpirVForProgramStage_(program, stage, &&size)

        if ptr = 0n then null
        else
            let data : uint32[] = Array.zeroCreate (int (size / 4UL))
            let gc = GCHandle.Alloc(data, GCHandleType.Pinned)
            try 
                Marshal.Copy(ptr, gc.AddrOfPinnedObject(), size)
            finally 
                gc.Free()
                ShFreeSpirV(ptr, size)
            data
