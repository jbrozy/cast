#version 460

out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D<point4<World>> gPosition;
uniform sampler2D gNormal;
uniform sampler2D gAlbedoSpec;

uniform point4<World> viewPos;

/* Point Light 0 */
uniform point4<World> light0Position;
uniform vec3 light0Color;
uniform float light0Linear;
uniform float light0Quadratic;

/* Point Light 1 */
uniform point4<World> light1Position;
uniform vec3 light1Color;
uniform float light1Linear;
uniform float light1Quadratic;

void main()
{
    point4<World> fragPos = texture(gPosition, TexCoords);
	vec4 N = texture(gNormal, TexCoords);
    vec3<World> normal = normalize(N.xyz);

    vec3 diffuseColor = texture(gAlbedoSpec, TexCoords).rgb;
    float specularStrength = texture(gAlbedoSpec, TexCoords).a;

    vec3<World> viewDir = normalize(viewPos.xyz - fragPos.xyz);

    vec3 lighting = diffuseColor * 0.1;

    {
        vec3<World> lightDir = normalize(light0Position.xyz - fragPos.xyz);

        float diff = max(dot(normal, lightDir), 0.0);

        vec3<World> halfwayDir = normalize(lightDir + viewDir);
        float spec = pow(max(dot(normal, halfwayDir), 0.0), 16.0);

        float distance = length(light0Position.xyz - fragPos.xyz);
        float attenuation = 1.0 / (
            1.0 +
            light0Linear * distance +
            light0Quadratic * distance * distance
        );

        vec3 diffuse = diff * diffuseColor * light0Color;
        vec3 specular = spec * specularStrength * light0Color;

        lighting = lighting + (diffuse + specular) * attenuation;
    }

    {
        vec3<World> lightDir = normalize(light1Position.xyz - fragPos.xyz);

        float diff = max(dot(normal, lightDir), 0.0);

        vec3<World> halfwayDir = normalize(lightDir + viewDir);
        float spec = pow(max(dot(normal, halfwayDir), 0.0), 16.0);

        float distance = length(light1Position.xyz - fragPos.xyz);
        float attenuation = 1.0 / (
            1.0 +
            light1Linear * distance +
            light1Quadratic * distance * distance
        );

        vec3 diffuse = diff * diffuseColor * light1Color;
        vec3 specular = spec * specularStrength * light1Color;

        lighting = lighting + (diffuse + specular) * attenuation;
    }

    FragColor = vec4(lighting, 1.0);
}