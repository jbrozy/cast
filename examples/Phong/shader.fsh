uniform {
    modelViewMatrix : mat4<Model, View>,
    projectionMatrix : mat4<View, Clip>,
    normalMatrix : mat3<Model, View>,
    gbufferModelViewInverse : mat4<View, Model>,
    chunkOffset : vec3<Model>
}

out { 
    outNormals: @loc(0) vec3, 
    fragPosition: @loc(1) vec3, 
    uv: @loc(2) vec2 
}

fn main() {
}
