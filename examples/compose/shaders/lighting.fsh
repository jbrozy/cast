#version 330

uniform sampler2D txt;

void main() {
	mat4<Model, World> modelWorld;
	mat4<World, View>  worldView;
	mat4<View, Projection> viewProjection;
	
	mat4<Model, Projection> modelProjection = viewProjection * worldView * modelWorld;
	
	vec4<Model> m;
	vec4<Projection> p = modelProjection * m;
}