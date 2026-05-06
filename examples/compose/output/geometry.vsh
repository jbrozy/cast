#version 330

out vec3 vPos;
void main() {
  vec4 a = vec4(vec3(1.0), 1.0);
  mat4 modelWorld;
  mat4 worldView;
  vec4 b = modelWorld * a;  
}
