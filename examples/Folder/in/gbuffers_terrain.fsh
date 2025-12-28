struct Vertex { color: vec4, position: vec3, normal: vec3, tex_coords : vec2, lm_coords : vec2 }
        
in {
    vertex: Vertex
}

fn (mat4<Model>) __mul__(rhs: vec4) : vec4<Model>;

fn main () {
}
