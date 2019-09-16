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


int myfun(int a, int b) 
{
    int x = a*a;
    for(int i = 0; i < b; i++) 
    {
        x = x * b;
    }
    return x;
}

layout(location = 0) in vec2 DiffuseColorCoordinates;
layout(location = 0) out vec4 ColorsOut;
void main()
{
    if(1 > 2) discard;


    ivec2 s = textureSize(samplers[0], 0);

    int index = 1 - 1 + (((DiffuseColorCoordinates.x > 0.5)) ? (1) : (0));
    vec4 t = texelFetch(samplers[0], ivec2(index, textureSize(samplers[0], 0).y - 1 - index), 0);

    int sepp = myfun(index, myfun(s.x,index));
    ColorsOut = t + texture(samplers[sepp], DiffuseColorCoordinates);
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

let rclosest = """
#version 460
#extension GL_NV_ray_tracing : require
#extension GL_EXT_nonuniform_qualifier : enable

#ifdef RayClosestHit

struct RayPayload {
	vec3 color;
	float distance;
	vec3 normal;
	float reflector;
};

layout(location = 0) rayPayloadInNV RayPayload rayPayload;

hitAttributeNV vec3 attribs;

layout(binding = 0, set = 0) uniform accelerationStructureNV topLevelAS;
layout(binding = 2, set = 0) uniform CameraProperties 
{
	mat4 viewInverse;
	mat4 projInverse;
	vec4 lightPos;
} cam;
layout(binding = 3, set = 0) buffer Vertices { vec4 v[]; } vertices;
layout(binding = 4, set = 0) buffer Indices { uint i[]; } indices;

struct Vertex
{
  vec3 pos;
  vec3 normal;
  vec3 color;
  vec2 uv;
  float _pad0;
};

Vertex unpack(uint index)
{
	vec4 d0 = vertices.v[3 * index + 0];
	vec4 d1 = vertices.v[3 * index + 1];
	vec4 d2 = vertices.v[3 * index + 2];

	Vertex v;
	v.pos = d0.xyz;
	v.normal = vec3(d0.w, d1.x, d1.y);
	v.color = vec3(d1.z, d1.w, d2.x);
	return v;
}

void main()
{
	ivec3 index = ivec3(indices.i[3 * gl_PrimitiveID], indices.i[3 * gl_PrimitiveID + 1], indices.i[3 * gl_PrimitiveID + 2]);

	Vertex v0 = unpack(index.x);
	Vertex v1 = unpack(index.y);
	Vertex v2 = unpack(index.z);

	// Interpolate normal
	const vec3 barycentricCoords = vec3(1.0f - attribs.x - attribs.y, attribs.x, attribs.y);
	vec3 normal = normalize(v0.normal * barycentricCoords.x + v1.normal * barycentricCoords.y + v2.normal * barycentricCoords.z);

	// Basic lighting
	vec3 lightVector = normalize(cam.lightPos.xyz);
	float dot_product = max(dot(lightVector, normal), 0.6);
	rayPayload.color = v0.color * vec3(dot_product);
	rayPayload.distance = gl_RayTmaxNV;
	rayPayload.normal = normal;

	// Objects with full white vertex color are treated as reflectors
	rayPayload.reflector = ((v0.color.r == 1.0f) && (v0.color.g == 1.0f) && (v0.color.b == 1.0f)) ? 1.0f : 0.0f; 
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

    match GLSLang.tryCompile ShaderStage.RayClosestHit "main" ["RayClosestHit"] rclosest with
        | Some binary, log ->
            let code = System.Text.StringBuilder()
            let m = Module.ofArray binary
            for i in m.instructions do
                match Instruction.tryGetId i with
                    | Some id -> code.AppendLine(sprintf "%d:\t%A" id i) |> ignore
                    | None -> code.AppendLine(sprintf "   \t%A" i) |> ignore
            File.writeAllText @"C:\Users\Schorsch\Desktop\org.spv" (code.ToString())
            
            let binary = GLSLang.optimizeDefault binary
            
            let code = System.Text.StringBuilder()
            let m = Module.ofArray binary
            for i in m.instructions do
                match Instruction.tryGetId i with
                    | Some id -> code.AppendLine(sprintf "%d:\t%A" id i) |> ignore
                    | None -> code.AppendLine(sprintf "   \t%A" i) |> ignore
            File.writeAllText @"C:\Users\Schorsch\Desktop\opt.spv" (code.ToString())

        | None, e ->
            printfn "%s" e

    0 // return an integer exit code
