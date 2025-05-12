namespace GLSLang.Raw

open System.Security
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
    | RayGen = 6
    | Intersect = 7
    | AnyHit = 8
    | ClosestHit = 9
    | Miss = 10
    | Callable = 11
    | Task = 12
    | Mesh = 13

type ShTargetLanguageVersion =
    | Spv_1_0 = 65536
    | Spv_1_1 = 65792
    | Spv_1_2 = 66048
    | Spv_1_3 = 66304
    | Spv_1_4 = 66560
    | Spv_1_5 = 66816

module GLSLang =
    
    [<Literal>]
    let lib = "GLSLangNative"


    [<DllImport(lib); SuppressUnmanagedCodeSecurity>]
    extern bool ShInitializeProcess()

    [<DllImport(lib); SuppressUnmanagedCodeSecurity>]
    extern void ShFinalizeProcess()

    // ShCompileShader(
    //      EShLanguage lang, 
    //      const char* entryName, 
    //      const char* code, 
    //      int nDefines, const char* defines[], 
    //      size_t* outputSize, void** output, 
    //      int* logLength, char** log
    //  )
    [<DllImport(lib); SuppressUnmanagedCodeSecurity>]
    extern int ShCompileShader(
            ShLanguage lang,
            ShTargetLanguageVersion version,
            string entryName, 
            string code, 
            int nDefines, string[] defines, 
            unativeint& outputSize, nativeint& output, 
            int& logLength, nativeint& log, bool debug
        )

    [<DllImport(lib); SuppressUnmanagedCodeSecurity>]
    extern void ShFree(void* data)

    
    [<DllImport(lib); SuppressUnmanagedCodeSecurity>]
    extern void ShOptimize( 
            nativeint input, unativeint inputLength,
            nativeint& output, unativeint& outputLength,
            string[] passNames, int passCount
        )
