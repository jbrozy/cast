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
            RegisterFunction("*", "float", "float", "float");
            RegisterFunction("*", "int", "int", "int");
            RegisterFunction("*", "uint", "uint", "uint");

            RegisterFunction("*", "vec2<T>", "vec2<T>", "float");
            RegisterFunction("*", "vec3<T>", "vec3<T>", "float");
            RegisterFunction("*", "vec4<T>", "vec4<T>", "float");
            
            RegisterFunction("*", "vec2<T>", "float", "vec2<T>");
            RegisterFunction("*", "vec3<T>", "float", "vec3<T>");
            RegisterFunction("*", "vec4<T>", "float", "vec4<T>");

            RegisterFunction("*", "vec2<T>", "vec2<T>", "vec2<T>");
            RegisterFunction("*", "vec3<T>", "vec3<T>", "vec3<T>");
            RegisterFunction("*", "vec4<T>", "vec4<T>", "vec4<T>");

            RegisterFunction("*", "vec2<U>", "mat2<T, U>", "vec2<T>");
            RegisterFunction("*", "vec3<U>", "mat3<T, U>", "vec3<T>");
            RegisterFunction("*", "vec4<U>", "mat4<T, U>", "vec4<T>");

            RegisterFunction("+", "float", "float", "float");
            RegisterFunction("+", "int", "int", "int");

            RegisterFunction("+", "vec2<T>", "vec2<T>", "vec2<T>");
            RegisterFunction("+", "vec3<T>", "vec3<T>", "vec3<T>");
            RegisterFunction("+", "vec4<T>", "vec4<T>", "vec4<T>");

            RegisterFunction("+", "vec2<T>", "vec2<T>", "float");
            RegisterFunction("+", "vec3<T>", "vec3<T>", "float");
            RegisterFunction("+", "vec4<T>", "vec4<T>", "float");
            RegisterFunction("+", "vec2<T>", "float", "vec2<T>");
            RegisterFunction("+", "vec3<T>", "float", "vec3<T>");
            RegisterFunction("+", "vec4<T>", "float", "vec4<T>");

            RegisterFunction("-", "float", "float", "float");
            RegisterFunction("-", "int", "int", "int");

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
        }

        private static (string type, string[] genericParams) ParseType(string type)
        {
            if (!type.Contains("<")) return (type, new  string[] { });
            string[] lhs = type.Replace(" ", "").Replace(">", "").Split("<");
            string lhsType = lhs[0];
            string[] lhsSpaces = lhs[1].Split(",");
            
            return (lhsType, lhsSpaces);
        }
            
        private static CastType t(IToken token,Scope scope, ErrorLogger logger, string left, string right, string op)
        {
            // mat4<T, U> * vec4<T> -> vec4<U>
            (string lhsType, string[] lhsParams) = ParseType(left);
            (string rhsType, string[] rhsParams) = ParseType(right);

            var candidates = functions[op];

            Dictionary<string, string> s = new Dictionary<string, string>();
            
            foreach ((string[] parameters, string returnType) in candidates)
            {
                s.Clear();
                (string fnLhs, string[] fnLhsParams) = ParseType(parameters[0]);
                (string fnRhs, string[] fnRhsParams) = ParseType(parameters[1]);
                (string returnTypeType, string[] returnTypeParams) = ParseType(returnType);

                if (lhsType != fnLhs) continue;
                if (rhsType != fnRhs) continue;
                
                if (lhsParams.Length != fnLhsParams.Length) continue;
                if (rhsParams.Length != fnRhsParams.Length) continue;

                // add left spaces
                for (int i = 0; i < lhsParams.Length; ++i)
                {
                    s[fnLhsParams[i]] = lhsParams[i];
                }
                
                // validate right side based on left side
                bool valid = true;
                for (int i = 0; i < rhsParams.Length; i++)
                {
                    if (s[fnRhsParams[i]] != rhsParams[i]) valid = false;
                }

                for (int i = 0; i < returnTypeParams.Length; i++)
                {
                    if (!s.ContainsKey(returnTypeParams[i])) valid = false;
                }

                if (!valid) continue;
                TypeSymbol? type = scope[returnTypeType] as TypeSymbol;
                List<SpaceSymbol> typeSpaces = returnTypeParams.Select(c => scope[s[c]] as SpaceSymbol).ToList();
                return new CastType(type, typeSpaces);
            }

            string functionCandidates = string.Join("\n", candidates.Select(c => $"     ({string.Join(", ", c.Params)}) -> {c.returnType}"));
            string message = $"Unable to find function '{op}' with params ['{left}', '{right}'], \n candidates are: \n{functionCandidates}";
            logger.Log(token, message);
            
            return CastType.ErrorType;
        }

        public static CastType? Resolve(IToken token, Scope scope, ErrorLogger logger, string op, List<CastType> parameters)
        {
            // List<string> stringParams = parameters.Select(c=> c.ToString()).ToList();
            // List<(string[] Params, string returnType)> functionCandidates = functions[op];
            // 
            // CastType a = t(scope, "mat4<Model, View>", "vec4<Model>", op);

            // foreach ((string[] functionParameters, string functionReturnType) in functionCandidates)
            // {
            //     bool valid = true;
            //     if (functionParameters.Length != stringParams.Count) continue;
            //     
            //     for (int i = 0; i < functionParameters.Length; ++i)
            //     {
            //         Console.WriteLine($"functionParameters[i] != stringParams[i], {functionParameters[i]} != {stringParams[i]}");
            //         if (functionParameters[i] != stringParams[i])
            //         {
            //             valid = false;
            //         }
            //     }

            //     if (valid)
            //     {
            //         TypeSymbol? type = scope[functionReturnType] as TypeSymbol;
            //         return new CastType(type);
            //     }
            // }

            return t(token, scope, logger, parameters[0].ToString(), parameters[1].ToString(), op);
        }
    }
}