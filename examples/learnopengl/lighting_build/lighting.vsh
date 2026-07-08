#version 460

layout(location=0) in vec3 aPos;
layout(location=1) in vec3 aNormal;

out vec4 FragPos;
out vec4 Normal;

uniform mat4 model;
uniform mat4 normalMatrix;

uniform mat4 view;
uniform mat4 projection;

void main() {

  vec4 localPos = vec4(aPos, 1.0);
  vec4 worldPos = model * localPos;
  vec4 viewPos = view * worldPos;
  vec4 clipPos = projection * viewPos;

  FragPos = worldPos;
  vec4 localNormal = vec4(aNormal, 0.0);
  Normal = normalize(normalMatrix * localNormal);

  gl_Position = clipPos;
}
