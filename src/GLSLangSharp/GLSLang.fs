namespace GLSLang

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
open Aardvark.Base

#nowarn "9"

type ShaderStage =
    | Vertex = 0
    | TessControl = 1
    | TessEvaluation = 2
    | Geometry = 3
    | Fragment = 4
    | Compute = 5

[<AutoOpen>]
module private ``GLSLang internal functions`` =
    let mutable private initialized = false

    let init() =
        if not initialized then
            initialized <- true
            let success = Raw.GLSLang.ShInitializeProcess()
            if not success then
                failwith "could not initialize GLSLang process"


module GLSLang =
    
    let tryCompile (stage : ShaderStage) (entryName : string) (defines : list<string>) (source : string) =
        init()

        let lang = stage |> int |> unbox<Raw.ShLanguage>
        let defines = List.toArray defines

        let mutable ptr = 0n
        let mutable size = 0un

        let mutable logLength = 0
        let mutable log = 0n

        try
            match Raw.GLSLang.ShCompileShader(lang, entryName, source, defines.Length, defines, &size, &ptr, &logLength, &log) with
                | 0 ->
                    let arr : byte[] = Array.zeroCreate (int size)
                    Marshal.Copy(ptr, arr, 0, int size)
                    let str = Marshal.PtrToStringAnsi(log, int logLength)
                    (Some arr, str)
                | _ ->
                    let str = Marshal.PtrToStringAnsi(log, int logLength)
                    (None, str)
        finally
            if ptr <> 0n then Raw.GLSLang.ShFree ptr
            if log <> 0n then Raw.GLSLang.ShFree log
