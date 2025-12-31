# CAST

Cast is designed to solve the common frustrations of shader programming. Writing shaders often involves wrestling with ambiguity in coordinate spaces and deciphering deeply nested math functions. Cast aims to provide a more secure and structured approach to shading.

## Key Features
- Explicit Coordinate Spaces: Never lose track of whether a vector is in Model, World, or Projection space again. Cast enforces clarity in space transformations.
- Linear Readable Syntax: Say goodbye to "inside-out" reading like max(pow(dot(...))). Cast allows you to write instructions from left-to-right, making the flow of data logical and easy to follow.
- Struct-Based OOP: Leverage a lightweight Object-Oriented approach with structs to keep your code modular and organized.


## Examples
### Coordinate Space Definitions
Cast already defines spaces for Model, World, View, Clip and Space.

```rust
declare space Model;

let a = vec3<World>(1.0);
let b = vec3<World>(1.0);
let c = a * b;  // this will work

let a = vec3<Model>(1.0);
let b = vec3<World>(1.0);
let c = a * b;  // unable to mix Model * World
```

```rust
let matrix: mat4<Model, World>;
let position = vec4<Model>(...);
let worldPos : vec4<World> = matrix * position; // converts Model to World Position
```


### Object Oriented Structs

```rust
struct SomeStruct { x: float, y: float }

fn (SomeStruct) swap() : SomeStruct {
  return SomeStruct(self.y, self.x);
}
```

The result in GLSL looks like this.

```glsl
struct SomeStruct {
	float x;
	float y;
};

SomeStruct swap(SomeStruct self) {
	return SomeStruct(self.y, self.x);
}
```

### Left to right Syntax

```rust
let a = vec3(1.0).pow(vec3(2.2)).max(1.0);
```

```glsl
vec3 a = max(pow(vec3(1.0), vec3(2.2)), 1.0);
```
