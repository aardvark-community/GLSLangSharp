// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Aardvark.Base
open GLSLang
open GLSLang.SpirV
let code = """#version 420 core
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
#extension GL_ARB_tessellation_shader : enable

layout(set = 0, binding = 0) uniform sampler2D samplers[2];

layout(location = 0) in vec2 DiffuseColorCoordinates;
layout(location = 0) out vec4 ColorsOut;
void main()
{
    int index = (((DiffuseColorCoordinates.x > 0.5)) ? (1) : (0));
    ColorsOut = texture(samplers[index], DiffuseColorCoordinates);
}
"""

[<EntryPoint>]
let main argv = 

    let strBuilder = System.Text.StringBuilder()
    //let printfn fmt = Printf.kprintf (fun s -> strBuilder.AppendLine s |> ignore) fmt

    match GLSLang.tryCompileSpirVBinary ShaderStage.Vertex code with
        | Success theirs ->
            let theirs = theirs.UnsafeCoerce<uint32>()
            let m = Module.ofArray theirs

            for i in m.instructions do
                match Instruction.tryGetId i with
                    | Some id -> printfn "%d:\t%A" id i
                    | None -> printfn "   \t%A" i

            
            let iface = SpirVReflection.getInterface m

            printfn "%A" iface

            let mine = Module.toArray m

 
            let equalLength = theirs.Length = mine.Length
            let equal = equalLength && Array.forall2 (=) theirs mine
            printfn "length: %A equal: %A" equalLength equal

            //System.IO.File.WriteAllBytes(@"C:\Users\steinlechner\Desktop\test.spv", theirs)

            //System.IO.File.WriteAllText(@"C:\Users\steinlechner\Desktop\test.txt",strBuilder.ToString())
        | Error e ->
            printfn "%s" e

    0 // return an integer exit code
