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
        private static HashSet<InternalFunction> _internalFunctions = new HashSet<InternalFunction>();
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
            _internalFunctions.Add(new InternalFunction(name, returnType, parameters.ToList()));
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

            string[] plainVectors = { "vec2", "vec3", "vec4" };
            foreach(string vector in plainVectors)
            {
                RegisterFunction("dot", "float", vector, vector);
                RegisterFunction("normalize", vector, vector);
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
            RegisterFunction("*", "vec2", "vec2", "float");
            RegisterFunction("*", "vec4", "vec4", "float");
            RegisterFunction("*", "vec2", "vec2", "vec2");
            RegisterFunction("*", "vec4", "vec4", "vec4");
            
            RegisterFunction("*", "vec2<T>", "float", "vec2<T>");
            RegisterFunction("*", "vec3<T>", "float", "vec3<T>");
            RegisterFunction("*", "vec4<T>", "float", "vec4<T>");
            RegisterFunction("*", "vec2", "float", "vec2");
            RegisterFunction("*", "vec3", "float", "vec3");
            RegisterFunction("*", "vec4", "float", "vec4");

            RegisterFunction("*", "vec2<T>", "vec2<T>", "vec2<T>");
            RegisterFunction("*", "vec3<T>", "vec3<T>", "vec3<T>");
            RegisterFunction("*", "vec4<T>", "vec4<T>", "vec4<T>");

            RegisterFunction("*", "vec2<U>", "mat2<T, U>", "vec2<T>");
            RegisterFunction("*", "vec3<U>", "mat3<T, U>", "vec3<T>");
            RegisterFunction("*", "vec4<U>", "mat4<T, U>", "vec4<T>");
            RegisterFunction("*", "vec4", "mat4", "vec4");
            RegisterFunction("*", "vec2", "mat2", "vec2");
            RegisterFunction("*", "vec3", "mat3", "vec3");

            RegisterFunction("/", "vec2", "vec2", "float");
            RegisterFunction("/", "vec3", "vec3", "float");
            RegisterFunction("/", "vec4", "vec4", "float");
            RegisterFunction("/", "vec2", "vec2", "vec2");
            RegisterFunction("/", "vec3", "vec3", "vec3");
            RegisterFunction("/", "vec4", "vec4", "vec4");

            RegisterFunction("-", "vec2", "vec2", "vec2");
            RegisterFunction("-", "vec3", "vec3", "vec3");
            RegisterFunction("-", "vec4", "vec4", "vec4");
            RegisterFunction("-", "vec2", "vec2", "float");
            RegisterFunction("-", "vec3", "vec3", "float");
            RegisterFunction("-", "vec4", "vec4", "float");

            RegisterFunction("*", "mat4<T, V>", "mat4<U, V>", "mat4<T, U>");
            
            RegisterFunction("+", "float", "float", "float");
            RegisterFunction("+", "int", "int", "int");

            RegisterFunction("+", "vec2<T>", "vec2<T>", "vec2<T>");
            RegisterFunction("+", "vec3<T>", "vec3<T>", "vec3<T>");
            RegisterFunction("+", "vec4<T>", "vec4<T>", "vec4<T>");

            RegisterFunction("+", "vec2<T>", "vec2<T>", "float");
            RegisterFunction("+", "vec3<T>", "vec3<T>", "float");
            RegisterFunction("+", "vec3", "vec3", "vec3");
            RegisterFunction("+", "vec2", "vec2", "vec2");
            RegisterFunction("+", "vec4", "vec4", "vec4");
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
            RegisterFunction("==", "bool", "uint", "uint");
            RegisterFunction("==", "bool", "bool", "bool");
            RegisterFunction("==", "bool", "vec2<T>", "vec2<T>");
            RegisterFunction("==", "bool", "vec3<T>", "vec3<T>");
            RegisterFunction("==", "bool", "vec4<T>", "vec4<T>");

            RegisterFunction("!=", "bool", "float", "float");
            RegisterFunction("!=", "bool", "int", "int");
            RegisterFunction("!=", "bool", "uint", "uint");
            RegisterFunction("!=", "bool", "bool", "bool");
            RegisterFunction("!=", "bool", "vec2<T>", "vec2<T>");
            RegisterFunction("!=", "bool", "vec3<T>", "vec3<T>");
            RegisterFunction("!=", "bool", "vec4<T>", "vec4<T>");

            RegisterFunction("<", "bool", "float", "float");
            RegisterFunction(">", "bool", "float", "float");
            RegisterFunction("<=", "bool", "float", "float");
            RegisterFunction(">=", "bool", "float", "float");
            RegisterFunction("<", "bool", "int", "int");
            RegisterFunction(">", "bool", "int", "int");
            RegisterFunction("<=", "bool", "int", "int");
            RegisterFunction(">=", "bool", "int", "int");
            RegisterFunction("<", "bool", "uint", "uint");
            RegisterFunction(">", "bool", "uint", "uint");
            RegisterFunction("<=", "bool", "uint", "uint");
            RegisterFunction(">=", "bool", "uint", "uint");

            RegisterFunction("&&", "bool", "bool", "bool");
            RegisterFunction("||", "bool", "bool", "bool");
            
            RegisterFunction("<<", "int", "int", "int");
            RegisterFunction(">>", "int", "int", "int");

            RegisterFunction("!", "bool", "bool");
            RegisterFunction("-", "int", "int");
            RegisterFunction("-", "float", "float");
            RegisterFunction("-", "vec2", "vec2");
            RegisterFunction("-", "vec3", "vec3");
            RegisterFunction("-", "vec4", "vec4");
            RegisterFunction("-", "mat2", "mat2");
            RegisterFunction("-", "mat3", "mat3");
            RegisterFunction("-", "mat4", "mat4");
            RegisterFunction("+", "int", "int");
            RegisterFunction("+", "float", "float");
            RegisterFunction("+", "vec2", "vec2");
            RegisterFunction("+", "vec3", "vec3");
            RegisterFunction("+", "vec4", "vec4");
            RegisterFunction("+", "mat2", "mat2");
            RegisterFunction("+", "mat3", "mat3");
            RegisterFunction("+", "mat4", "mat4");
            
            RegisterFunction("texture", "T", "sampler2D<T>", "vec2");
            RegisterFunction("texture", "vec4", "sampler2D", "vec2");
            RegisterFunction("texture", "T", "sampler3D<T>", "vec3");
            RegisterFunction("texture", "vec4", "sampler3D", "vec3");
            RegisterFunction("texture", "T", "samplerCube<T>", "vec3");
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

            RegisterFunction("mat3", "mat3", "mat4");
            RegisterFunction("mat3", "mat3<T, U>", "mat4<T, U>");
            RegisterFunction("mat3", "mat3<T>", "vec3<T>");
            RegisterFunction("mat3", "mat3", "vec3");
            RegisterFunction("mat3", "mat3", "float");
            RegisterFunction("mat3", "mat3", "vec3", "vec3", "vec3");
        }

        private static (string type, string[] genericParams) ParseType(string type)
        {
            if (!type.Contains("<")) return (type, new string[] { });

            int angleStart = type.IndexOf('<');
            string lhsType = type.Substring(0, angleStart).Trim();
            string inner = type.Substring(angleStart + 1, type.Length - angleStart - 2);

            var parts = new List<string>();
            int depth = 0;
            int partStart = 0;
            for (int i = 0; i < inner.Length; i++)
            {
                if (inner[i] == '<') depth++;
                else if (inner[i] == '>') depth--;
                else if (inner[i] == ',' && depth == 0)
                {
                    parts.Add(inner.Substring(partStart, i - partStart).Trim());
                    partStart = i + 1;
                }
            }
            parts.Add(inner.Substring(partStart).Trim());

            return (lhsType, parts.ToArray());
        }

        public static CastType ResolveFunction(string name, List<CastType> parameters, ErrorLogger logger, Scope scope, IToken token)
        {
            if (!functions.ContainsKey(name))
            {
                logger.Log(token, $"Function '{token.Text}' not found.");
                return CastType.ErrorType;
            }

            // we have to prefer generic functions before non generics
            IEnumerable<InternalFunction> candidates = _internalFunctions
                .Where(f => f.Name == name)
                .OrderByDescending(f => f.Parameters.Count);
            
            foreach (InternalFunction internalFunction in candidates)
            {
                // this will give us things like vec4<T>, mat4<T, U> the generic parameters
                var internalParameters = internalFunction.Parameters;
                bool valid = true;
                for (int i = 0; i < internalParameters.Count; i++)
                {
                    var internalParam = internalParameters[i];
                    (string type, string[] genericParams) = ParseType(internalParam);
                    
                    // if the base type is unequal to the given type or the amount of spaces is incorrect,
                    // break the inner loop and proceed with other candidates
                    if (i >= parameters.Count)
                    {
                        valid = false;
                        break;
                    }
                    
                    if (type != parameters[i].Type.Name || genericParams.Count() != parameters[i].Spaces.Count)
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid) continue;
                // all parameters should have matched here
                // add the generic parameters to a dictionary
                // so we can keep track of them
                Dictionary<string, string> generics = new Dictionary<string, string>();
                foreach (var internalParam in internalFunction.Parameters)
                {
                    // parse the parameter since its form is f. e. vec4<T>
                    (_, string[] genericParams) = ParseType(internalParam);
                    foreach (var genericParam in genericParams)
                    {
                        // we do not have an actual value for the generic yet
                        // its possible that one generic param has already been added
                        generics.TryAdd(genericParam, string.Empty);
                    }
                }

                for (int i = 0; i < parameters.Count; i++)
                {
                    CastType externalParam = parameters[i];
                    string sig = externalParam.ToString();
                    (_,  string[] genericParams) = ParseType(sig);
                    
                    for (int j = 0; j < genericParams.Length; j++)
                    {
                        for(int k = 0; k < generics.Count; k++)
                        {
                            string key = generics.ElementAt(k).Key;
                            if (generics[key] == string.Empty)
                            {
                                generics[key] = genericParams[k];
                            }
                            else if (generics[key] != genericParams[k])
                            {
                                logger.Log(token, $"Expected: {key} == {genericParams[j]}");
                                return  CastType.ErrorType;
                            }
                        }
                    }
                }

                Console.WriteLine("");
                // check if the index of the parameters from the keys of the elements in the generics-Dictionary match
                // this will be from left to right, if its vec4<World> * vec4<Model>
                // the first occurrence of vec4<World> will result in T being World
                // this means a variant of the function with matching generics has been validated
                (string expectedReturnType, _) = ParseType(internalFunction.ReturnType);
                TypeSymbol returnType = scope[expectedReturnType] as TypeSymbol;
                if (generics.Any() && generics.All(c => c.Value != string.Empty))
                {
                    List<SpaceSymbol> spaces = new List<SpaceSymbol>();
                    if (returnType.HasSpaces())
                    {
                        foreach ((_, string space) in generics)
                        {
                            SpaceSymbol s = scope[space] as SpaceSymbol;
                            spaces.Add(s);
                        }
                    }
                    return new CastType(returnType, spaces);
                }
                return new CastType(returnType);
            }
            
            return CastType.ErrorType;
            // if (!functions.ContainsKey(name)) return CastType.ErrorType;
            // List<(string[] Params, string returnType)> candidates = functions[name];

            // foreach ((string[] fnParams, string returnType) in candidates)
            // {
            //     if (fnParams.Length != parameters.Count) continue;

            //     Dictionary<string, string> genericParameters = new Dictionary<string, string>();
            //     bool valid = true;

            //     for (int i = 0; i < fnParams.Length; ++i)
            //     {
            //         (string expectedType, string[] leftGenerics) = ParseType(fnParams[i]);
            //         (string givenType, string[] rightGenerics) = ParseType(parameters[i].ToString());

            //         if (expectedType != givenType)
            //         {
            //             valid = false;
            //             break;
            //         }

            //         for (int j = 0; j < leftGenerics.Length && j < rightGenerics.Length; j++)
            //         {
            //             if (genericParameters.TryGetValue(leftGenerics[j], out var existing))
            //             {
            //                 if (existing != rightGenerics[j])
            //                 {
            //                     valid = false;
            //                     break;
            //                 }
            //             }
            //             else
            //             {
            //                 genericParameters.Add(leftGenerics[j], rightGenerics[j]);
            //             }
            //         }
            //         if (!valid) break;
            //     }

            //     if (!valid) continue;

            //     if (genericParameters.Any())
            //     {
            //         string resolved = genericParameters.ContainsKey(returnType) ? genericParameters[returnType] : returnType;
            //         (string expected, string [] expectedParams) = ParseType(resolved);
            //         List<SpaceSymbol> spaces = new  List<SpaceSymbol>();
            //         for (int i = 0; i < expectedParams.Length; ++i)
            //         {
            //             if (genericParameters.TryGetValue(expectedParams[i], out var resolvedParam))
            //                 spaces.Add(scope[resolvedParam] as SpaceSymbol);
            //             else
            //                 spaces.Add(scope[expectedParams[i]] as SpaceSymbol);
            //         }
            //         TypeSymbol type = scope[expected] as TypeSymbol;
            //         if (type == null) return CastType.ErrorType;
            //         return new CastType(type, spaces);
            //     }

            //     TypeSymbol? returnTypeSymbol = scope[returnType] as TypeSymbol;
            //     if (returnTypeSymbol == null) continue;
            //     return new CastType(returnTypeSymbol);
            // }

            // return CastType.ErrorType;
        }

        public static CastType? ResolveOperator(IToken token, Scope scope, ErrorLogger logger, string op, List<CastType> parameters)
        {
            if (!functions.ContainsKey(op))
            {
                logger.Log(token, $"Operator '{op}' is not defined.");
                return CastType.ErrorType;
            }

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
                (string fnRhs, string[] fnRhsParams) = (string.Empty, null);
                (string returnTypeType, string[] returnTypeParams) = ParseType(returnType);
                if (fnParams.Length == 1)
                {
                    (fnRhs, fnRhsParams) = (fnLhs, fnLhsParams);
                }

                if (fnParams.Length == 2)
                {
                    (fnRhs, fnRhsParams) = ParseType(fnParams[1]);
                }

                if (lhsType != fnLhs || rhsType != fnRhs) continue;

                for (int i = 0; i < lhsParams.Length && i < fnLhsParams.Length; i++)
                    s[fnLhsParams[i]] = lhsParams[i];

                bool mismatch = false;
                for (int i = 0; i < rhsParams.Length && i < fnRhsParams.Length; i++)
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

                bool allResolved = true;
                for (int i = 0; i < returnTypeParams.Length; i++)
                {
                    if (!s.ContainsKey(returnTypeParams[i])) { allResolved = false; break; }
                }
                if (!allResolved) continue;

                var typeSpaces = new List<SpaceSymbol>();
                for (int i = 0; i < returnTypeParams.Length; i++)
                {
                    var symbol = scope[s[returnTypeParams[i]]] as SpaceSymbol;
                    if (symbol != null)
                        typeSpaces.Add(symbol);
                }

                var type = scope[returnTypeType] as TypeSymbol;
                if (type == null) continue;
                return new CastType(type, typeSpaces);
            }

            logger.Log(token, $"No matching overload for operator '{op}' with types '{parameters[0]}' and '{parameters[1]}'.");
            return CastType.ErrorType;
        }

        public static CastType ResolveUnaryOperator(Scope scope, string op, CastType operand)
        {
            if (!functions.ContainsKey(op)) return CastType.ErrorType;

            string operandStr = operand.ToString();
            (string operandType, string[] operandParams) = ParseType(operandStr);

            var candidates = functions[op];
            foreach ((string[] fnParams, string returnType) in candidates)
            {
                if (fnParams.Length != 1) continue;
                (string fnParam, string[] fnParamGenerics) = ParseType(fnParams[0]);

                if (operandType != fnParam) continue;
                if (operandParams.Length != fnParamGenerics.Length) continue;

                var genericMap = new Dictionary<string, string>();
                for (int i = 0; i < operandParams.Length; i++)
                    genericMap[fnParamGenerics[i]] = operandParams[i];

                (string retType, string[] retGenerics) = ParseType(returnType);
                var spaces = new List<SpaceSymbol>();
                for (int i = 0; i < retGenerics.Length; i++)
                {
                    if (genericMap.TryGetValue(retGenerics[i], out var mapped))
                    {
                        var space = scope[mapped] as SpaceSymbol;
                        if (space != null) spaces.Add(space);
                    }
                }

                var typeSym = scope[retType] as TypeSymbol;
                if (typeSym == null) continue;
                return new CastType(typeSym, spaces);
            }

            return CastType.ErrorType;
        }
    }
}