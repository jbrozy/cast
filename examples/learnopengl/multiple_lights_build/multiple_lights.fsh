#version 460

out vec4 FragColor;

in vec4 FragPos;
in vec3 Normal;
in vec2 TexCoords;

uniform sampler2D materialDiffuse;
uniform sampler2D materialSpecular;
uniform float materialShininess;

uniform vec4 viewPos;
/* Directional Light */


uniform vec3 dirLightDirection;
uniform vec3 dirLightAmbient;
uniform vec3 dirLightDiffuse;
uniform vec3 dirLightSpecular;
/* Point Light */


uniform vec4 pointLightPosition;
uniform float pointLightConstant;
uniform float pointLightLinear;
uniform float pointLightQuadratic;
uniform vec3 pointLightAmbient;
uniform vec3 pointLightDiffuse;
uniform vec3 pointLightSpecular;
/* Spot Light */


uniform vec4 spotLightPosition;
uniform vec3 spotLightDirection;
uniform float spotLightCutOff;
uniform float spotLightOuterCutOff;
uniform float spotLightConstant;
uniform float spotLightLinear;
uniform float spotLightQuadratic;
uniform vec3 spotLightAmbient;
uniform vec3 spotLightDiffuse;
uniform vec3 spotLightSpecular;

void main() {

  vec3 diffuseColor = texture(materialDiffuse, TexCoords).rgb;
  vec3 specularColor = texture(materialSpecular, TexCoords).rgb;

  vec3 normal = normalize(Normal);
  vec3 viewDir = normalize(viewPos.xyz - FragPos.xyz);

  vec3 result = vec3(0.0);


  /* Directional Light */

      {
    vec3 lightDir = normalize(-dirLightDirection);

    float diff = max(dot(normal, lightDir), 0.0);

    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), materialShininess);

    vec3 ambient = dirLightAmbient * diffuseColor;
    vec3 diffuse = dirLightDiffuse * diff * diffuseColor;
    vec3 specular = dirLightSpecular * spec * specularColor;

    result = result + ambient + diffuse + specular;
    
}


  /* Point Light */

      {
    vec3 lightDir = normalize(pointLightPosition.xyz - FragPos.xyz);

    float diff = max(dot(normal, lightDir), 0.0);

    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), materialShininess);

    float distance = length(pointLightPosition.xyz - FragPos.xyz);
    float attenuation = 1.0 / (pointLightConstant + pointLightLinear * distance + pointLightQuadratic * distance * distance);

    vec3 ambient = pointLightAmbient * diffuseColor;
    vec3 diffuse = pointLightDiffuse * diff * diffuseColor;
    vec3 specular = pointLightSpecular * spec * specularColor;

    result = result + ambient * attenuation;
    result = result + diffuse * attenuation;
    result = result + specular * attenuation;
    
}


  /* Spot Light */

      {
    vec3 lightDir = normalize(spotLightPosition.xyz - FragPos.xyz);

    float diff = max(dot(normal, lightDir), 0.0);

    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), materialShininess);

    float distance = length(spotLightPosition.xyz - FragPos.xyz);
    float attenuation = 1.0 / (spotLightConstant + spotLightLinear * distance + spotLightQuadratic * distance * distance);

    float theta = dot(lightDir, normalize(-spotLightDirection));
    float epsilon = spotLightCutOff - spotLightOuterCutOff;
    float intensity = clamp((theta - spotLightOuterCutOff) / epsilon, 0.0, 1.0);

    vec3 ambient = spotLightAmbient * diffuseColor;
    vec3 diffuse = spotLightDiffuse * diff * diffuseColor;
    vec3 specular = spotLightSpecular * spec * specularColor;

    result = result + ambient * attenuation * intensity;
    result = result + diffuse * attenuation * intensity;
    result = result + specular * attenuation * intensity;
    
}

  FragColor = vec4(result, 1.0);
}
