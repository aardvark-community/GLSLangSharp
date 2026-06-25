#version 460

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
#extension GL_ARB_tessellation_shader : enable
#extension GL_EXT_opacity_micromap : enable
#extension GL_EXT_ray_tracing : enable
#extension GL_EXT_shader_invocation_reorder : enable

layout(std140, set = 0, binding = 0)
uniform Global
{
    int Blub;
};

layout(set = 0, binding = 1)
uniform accelerationStructureEXT RaytracingScene;


#ifdef RayGeneration

layout(location = 0) rayPayloadEXT int rayPayload0;

hitObjectEXT FShade_RaytracingIntrinsicsSER_newHitObject_uJz00jswdj_09khhbisPtqB0CJPI()
{
    hitObjectEXT temp;
    return temp;
}


int Raytracing_int32_i7ttpTfTzJg1qJ5J9X3akW3MbDE(vec3 origin, vec3 direction, int payload, hitObjectEXT ho)
{
    hitObjectTraceRayEXT(ho, RaytracingScene, 0, 255, 0, 1, 0, origin, 0.0010000000474974513, direction, 10000.0, 0);
    reorderThreadEXT(ho);
    reorderThreadEXT(32u, 1u);
    reorderThreadEXT(Blub, 1);
    reorderThreadEXT(ho, 32u, 1u);
    reorderThreadEXT(ho, 32, 1);
    rayPayload0 = payload;
    hitObjectExecuteShaderEXT(ho, 0);
    return rayPayload0;
}


hitObjectEXT Raytracing_myHitObject_GTPY7c1SrXBnCxTDIpum7XOr7Ts()
{
    return FShade_RaytracingIntrinsicsSER_newHitObject_uJz00jswdj_09khhbisPtqB0CJPI();
}


void main()
{
    Raytracing_int32_i7ttpTfTzJg1qJ5J9X3akW3MbDE(vec3(0.0, 0.0, 0.0), vec3(0.0, 0.0, 1.0), 42, FShade_RaytracingIntrinsicsSER_newHitObject_uJz00jswdj_09khhbisPtqB0CJPI());
    Raytracing_int32_i7ttpTfTzJg1qJ5J9X3akW3MbDE(vec3(0.0, 0.0, 0.0), vec3(0.0, 0.0, 1.0), 42, Raytracing_myHitObject_GTPY7c1SrXBnCxTDIpum7XOr7Ts());
}

#endif