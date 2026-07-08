#version 460

out vec4 FragColor;

in point4<World> FragPos;
in vec4<World> Normal;

uniform point4<World> lightPos;
uniform point4<World> viewPos;

uniform vec3 lightColor;
uniform vec3 objectColor;

void main()
{
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

    vec4<World> norm = normalize(Normal);
    vec4<World> lightDir = normalize(lightPos - FragPos);

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    float specularStrength = 0.5;

    vec4<World> viewDir = normalize(viewPos - FragPos);
    vec4<World> reflectDir = reflect(-lightDir, norm);

    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
    vec3 specular = specularStrength * spec * lightColor;

    vec3 result = (ambient + diffuse + specular) * objectColor;
    FragColor = vec4(result, 1.0);
}