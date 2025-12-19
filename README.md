# CAST

![Build Status](https://img.shields.io/badge/build-passing-brightgreen) ![License](https://img.shields.io/badge/license-MIT-blue)

**Cross-API Shader Transpiler**

> A high-level DSL for writing shader code that can be emitted as GLSL, HLSL or MSL.

---

## 🎓 Bachelor Thesis

This repository contains the implementation of my Bachelor Thesis in Computer Science at University of Applied Sciences Kiel.  

---

## 📋 Table of Contents

- [Disclaimer](#disclaimer)
- [Features](#features)
- [Methodology](#methodology)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

---

## ⚠️ Disclaimer

CAST is an experimental DSL whose goal is to provide a unified, high-level language for shader development.  
- **Phase 1:** GLSL output
- **Phase 2:** HLSL output
- **Phase 3:** MSL output

---

## 🚀 Features

- Define shaders in a concise, expressive syntax
- Auto-generate production-ready GLSL (OpenGL/Vulkan)
- Extendable backends for HLSL (DirectX) and MSL (Metal)
- Modular architecture for adding new target languages

---

## 🛠 Methodology

1. **Grammar Definition**
   - All syntax is specified using ANTLR4 grammar files.
2. **Test-Driven Development**
   - Parser and AST generation are covered by unit tests.
   - Code generation outputs are validated against reference shaders.
3. **Incremental Backends**
   - Start with GLSL, then add HLSL, and finally MSL.

---

## 🏁 Getting Started

TBD

