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
