uniform { matrix: mat4, lightDir: vec3 }
in { position: vec3, color: vec3, normals: vec3, texCoords: vec2 }

out { 
    outNormals: @loc(0) vec3, 
    fragPosition: @loc(1) vec3, 
    uv: @loc(2) vec2 
}

fn main() {
    let a = vec3<World>(1.0);
    let b = vec3<World>(1.0);
    let c : vec3<World> = a * b;
}
