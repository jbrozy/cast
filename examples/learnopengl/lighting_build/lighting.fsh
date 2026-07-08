#version 460

out vec4 FragColor;

in vec4 FragPos;
in vec4 Normal;

uniform vec4 lightPos;
uniform vec4 viewPos;

uniform vec3 lightColor;
uniform vec3 objectColor;

void main() {

  float ambientStrength = 0.1;
  vec3 ambient = ambientStrength * lightColor;

  vec4 norm = normalize(Normal);
  vec4 lightDir = normalize(lightPos - FragPos);

  float diff = max(dot(norm, lightDir), 0.0);
  vec3 diffuse = diff * lightColor;

  float specularStrength = 0.5;

  vec4 viewDir = normalize(viewPos - FragPos);
  vec4 reflectDir = reflect(-lightDir, norm);

  float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
  vec3 specular = specularStrength * spec * lightColor;

  vec3 result = (ambient + diffuse + specular) * objectColor;
  FragColor = vec4(result, 1.0);
}
