#version 460

out vec4 FragColor;

in point4<World> FragPos;
in vec3<World> Normal;
in vec2 TexCoords;

uniform sampler2D floorTexture;

uniform point3<World> lightPos;
uniform point3<World> viewPos;

uniform vec3 lightColor;
uniform bool blinn;

void main()
{
    vec3 color = texture(floorTexture, TexCoords).rgb;

    vec3 ambient = 0.05 * color;

    vec3<World> normal = normalize(Normal);
    vec3<World> lightDir = normalize(lightPos - FragPos.xyz);
    vec3<World> viewDir = normalize(viewPos - FragPos.xyz);

    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = diff * color;

    float spec = 0.0;

    if (blinn)
    {
        vec3<World> halfwayDir = normalize(lightDir + viewDir);
        spec = pow(max(dot(normal, halfwayDir), 0.0), 32.0);
    }
    else
    {
        vec3<World> reflectDir = reflect(-lightDir, normal);
        spec = pow(max(dot(viewDir, reflectDir), 0.0), 8.0);
    }

    vec3 specular = vec3(0.3) * spec;

    FragColor = vec4(ambient + diffuse + specular, 1.0);
}