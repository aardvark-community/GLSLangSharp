namespace GLSLang.Raw

open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#nowarn "9"
#nowarn "51"


type private cstr = nativeptr<byte>


type EShLanguage =
    | EShLangVertex = 0
    | EShLangTessControl = 1
    | EShLangTessEvaluation = 2
    | EShLangGeometry = 3
    | EShLangFragment = 4
    | EShLangCompute = 5
    | EShLangCount = 6

type EShLanguageMask =
    | EShLangVertexMask         = 0x00000001
    | EShLangTessControlMask    = 0x00000002
    | EShLangTessEvaluationMask = 0x00000004
    | EShLangGeometryMask       = 0x00000008
    | EShLangFragmentMask       = 0x00000010
    | EShLangComputeMask        = 0x00000020

type EShExecutable =
    | EShExVertexFragment = 0
    | EShExFragment = 1

type EShOptimizationLevel =
    | EShOptNoGeneration = 0
    | EShOptNone = 1
    | EShOptSimple = 2
    | EShOptFull = 3

type EshMessages =
    | EShMsgDefault          = 0x0000000  // default is to give all required errors and extra warnings
    | EShMsgRelaxedErrors    = 0x0000001  // be liberal in accepting input
    | EShMsgSuppressWarnings = 0x0000002  // suppress all warnings, except those required by the specification
    | EShMsgAST              = 0x0000004  // print the AST intermediate representation
    | EShMsgSpvRules         = 0x0000008  // issue messages for SPIR-V generation
    | EShMsgVulkanRules      = 0x0000010  // issue messages for Vulkan-requirements of GLSL for SPIR-V
    | EShMsgOnlyPreprocessor = 0x0000020  // only print out errors produced by the preprocessor

    
[<StructLayout(LayoutKind.Sequential)>]
type ShBinding =
    struct
        val mutable public name : cstr
        val mutable public binding : int
    end

[<StructLayout(LayoutKind.Sequential)>]
type ShBindingTable =
    struct
        val mutable public numBindings : int
        val mutable public bindings : nativeptr<ShBinding>
    end

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
    end

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
    end

type ShHandle = nativeint

module GLSLang =

    
    [<Literal>]
    let lib = "glslang"

    [<DllImport(lib)>]
    extern int ShInitialize()

    [<DllImport(lib)>]
    extern int ShFinalize()


    [<DllImport(lib)>]
    extern ShHandle ShConstructCompiler(EShLanguage language, int debugOptions)

    [<DllImport(lib)>]
    extern ShHandle ShConstructLinker(EShExecutable, int debugOptions)

    [<DllImport(lib)>]
    extern ShHandle ShConstructUniformMap()

    [<DllImport(lib)>]
    extern void ShDestruct(ShHandle handle)

    [<DllImport(lib)>]
    extern int ShCompile(
        ShHandle handle,
        cstr* shaderStrings,
        int numStrings,
        int* lengths,
        EShOptimizationLevel optimizationLevel,
        TBuiltInResource* resources,
        int debugOptions,
        int defaultVersion,
        bool forwardCompatible,
        EshMessages messages
    )


    type private int16arr = nativeptr<int16>

    [<DllImport(lib)>]
    extern int ShLink(
        ShHandle handle,
        ShHandle* h,
        int numHandles,
        ShHandle uniformMap,
        int16arr* uniformsAccessed,
        int* numUniformsAccessed
    )

    [<DllImport(lib)>]
    extern int ShLinkExt(
        ShHandle handle,
        ShHandle* h,
        int numHandles
    )

    [<DllImport(lib)>]
    extern void ShSetEncryptionMethod(ShHandle handle)

    [<DllImport(lib)>]
    extern string ShGetInfoLog(ShHandle handle)

    [<DllImport(lib)>]
    extern nativeint ShGetExecutable(ShHandle handle)

    [<DllImport(lib)>]
    extern int ShSetVirtualAttributeBindings(ShHandle handle, ShBindingTable* table)

    [<DllImport(lib)>]
    extern int ShSetFixedAttributeBindings(ShHandle handle, ShBindingTable* table)

    type private ShBindingTablePtr = nativeptr<ShBindingTable>

    [<DllImport(lib)>]
    extern int ShGetPhysicalAttributeBindings(ShHandle handle, ShBindingTablePtr* table)

    [<DllImport(lib)>]
    extern int ShExcludeAttributes(ShHandle handle, int *attributes, int count)

    [<DllImport(lib)>]
    extern int ShGetUniformLocation(ShHandle uniformMap, string name)