#version 460

layout(location = 0) in point2 aPos;
layout(location = 1) in vec2 aTexCoords;

out vec2 TexCoords;

void main()
{
    TexCoords = aTexCoords;
    gl_Position = point4<Clip>(aPos, 0.0, 1.0);
}