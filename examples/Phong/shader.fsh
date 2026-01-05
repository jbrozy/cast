uniform { colortex0 : sampler2D, colortex1 : sampler2D }

in {
    fsUV0 : vec2,
    fsUV2 : vec2,
    fsColor : vec4,
    fsNormal : vec3<View>,
    fsPosition : vec3<View>,
    fsLMCoords : vec2
}

out {
    color : @loc(0) vec4
}

fn main() {
    let albedo : vec4 = colortex0.texture(fsUV0);
    if (albedo.a < 0.5) {
        return;
    }
	color = albedo * fsColor;
}