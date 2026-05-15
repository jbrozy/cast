using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using cast.core.logging;
using cast.core.models;
using cast.core.models.symbols;

namespace cast.core.registry
{
    public class Registry
    {
        private static Dictionary<string, List<(string[] Params, string returnType)>> functions = new Dictionary<string, List<(string[], string)>>();

        public static bool HasCandidates(string functionName)
        {
            return functions.ContainsKey(functionName);
        }
        
        private static void RegisterFunction(string name, string returnType, params string[] parameters)
        {
            if (!functions.ContainsKey(name))
            {
                functions[name] = new List<(string[] Params, string returnType)>();
            }
            
            functions[name].Add((parameters, returnType));
        }
        
        public static void Setup()
        {
            string[] vectors = { "vec2", "vec3", "vec4" };
            foreach(string vector in vectors)
            {
                RegisterFunction("length", "float", $"{vector}<T>");
                RegisterFunction("distance", "float", $"{vector}<T>",  $"{vector}<T>");
                RegisterFunction("dot", "float", $"{vector}<T>",  $"{vector}<T>");
                RegisterFunction("normalize", $"{vector}<T>", $"{vector}<T>");
            }
            
            RegisterFunction("cross", "vec3<T>", "vec3<T>", "vec3<T>");
            RegisterFunction("reflect", "vec3<T>", "vec3<T>", "vec3<T>");
            RegisterFunction("refract", "vec3<T>", "vec3<T>", "vec3<T>", "float");
            
            string[] floatTypes = { "float", "vec2<T>", "vec3<T>", "vec4<T>" };
            foreach (string t in floatTypes)
            {
                RegisterFunction("abs", t, t);
                RegisterFunction("sign", t, t);
                RegisterFunction("floor", t, t);
                RegisterFunction("ceil", t, t);
                RegisterFunction("round", t, t);
                RegisterFunction("fract", t, t);

                // min / max / mod (Between two identical types)
                RegisterFunction("min", t, t, t);
                RegisterFunction("max", t, t, t);
                RegisterFunction("mod", t, t, t);
                RegisterFunction("step", t, t, t);

                // clamp: clamp(x, minVal, maxVal)
                RegisterFunction("clamp", t, t, t, t);
        
                // mix: mix(x, y, a) -> Linear interpolation
                RegisterFunction("mix", t, t, t, t);
        
                // smoothstep: smoothstep(edge0, edge1, x)
                RegisterFunction("smoothstep", t, t, t, t); 
            }
            
            foreach (string v in vectors)
            {
                string vt = $"{v}<T>";
                RegisterFunction("min", vt, vt, "float");
                RegisterFunction("max", vt, vt, "float");
                RegisterFunction("mod", vt, vt, "float");
                RegisterFunction("mix", vt, vt, vt, "float");
                RegisterFunction("clamp", vt, vt, "float", "float");
                RegisterFunction("smoothstep", vt, "float", "float", vt);
            }
            
            foreach (string t in floatTypes)
            {
                // Exponential
                RegisterFunction("pow", t, t, t);
                RegisterFunction("exp", t, t);
                RegisterFunction("log", t, t);
                RegisterFunction("exp2", t, t);
                RegisterFunction("log2", t, t);
                RegisterFunction("sqrt", t, t);
                RegisterFunction("inversesqrt", t, t);

                // Trigonometry
                RegisterFunction("radians", t, t);
                RegisterFunction("degrees", t, t);
                RegisterFunction("sin", t, t);
                RegisterFunction("cos", t, t);
                RegisterFunction("tan", t, t);
                RegisterFunction("asin", t, t);
                RegisterFunction("acos", t, t);
                RegisterFunction("atan", t, t);     // atan(y_over_x)
                RegisterFunction("atan", t, t, t);  // atan(y, x)
            }
            
            RegisterFunction("inverse", "mat2<U, T>", "mat2<T, U>");
            RegisterFunction("inverse", "mat3<U, T>", "mat3<T, U>");
            RegisterFunction("inverse", "mat4<U, T>", "mat4<T, U>");
            
            RegisterFunction("transpose", "mat2<U, T>", "mat2<T, U>");
            RegisterFunction("transpose", "mat3<U, T>", "mat3<T, U>");
            RegisterFunction("transpose", "mat4<U, T>", "mat4<T, U>");
            
            RegisterFunction("determinant", "float", "mat2<T, U>");
            RegisterFunction("determinant", "float", "mat3<T, U>");
            RegisterFunction("determinant", "float", "mat4<T, U>");
            
            RegisterFunction("lessThan", "bvec2", "vec2<T>", "vec2<T>");
            RegisterFunction("lessThan", "bvec3", "vec3<T>", "vec3<T>");
            RegisterFunction("lessThan", "bvec4", "vec4<T>", "vec4<T>");
            
            RegisterFunction("lessThanEqual", "bvec2", "vec2<T>", "vec2<T>");
            RegisterFunction("lessThanEqual", "bvec3", "vec3<T>", "vec3<T>");
            RegisterFunction("lessThanEqual", "bvec4", "vec4<T>", "vec4<T>");

            RegisterFunction("greaterThan", "bvec2", "vec2<T>", "vec2<T>");
            RegisterFunction("greaterThan", "bvec3", "vec3<T>", "vec3<T>");
            RegisterFunction("greaterThan", "bvec4", "vec4<T>", "vec4<T>");

            RegisterFunction("greaterThanEqual", "bvec2", "vec2<T>", "vec2<T>");
            RegisterFunction("greaterThanEqual", "bvec3", "vec3<T>", "vec3<T>");
            RegisterFunction("greaterThanEqual", "bvec4", "vec4<T>", "vec4<T>");
            
            RegisterFunction("any", "bool", "bvec2");
            RegisterFunction("any", "bool", "bvec3");
            RegisterFunction("any", "bool", "bvec4");

            RegisterFunction("all", "bool", "bvec2");
            RegisterFunction("all", "bool", "bvec3");
            RegisterFunction("all", "bool", "bvec4");
            
            RegisterFunction("*", "float", "float", "float");
            RegisterFunction("*", "int", "int", "int");
            RegisterFunction("*", "uint", "uint", "uint");

            RegisterFunction("*", "vec2<T>", "vec2<T>", "float");
            RegisterFunction("*", "vec3", "vec3", "float");
            RegisterFunction("*", "vec3", "vec3", "vec3");
            RegisterFunction("*", "vec3<T>", "vec3<T>", "float");
            RegisterFunction("*", "vec4<T>", "vec4<T>", "float");
            RegisterFunction("*", "vec4<T>", "vec4<T>", "vec4");
            
            RegisterFunction("*", "vec2<T>", "float", "vec2<T>");
            RegisterFunction("*", "vec3<T>", "float", "vec3<T>");
            RegisterFunction("*", "vec4<T>", "float", "vec4<T>");

            RegisterFunction("*", "vec2<T>", "vec2<T>", "vec2<T>");
            RegisterFunction("*", "vec3<T>", "vec3<T>", "vec3<T>");
            RegisterFunction("*", "vec4<T>", "vec4<T>", "vec4<T>");

            RegisterFunction("*", "vec2<U>", "mat2<T, U>", "vec2<T>");
            RegisterFunction("*", "vec3<U>", "mat3<T, U>", "vec3<T>");
            RegisterFunction("*", "vec4<U>", "mat4<T, U>", "vec4<T>");
            RegisterFunction("/", "vec3", "vec3", "float");
            RegisterFunction("/", "vec3", "vec3", "vec3");
            RegisterFunction("-", "vec3", "vec3", "vec3");

            RegisterFunction("*", "mat4<T, V>", "mat4<U, V>", "mat4<T, U>");
            
            RegisterFunction("+", "float", "float", "float");
            RegisterFunction("+", "int", "int", "int");

            RegisterFunction("+", "vec2<T>", "vec2<T>", "vec2<T>");
            RegisterFunction("+", "vec3<T>", "vec3<T>", "vec3<T>");
            RegisterFunction("+", "vec4<T>", "vec4<T>", "vec4<T>");

            RegisterFunction("+", "vec2<T>", "vec2<T>", "float");
            RegisterFunction("+", "vec3<T>", "vec3<T>", "float");
            RegisterFunction("+", "vec3", "vec3", "vec3");
            RegisterFunction("+", "vec3<T>", "vec3<T>", "vec3");
            RegisterFunction("+", "vec4<T>", "vec4<T>", "float");
            RegisterFunction("+", "vec2<T>", "float", "vec2<T>");
            RegisterFunction("+", "vec3<T>", "float", "vec3<T>");
            RegisterFunction("+", "vec4<T>", "float", "vec4<T>");

            RegisterFunction("-", "float", "float", "float");
            RegisterFunction("-", "int", "int", "int");
            RegisterFunction("-", "vec3", "float", "vec3");
            RegisterFunction("-", "vec3<T>", "float", "vec3<T>");

            RegisterFunction("-", "vec2<T>", "vec2<T>", "vec2<T>");
            RegisterFunction("-", "vec3<T>", "vec3<T>", "vec3<T>");
            RegisterFunction("-", "vec4<T>", "vec4<T>", "vec4<T>");

            RegisterFunction("/", "float", "float", "float");
            RegisterFunction("/", "int", "int", "int");

            RegisterFunction("/", "vec2<T>", "vec2<T>", "float");
            RegisterFunction("/", "vec3<T>", "vec3<T>", "float");
            RegisterFunction("/", "vec4<T>", "vec4<T>", "float");

            RegisterFunction("/", "vec2<T>", "vec2<T>", "vec2<T>");
            RegisterFunction("/", "vec3<T>", "vec3<T>", "vec3<T>");
            RegisterFunction("/", "vec4<T>", "vec4<T>", "vec4<T>");

            RegisterFunction("%", "int", "int", "int");
            RegisterFunction("%", "uint", "uint", "uint");

            RegisterFunction("==", "bool", "float", "float");
            RegisterFunction("==", "bool", "int", "int");
            RegisterFunction("==", "bool", "bool", "bool");
            RegisterFunction("==", "bool", "vec2<T>", "vec2<T>");
            RegisterFunction("==", "bool", "vec3<T>", "vec3<T>");
            RegisterFunction("==", "bool", "vec4<T>", "vec4<T>");

            RegisterFunction("!=", "bool", "float", "float");
            RegisterFunction("!=", "bool", "int", "int");
            RegisterFunction("!=", "bool", "vec3<T>", "vec3<T>");

            RegisterFunction("<", "bool", "float", "float");
            RegisterFunction(">", "bool", "float", "float");
            RegisterFunction("<=", "bool", "float", "float");
            RegisterFunction(">=", "bool", "float", "float");
            RegisterFunction("<", "bool", "int", "int");
            RegisterFunction(">", "bool", "int", "int");

            RegisterFunction("&&", "bool", "bool", "bool");
            RegisterFunction("||", "bool", "bool", "bool");
            
            RegisterFunction("<<", "int", "int", "int");
            RegisterFunction(">>", "int", "int", "int");
            
            RegisterFunction("texture", "vec4", "sampler2D", "vec2");
            RegisterFunction("texture", "vec4", "sampler3D", "vec3");
            RegisterFunction("texture", "vec4", "samplerCube", "vec3");
            
            RegisterFunction("vec2", "vec2", "float");
            RegisterFunction("vec2", "vec2", "float", "float");
            RegisterFunction("vec2", "vec2", "vec3");
            RegisterFunction("vec2", "vec2", "vec4");
            RegisterFunction("vec3", "vec3", "float");
            RegisterFunction("vec3", "vec3", "float", "float", "float");
            RegisterFunction("vec3", "vec3", "vec2", "float");
            RegisterFunction("vec3", "vec3", "float", "vec2");
            RegisterFunction("vec3", "vec3", "vec4");
            RegisterFunction("vec4", "vec4", "float");
            
            RegisterFunction("vec3", "vec3", "float");
            
            RegisterFunction("vec4", "vec4<T>", "vec3<T>", "float");

            RegisterFunction("vec4", "vec4", "float", "float", "float", "float");

            RegisterFunction("vec4", "vec4", "vec3", "float");
            RegisterFunction("vec4", "vec4", "float", "vec3");

            RegisterFunction("vec4", "vec4", "vec2", "vec2");
            RegisterFunction("vec4", "vec4<T>", "vec3<T>", "float");

            RegisterFunction("vec4", "vec4", "vec2", "float", "float");
            RegisterFunction("vec4", "vec4", "float", "vec2", "float");
            RegisterFunction("vec4", "vec4", "float", "float", "vec2");
        }

        private static (string type, string[] genericParams) ParseType(string type)
        {
            if (!type.Contains("<")) return (type, new  string[] { });
            string[] lhs = type.Replace(" ", "").Replace(">", "").Split("<");
            string lhsType = lhs[0];
            string[] lhsSpaces = lhs[1].Split(",");
            
            return (lhsType, lhsSpaces);
        }

        public static CastType ResolveFunction(string name, List<CastType> parameters, ErrorLogger logger, Scope scope)
        {
            if (!functions.ContainsKey(name)) return CastType.ErrorType;
            List<(string[] Params, string returnType)> candidates = functions[name];

            Dictionary<string, string> genericParameters = new Dictionary<string, string>();

            bool valid = false;
            foreach ((string[] fnParams, string returnType) in candidates)
            {
                valid = fnParams.Length == parameters.Count;
                if (!valid) continue;

                for (int i = 0; i < fnParams.Length; ++i)
                {
                    (string expectedType, string[] leftGenerics) = ParseType(fnParams[i]);
                    (string givenType, string[] rightGenerics) = ParseType(parameters[i].ToString());

                    if (leftGenerics.Length != rightGenerics.Length)
                    {
                        valid = false;
                        continue;
                    }
                    for (int j = 0; j < leftGenerics.Length; j++)
                    {
                        if (!genericParameters.ContainsKey(leftGenerics[j])) genericParameters.Add(leftGenerics[j], rightGenerics[j]);
                    }

                    if (expectedType != givenType)
                    {
                        genericParameters.Clear();
                        valid = false;
                        continue;
                    }
                }

                if (genericParameters.Any() && valid)
                {
                    (string expected, string [] expectedParams) = ParseType(returnType);
                    List<SpaceSymbol> spaces = new  List<SpaceSymbol>();
                    for (int i = 0; i < expectedParams.Length; ++i)
                    {
                        spaces.Add(scope[genericParameters[expectedParams[i]]] as SpaceSymbol);
                    }
                    TypeSymbol type = scope[expected] as TypeSymbol;
                    CastType result = new CastType(type, spaces);
                    return result;
                }

                if (valid)
                {
                    TypeSymbol? returnTypeSymbol = scope[returnType] as TypeSymbol;
                    return new CastType(returnTypeSymbol);
                }
            }
            
            return CastType.ErrorType;
        }

        public static CastType? ResolveOperator(IToken token, Scope scope, ErrorLogger logger, string op, List<CastType> parameters)
        {
            if (!functions.ContainsKey(op)) return CastType.ErrorType;

            string left = parameters[0].ToString();
            string right = parameters[1].ToString();
            (string lhsType, string[] lhsParams) = ParseType(left);
            (string rhsType, string[] rhsParams) = ParseType(right);

            var candidates = functions[op];
            var s = new Dictionary<string, string>();

            foreach ((string[] fnParams, string returnType) in candidates)
            {
                s.Clear();
                (string fnLhs, string[] fnLhsParams) = ParseType(fnParams[0]);
                (string fnRhs, string[] fnRhsParams) = ParseType(fnParams[1]);
                (string returnTypeType, string[] returnTypeParams) = ParseType(returnType);

                if (lhsType != fnLhs || rhsType != fnRhs) continue;
                if (lhsParams.Length != fnLhsParams.Length || rhsParams.Length != fnRhsParams.Length) continue;

                for (int i = 0; i < lhsParams.Length; i++)
                    s[fnLhsParams[i]] = lhsParams[i];

                bool mismatch = false;
                for (int i = 0; i < rhsParams.Length; i++)
                {
                    if (s.TryGetValue(fnRhsParams[i], out var existing))
                    {
                        if (existing != rhsParams[i]) { mismatch = true; break; }
                    }
                    else
                    {
                        s[fnRhsParams[i]] = rhsParams[i];
                    }
                }
                if (mismatch) continue;

                var typeSpaces = new List<SpaceSymbol>();
                for (int i = 0; i < returnTypeParams.Length; i++)
                {
                    var symbol = scope[s[returnTypeParams[i]]] as SpaceSymbol;
                    if (symbol != null)
                        typeSpaces.Add(symbol);
                }

                var type = scope[returnTypeType] as TypeSymbol;
                return new CastType(type, typeSpaces);
            }

            return CastType.ErrorType;
        }
    }
}