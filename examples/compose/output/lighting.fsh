#version 330

uniform sampler2D txt;
void main() {
  mat4 modelWorld;
  mat4 worldView;
  mat4 viewProjection;
  mat4 modelProjection = viewProjection * worldView * modelWorld;
  vec4 m;
  vec4 p = modelProjection * m;  
}
