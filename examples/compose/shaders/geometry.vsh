#version 330

out vec3 vPos;

void main() {
	vec4<Model> a 		= vec4(vec3(1.0), 1.0);
	mat4<Model, World> 	modelWorld;
	mat4<World, View> 	worldView;
	vec4<World> b 		= modelWorld * a;
}