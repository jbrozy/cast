# CAST

# Space Type System
```rust
fn main() {
	let a = vec3<Model>(1.0); // Defined in Model-Space
	let b = vec3<Model>(1.0); // Defined in Model-Space
	let c = a * b + a; // THIS WORKS
}
```

```rust
fn main() {
	let a = vec3<Model>(1.0); // Defined in Model-Space
	let b = vec3<World>(1.0); // Defined in World-Space
	let c = a * b + a; // THIS DOESN'T WORK
}
```

## 🦍 Object-Oriented Design

Cast brings structure to shader programming. You can define custom data types (`structs`), initialize them with **constructors**, and attach behavior using **Go-like receiver syntax**.

### Structs, Constructors & Methods

Instead of loose variables, bundle your data into structs and manipulate them with methods.

```rust
// 1. Define a Struct
struct Light { 
    position: vec3, 
    color: vec3, 
    intensity: float 
}

// 2. Attach a Method (Receiver Syntax)
// The syntax 'fn (Type)' defines which struct this method belongs to.
fn (Light) calculateRadiance(dist: float) : vec3 {
    let attenuation = 1.0 / (dist * dist);
    // Access struct fields using 'self'
    return self.color * self.intensity * attenuation;
}

fn main() {
    let myLight = Light(vec3(1.0), vec3(1.0), 1.0);
    
    let dist = 5.0;
    let radiance = myLight.calculateRadiance(dist);
}
```

Converting this to GLSL gives:

```glsl
struct Light {
    vec3 position;
    vec3 color;
    float intensity;
};

vec3 calculateRadiance(Light self, float dist) {
    float attenuation = 1.0 / dist * dist;
    return self.color * self.intensity * attenuation;
}

void main() {
    Light myLight = Light(vec3(1.0), vec3(1.0), 1.0);
    float dist = 5.0;
    vec3 radiance = calculateRadiance(myLight, dist);
}
```

