// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Aardvark.Base
open GLSLang

let code = """#version 140
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
layout(binding = 0) uniform buf {
        mat4 MVP;
        vec4 position[12*3];
        vec4 attr[12*3];
} ubuf;

layout (location = 0) out vec4 texcoord;
void main() 
{
    texcoord = ubuf.attr[gl_VertexID];
    gl_Position = ubuf.MVP * ubuf.position[gl_VertexID];

    // GL->VK conventions
    gl_Position.y = -gl_Position.y;
    gl_Position.z = (gl_Position.z + gl_Position.w) / 2.0;
}

    """

[<EntryPoint>]
let main argv = 


    match GLSLang.tryCompileSpirVBinary ShaderStage.Vertex code with
        | Success spirv ->
            System.IO.File.WriteAllBytes(@"C:\Users\schorsch\Desktop\test.spv", spirv)
        | Error e ->
            printfn "%s" e

    0 // return an integer exit code
