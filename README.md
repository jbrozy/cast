# Cast – Coordinate-Annotated Shader Transpiler

Cast extends GLSL with **coordinate-space type annotations** for shader programs. Vectors and matrices can be typed with semantic spaces (e.g., `vec3<World>`, `mat4<Object, World>`). The transpiler validates these annotations at compile time and outputs standard-compliant GLSL.

## Concept

Standard GLSL provides no way to express which coordinate space a variable belongs to:

```glsl
// Standard GLSL – no type safety between spaces
vec3 position = vec3(1.0, 2.0, 3.0);   // Which space?
vec3 normal = vec3(0.0, 1.0, 0.0);     // Object? World? View?
```

Cast allows explicit annotations:

```glsl
vec3<Object> a_Position;
vec3<World> v_Normal;
uniform mat4<Object, World> u_Model;
```

The compiler validates that only compatible spaces are connected:

```glsl
vec3<World> worldPos = u_Model * vec4<Object>(a_Position, 1.0);  // OK
vec3<Clip>  clipPos  = u_ViewProjection * worldPos;               // OK
```

## Architecture

The project consists of four modules:

| Module | Type | Purpose |
|--------|------|---------|
| `cast.core` | .NET class library | Core compiler: lexer, parser, type checker, GLSL code generator |
| `cast.cli` | Console app | CLI tool (`build`, `compose`) |
| `cast.api.core` | .NET class library | Service layer for the web API |
| `cast.api` | ASP.NET web app | REST API (`/api/compile`, `/api/graph`) + web editor |

### Compiler Pipeline

1. **Lexer** (ANTLR) – Tokenizes Cast/GLSL source with preprocessor mode switching
2. **Preprocessor** – Handles `#version`, `#define` directives
3. **Parser** (ANTLR) – Builds an AST from a full GLSL 4.6 grammar
4. **Declaration Pass** – Registers types, functions, variables in the symbol table
5. **Semantic Pass** – Type- and space-checking with generic overload resolution
6. **Codegen Pass** – Outputs standard GLSL (strips space annotations)

The type system supports parameterized spaces (`vec3<T>`, `mat4<T,U>`) resolved through a built-in registry module with ~250+ signatures for built-in functions and operators.

### Multi-Pass Pipelines

Beyond single shaders, Cast can compose entire rendering pipelines from multiple passes:

```json
{
  "stages": ["gbuffer", "lighting", "postprocess"],
  "connections": {
    "gbuffer.gbuf_albedo": "lighting.gbuf_albedo",
    "gbuffer.gbuf_normal": "lighting.gbuf_normal"
  }
}
```

Input/output variables are automatically wired and checked for type compatibility. The result is displayed as a visual graph (Mermaid).

## Quickstart

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- ANTLR (generated automatically via build target)

### Build

```bash
dotnet build
```

### CLI

```bash
# Transpile a single file
dotnet run --project cast.cli -- build input.cast

# Compose a pipeline from a manifest
dotnet run --project cast.cli -- compose pipeline.json --output-dir out
```

### Web API

```bash
dotnet run --project cast.api
```

Open `http://localhost:5264` in your browser. The built-in editor loads shader files, transpiles on every keystroke, and visualizes pipeline graphs.

### Docker

```bash
docker compose up
```

## Technologies

- **C# / .NET 10.0** – Language and runtime
- **ANTLR 4** – Parser generator (grammar in `grammar/`)
- **ASP.NET Core** – REST API + static files
- **Mermaid.js** – Pipeline graph visualization
- **highlight.js** – Syntax highlighting
- **Spectre.Console** – CLI framework

## Project Structure

```
cast/
├── cast.core/           # Core compiler
│   ├── models/          # Types, symbols, scopes
│   ├── parser/          # GlslParser, GlslShaderProgram
│   ├── visitor/         # Three compiler passes
│   └── registry/        # Built-in functions/operators
├── cast.cli/            # Command-line tool
├── cast.api/            # Web API + frontend
│   └── www/             # Single-page editor (HTML/CSS/JS)
├── cast.api.core/       # API service layer
├── grammar/             # ANTLR grammar
│   ├── CastLexer.g4
│   ├── CastParser.g4
│   └── CastPreParser.g4
├── examples/            # Example shaders
└── test_programs/       # Manual test inputs
```

## Status

Cast is a **prototype** developed as part of a bachelor's thesis. The core concepts are implemented, but many compiler passes are still incomplete:

- [x] ANTLR grammar (full GLSL 4.6)
- [x] Space-annotated type system
- [x] Generic overload resolution
- [x] Three-stage compiler pipeline
- [x] Multi-pass pipeline composition
- [x] Web editor with live transpilation
- [ ] Loops (`for`, `while`, `do`)
- [ ] Switch statements
- [ ] Full array support
- [ ] Semantic layout qualifier validation
- [ ] Automated tests
