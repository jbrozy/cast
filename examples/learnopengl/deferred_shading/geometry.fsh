#version 460

layout(location = 0) out point4<World> gPosition;
layout(location = 1) out vec3<World> gNormal;
layout(location = 2) out vec4 gAlbedoSpec;

in point4<World> FragPos;
in vec3<World> Normal;
in vec2 TexCoords;

uniform sampler2D textureDiffuse;
uniform sampler2D textureSpecular;

void main()
{
    gPosition = FragPos;
    gNormal = normalize(Normal);

    gAlbedoSpec.rgb = texture(textureDiffuse, TexCoords).rgb;
    gAlbedoSpec.a = texture(textureSpecular, TexCoords).r;
}