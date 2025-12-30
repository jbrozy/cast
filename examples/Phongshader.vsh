#version 330

uniform mat4 matrix;
uniform vec3 lightDir;

in vec3 position;
in vec3 color;
in vec3 normals;
in vec2 texCoords;

layout(location = 0) out vec3 outNormals;
layout(location = 1) out vec3 fragPosition;
layout(location = 2) out vec2 uv;

void main() {
	gl_Position = matrix * vec4(position, 1.0);
	fragPosition = matrix * vec4(position, 1.0).xyz;
	uv = texCoords;
	outNormals = matrix * vec4(normals, 1.0).rgb;
	lightDirection = matrix * vec4(lightDir, 1.0).xyz;
}
