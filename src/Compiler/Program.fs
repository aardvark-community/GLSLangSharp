// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Aardvark.Base
open GLSLang
open GLSLang.SpirV
let code = """#version 140
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
layout( binding = 0) uniform buf {
        mat4 MVP;
        vec4 position[12*3];
        vec4 attr[12*3];
} ubuf;

layout(location = 0) in vec4 myPos;

layout (location = 0) out vec4 texcoord;
void main() 
{
    texcoord = ubuf.attr[gl_VertexID];
    gl_Position = ubuf.MVP * ubuf.position[gl_VertexID] + myPos;

    // GL->VK conventions
    gl_Position.y = -gl_Position.y;
    gl_Position.z = (gl_Position.z + gl_Position.w) / 2.0;
}

    """

[<EntryPoint>]
let main argv = 

    let strBuilder = System.Text.StringBuilder()
    //let printfn fmt = Printf.kprintf (fun s -> strBuilder.AppendLine s |> ignore) fmt

    match GLSLang.tryCompileSpirVBinary ShaderStage.Vertex code with
        | Success theirs ->
            let instructions = SpirV.disassemble theirs

            for i in instructions do
                match SpirV.tryGetResultId i with
                    | Some id -> printfn "%d:\t%A" id i
                    | None -> printfn "   \t%A" i

            
            let iface = SpirVReflection.getInterface instructions

            printfn "%A" iface

            let mine = SpirV.assemble instructions

 
            let equalLength = theirs.Length = mine.Length
            let equal = equalLength && Array.forall2 (=) theirs mine
            printfn "length: %A equal: %A" equalLength equal

            System.IO.File.WriteAllBytes(@"C:\Users\steinlechner\Desktop\test.spv", theirs)

            //System.IO.File.WriteAllText(@"C:\Users\steinlechner\Desktop\test.txt",strBuilder.ToString())
        | Error e ->
            printfn "%s" e

    0 // return an integer exit code
