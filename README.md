# CAST

Cast is a strongly-typed, modular shading language designed as a modern abstraction over GLSL (OpenGL Shading Language). It focuses on type safety and structural clarity for graphics programming.

## Key characteristics include:
Coordinate Space Safety: Unlike standard GLSL, Cast enforces coordinate systems via the type system. You can define specific spaces (e.g., declare space World;) and tag vectors with them (e.g., vec4<Clip>, vec4<Screen>). This prevents logical errors, such as accidentally mixing World-space and Screen-space coordinates.

Modern Syntax: It utilizes a syntax similar to Rust or TypeScript, using keywords like fn for functions , let for variable binding (implied by the grammar's assignment rules), and struct for data structures.

GLSL Interop: It mirrors standard GLSL functionality, providing built-in support for vector mathematics (dot products, cross products) , texture sampling (texture, textureLod) , and math libraries (sin, pow, mix).

Operator Overloading: The language defines core operators (like `+`, `*`) as functions (`__add__`, `__mul__`), allowing for explicit definition of vector and scalar interactions.

✅ Implemented Features (Current State)

🏗 Core Syntax & Structure

- [x] External Declarations: Support for the declare keyword to define external symbols.
- [x] Struct Definitions: C-style struct definitions (e.g., vec2, mat4, sampler2D).
- [x] Function Signatures: Typed function declarations using fn (returnType) name(args).
- [x] Function Implementation: Function bodies with { ... }, return statements, and let assignments.
- [x] Extension Methods: Method syntax support via self access within functions (e.g., self.x, self.dot(...)).

📐 Type System & Math
- [x] Primitive Types: Core support for int, float, and bool.
- [ ] Define Types inside the language itself
- [x] Vector & Matrix Types: Full support for vec2 through vec4, ivec, and mat4.
- [x] Constructors: Overloaded constructors (e.g., vec3(vec2, float) or vec3(float, float, float)).
- [x] Operator Overloading: Mapping standard operators to internal functions (__add__, __mul__, __div__, etc.) .
- [x] Math Library: Extensive math function support (sin, cos, pow, mix, smoothstep, etc.) for scalars and vectors .

🌌 Coordinate Spaces (Unique Feature)

- [x] Space Declaration: Definition of specific coordinate spaces (Model, World, View, Clip, Screen).
- [x] Space Typing: Generic-style space syntax for vectors (e.g., vec4<Clip>, vec4<Screen>).

🎨 Graphics & Textures

- [ ] Shader Inputs/Outputs: in and out variable declarations (e.g., vertex_index, pixel_depth).
- [x] Texture Samplers: Opaque definitions for sampler2D, sampler3D, samplerCube, sampler2DShadow, etc..
- [x] Texture Lookup: Standard functions for texture sampling (texture, textureLod, fetch, textureProj) .

🧠 Interop & Built-ins
- [ ] GLSL Mapping: Direct mapping to GLSL built-in variables (e.g., mapping vertex_index to gl_VertexID).

📝 Next Steps (Roadmap)
- [ ] Control Flow: Implement complex logic within the standard library (currently mostly linear return statements).
- [ ] Array Syntax: Usage of arrays in the standard library (not yet visible in current .cst uploads, but planned in grammar).
- [ ] Type Swizzling with Annoations `@swizzle`

## Space Type System
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

