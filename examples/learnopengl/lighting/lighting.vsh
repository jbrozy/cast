#version 460

layout (location = 0) in point3<Model> aPos;
layout (location = 1) in vec3<Model> aNormal;

out point4<World> FragPos;
out vec4<World> Normal;

uniform mat4<Model, World> model;
uniform mat4<Model, World> normalMatrix;

uniform mat4<World, View> view;
uniform mat4<View, Clip> projection;

void main()
{
    point4<Model> localPos = point4(aPos, 1.0);
    point4<World> worldPos = model * localPos;
    point4<View> viewPos = view * worldPos;
    point4<Clip> clipPos = projection * viewPos;

    FragPos = worldPos;
    vec4<Model> localNormal = vec4(aNormal, 0.0);
    Normal = normalize(normalMatrix * localNormal);

    gl_Position = clipPos;
}