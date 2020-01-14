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
            string entryName, 
            string code, 
            int nDefines, string[] defines, 
            unativeint& outputSize, nativeint& output, 
            int& logLength, nativeint& log
        )

    [<DllImport(lib); SuppressUnmanagedCodeSecurity>]
    extern void ShFree(void* data)

    
    [<DllImport(lib); SuppressUnmanagedCodeSecurity>]
    extern void ShOptimize( 
            nativeint input, unativeint inputLength,
            nativeint& output, unativeint& outputLength,
            string[] passNames, int passCount
        )
