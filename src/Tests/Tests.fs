module Tests

open Expecto
open GLSLang
open System.IO
open Aardvark.Base

[<Tests>]
let tests =
    let compileOptimize (target: Target) (stage: ShaderStage) (debug: bool) (defines: string list) (glsl: string) =
        match GLSLang.tryCompileWithTarget target stage "main" debug defines glsl with
        | Some binary, _ ->
            if not debug then GLSLang.optimizeDefault binary |> ignore

        | _, log
             -> failtestf "Failed to compiled shader: %s" log

    let buildTestCase (fileName, target, stage, defines) =
        test fileName {
            Aardvark.Init()

            let glsl = File.ReadAllText <| Path.Combine(__SOURCE_DIRECTORY__, "shaders", fileName)

            Report.Line("Compiling (debug = false)")
            compileOptimize target stage false defines glsl

            Report.Line("Compiling (debug = true)")
            compileOptimize target stage true defines glsl
        }

    [
        "test.glsl",         Target.SPIRV_1_0, ShaderStage.Vertex,     ["Vertex"]
        "test_rtx.glsl",     Target.SPIRV_1_4, ShaderStage.ClosestHit, ["RayClosestHit"]
        "test_rtx_ser.glsl", Target.SPIRV_1_4, ShaderStage.RayGen,     ["RayGeneration"]
    ]
    |> List.map buildTestCase
    |> testList "Tests"