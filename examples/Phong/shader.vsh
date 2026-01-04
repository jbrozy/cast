uniform { matrix: mat4, lightDir: vec3 }
in { position: vec3, color: vec3, normals: vec3, texCoords: vec2 }

out { 
    outNormals: @loc(0) vec3, 
    fragPosition: @loc(1) vec3, 
    uv: @loc(2) vec2 
}

fn main() {
    gl_Position = matrix * vec4(position, 1.0);
    fragPosition = (matrix * vec4(position, 1.0)).xyz;
    uv = texCoords;
    
    outNormals = (matrix * vec4(normals, 1.0)).rgb;
    lightDirection = (matrix * vec4(lightDir, 1.0)).xyz;
}