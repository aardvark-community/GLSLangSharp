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

type Shader internal(handle : Raw.Shader) =
   
    static let resources = Marshal.AllocHGlobal sizeof<Raw.TBuiltInResource>
    static do init(); Marshal.StructureToPtr(Raw.TBuiltInResource.Default, resources, false)

    let mutable handle = handle
    let mutable sourcesPtr = sizeof<nativeint> |> Marshal.AllocHGlobal
    let mutable sourcesLengthPtr = sizeof<int> |> Marshal.AllocHGlobal
    let mutable source = 0n

    member internal x.Handle = 
        if handle = 0n then
            raise <| ObjectDisposedException("Shader")
        handle

    member x.SetSource(code : string) =
        if handle = 0n then
            raise <| ObjectDisposedException("Shader")

        if source <> 0n then 
            Marshal.FreeHGlobal source
            source <- 0n

        source <- Marshal.StringToHGlobalAnsi code
        Marshal.WriteIntPtr(sourcesPtr, source)
        Marshal.WriteInt32(sourcesLengthPtr, code.Length)

        let success = Raw.GLSLang.ShSetShaderSource(handle, sourcesPtr, sourcesLengthPtr, 1)
        if not success then
            failwith "could not set shader source"

    member x.TryParse() =
        if handle = 0n then
            raise <| ObjectDisposedException("Shader")

        let success = Raw.GLSLang.ShParseShader(handle, NativePtr.ofNativeInt resources, 140, false, Raw.ShMessages.Default)

        if success then (true, "")
        else (false, Raw.GLSLang.ShGetShaderInfoLog(handle))

    member x.Dispose() =
        if handle <> 0n then
            Raw.GLSLang.ShDestroyShader(handle)
            handle <- 0n

        if sourcesPtr <> 0n then
            Marshal.FreeHGlobal sourcesPtr
            sourcesPtr <- 0n

        if sourcesLengthPtr <> 0n then
            Marshal.FreeHGlobal sourcesLengthPtr
            sourcesLengthPtr <- 0n

        if source <> 0n then 
            Marshal.FreeHGlobal source
            source <- 0n

    interface IDisposable with
        member x.Dispose() = x.Dispose()

    new(stage : ShaderStage) = new Shader(Raw.GLSLang.ShCreateShader (stage |> int |> unbox<Raw.ShLanguage>))

type Program internal(handle : Raw.Program) =
    static do init()

    let mutable linked = false
    let mutable handle = handle
    member internal x.Handle = handle

    member x.AddShader(shader : Shader) =
        if handle = 0n then
            raise <| ObjectDisposedException("Program")

        let success = Raw.GLSLang.ShAddShader(handle, shader.Handle)
        if not success then
            failwith "could not add shader to program"

    member x.TryLink() =
        if handle = 0n then
            raise <| ObjectDisposedException("Program")

        let success = Raw.GLSLang.ShLinkProgram(handle, Raw.ShMessages.Default)

        if success then 
            linked <- true
            (true, "")
        else 
            (false, Raw.GLSLang.ShGetProgramInfoLog(handle))

    member x.TryGetSpirVForStage (stage : ShaderStage) =
        if handle = 0n then
            raise <| ObjectDisposedException("Program")

        if not linked then
            failwith "cannot get SpirV for unlinked program (link the program prior to calling GetSpirVForStage)"

        let res = Raw.GLSLang.ShGetSpirVForProgramStage(handle, stage |> int |> unbox<_>)
        match res with
            | null -> None
            | res -> Some res

    member x.Dispose() =
        if handle <> 0n then
            Raw.GLSLang.ShDestroyProgram handle
            handle <- 0n
            linked <- false

    interface IDisposable with
        member x.Dispose() = x.Dispose()

    new() = new Program(Raw.GLSLang.ShCreateProgram())


module GLSLang =
    
    let tryCreateShader (stage : ShaderStage) (source : string) =
        let shader = new Shader(stage)
        shader.SetSource(source)

        match shader.TryParse() with
            | (true, _) -> Success shader
            | (false, msg) -> 
                shader.Dispose()
                Error msg

    let tryCreateProgram (shaders : seq<Shader>) =
        let prog = new Program()


        for s in shaders do
            prog.AddShader s

        match prog.TryLink() with
            | (true, _) -> Success prog
            | (false, log) -> Error log

    let tryGetSpirVBinaryForStage (program : Program) (stage : ShaderStage) =
        program.TryGetSpirVForStage stage


    let tryCompileSpirVBinary (stage : ShaderStage) (glslCode : string) =
        match tryCreateShader stage glslCode with
            | Error e -> Error e
            | Success shader -> 
                match tryCreateProgram [shader] with
                    | Error e -> 
                        shader.Dispose()
                        Error e
                    | Success program ->
                        let result = 
                            match tryGetSpirVBinaryForStage program stage with
                                | Some spirv ->
                                    Success spirv
                                | None ->
                                    Error "could not get SpirV code"
                        program.Dispose()
                        shader.Dispose()
                        result