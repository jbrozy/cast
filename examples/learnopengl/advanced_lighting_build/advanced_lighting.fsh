#version 460

out vec4 FragColor;

in vec4 FragPos;
in vec3 Normal;
in vec2 TexCoords;

uniform sampler2D floorTexture;

uniform vec3 lightPos;
uniform vec3 viewPos;

uniform vec3 lightColor;
uniform bool blinn;

void main() {

  vec3 color = texture(floorTexture, TexCoords).rgb;

  vec3 ambient = 0.05 * color;

  vec3 normal = normalize(Normal);
  vec3 lightDir = normalize(lightPos - FragPos.xyz);
  vec3 viewDir = normalize(viewPos - FragPos.xyz);

  float diff = max(dot(normal, lightDir), 0.0);
  vec3 diffuse = diff * color * lightColor;

  float spec = 0.0;

    if (blinn) {
      vec3 halfwayDir = normalize(lightDir + viewDir);
      spec = pow(max(dot(normal, halfwayDir), 0.0), 32.0);
  } else {


        vec3 reflectDir = reflect(-lightDir, normal);
        spec = pow(max(dot(viewDir, reflectDir), 0.0), 8.0);
    }

  vec3 specular = vec3(0.3) * spec * lightColor;

  FragColor = vec4(ambient + diffuse + specular, 1.0);
}
