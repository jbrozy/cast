#version 460

layout(location=0) out vec4 gPosition;
layout(location=1) out vec3 gNormal;
layout(location=2) out vec4 gAlbedoSpec;

in vec4 FragPos;
in vec3 Normal;
in vec2 TexCoords;

uniform sampler2D textureDiffuse;
uniform sampler2D textureSpecular;

void main() {

  gPosition = FragPos;
  gNormal = normalize(Normal);

  gAlbedoSpec.rgb = texture(textureDiffuse, TexCoords).rgb;
  gAlbedoSpec.a = texture(textureSpecular, TexCoords).r;
}
