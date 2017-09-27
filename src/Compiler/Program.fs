// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Aardvark.Base
open GLSLang
open GLSLang.SpirV
let code = """
#version 450 core

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
#extension GL_ARB_tessellation_shader : enable


layout(set = 0, binding = 0) uniform sampler2D samplers[2];

layout(set = 0, binding = 1) 
uniform BufferA
{
    float ValueA;
    float ValueB;
    float ValueC;
};


layout(set = 0, binding = 2) 
uniform sampler2D thing;


#ifdef Fragment

layout(location = 0) in vec2 DiffuseColorCoordinates;
layout(location = 0) out vec4 ColorsOut;
void main()
{

    int index = (((DiffuseColorCoordinates.x > 0.5)) ? (1) : (0));
    ColorsOut = texture(samplers[index], DiffuseColorCoordinates);
}

#endif

#ifdef Vertex


layout(location = 0) in vec4 Positions;
layout(location = 1) in vec2 TexCoord;
void main()
{
    gl_Position = Positions * ValueA;
}

#endif

"""

[<EntryPoint>]
let main argv = 
    let perf() = 
        for i in 1 .. 10 do
            GLSLang.tryCompile ShaderStage.Vertex "main" ["Vertex"] code |> ignore

        let iter = 100
        let sw = System.Diagnostics.Stopwatch.StartNew()
        for i in 1 .. iter do
            GLSLang.tryCompile ShaderStage.Vertex "main" ["Vertex"] code |> ignore

        sw.Stop()
        printfn "took: %.2fµs" (1000.0 * sw.Elapsed.TotalMilliseconds / float (iter))

    match GLSLang.tryCompile ShaderStage.Vertex "main" ["Vertex"] code with
        | Some binary, log ->
            let m = Module.ofArray binary

            for i in m.instructions do
                match Instruction.tryGetId i with
                    | Some id -> printfn "%d:\t%A" id i
                    | None -> printfn "   \t%A" i

            printfn "%s" log
        | None, e ->
            printfn "%s" e

    0 // return an integer exit code
