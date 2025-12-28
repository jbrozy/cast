uniform {
    modelViewMatrix : mat4,
    projectionMatrix : mat4,
    chunkOffset : vec3,
    normalMatrix : mat3,
    gbufferModelViewInverse : mat4
}

in {
    vaPosition : vec3,
    vaColor : vec4,
    vaNormal : vec3,
    vaUV0 : vec2,
    vaUV1 : vec2
}

struct Vertex { color: vec4, position: vec3, normal: vec3, tex_coords : vec2, lm_coords : vec2 }

out {
    vertex: Vertex
}

fn main () {
    let normal = gbufferModelViewInverse.mat3() * normalMatrix * vaNormal;
    vertex = Vertex(vec4(1.0, 1.0, 1.0, 1.0), vec3(1.0), normal, vec2(1.0), vec2(1.0));
}