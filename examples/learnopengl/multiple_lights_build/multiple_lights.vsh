#version 460

layout(location=0) in vec3 aPos;
layout(location=1) in vec3 aNormal;
layout(location=2) in vec2 aTexCoords;

out vec4 FragPos;
out vec3 Normal;
out vec2 TexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform mat3 normalMatrix;

void main() {

  vec4 localPos = vec4(aPos, 1.0);

  vec4 worldPos = model * localPos;
  vec4 viewPos = view * worldPos;
  vec4 clipPos = projection * viewPos;

  FragPos = worldPos;
  Normal = normalize(normalMatrix * aNormal);
  TexCoords = aTexCoords;

  gl_Position = clipPos;
}
