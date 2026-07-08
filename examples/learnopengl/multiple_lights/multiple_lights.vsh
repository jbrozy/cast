#version 460

layout(location = 0) in point3<Model> aPos;
layout(location = 1) in vec3<Model> aNormal;
layout(location = 2) in vec2 aTexCoords;

out point4<World> FragPos;
out vec3<World> Normal;
out vec2 TexCoords;

uniform mat4<Model, World> model;
uniform mat4<World, View> view;
uniform mat4<View, Clip> projection;

uniform mat3<Model, World> normalMatrix;

void main()
{
    point4<Model> localPos = point4(aPos, 1.0);

    point4<World> worldPos = model * localPos;
    point4<View> viewPos = view * worldPos;
    point4<Clip> clipPos = projection * viewPos;

    FragPos = worldPos;
    Normal = normalize(normalMatrix * aNormal);
    TexCoords = aTexCoords;

    gl_Position = clipPos;
}