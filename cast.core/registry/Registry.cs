using System;
using System.Collections.Generic;
using System.Linq;
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
            RegisterFunction("*", "vec4<U>", "mat4<T, U>", "vec4<T>");
            RegisterFunction("*", "float", "float", "float");
            RegisterFunction("+", "int", "int", "int");
            RegisterFunction("+", "float", "float", "int");
        }

        private static (string type, string[] genericParams) ParseType(string type)
        {
            string[] lhs = type.Replace(" ", "").Replace(">", "").Split("<");
            string lhsType = lhs[0];
            string[] lhsSpaces = lhs[1].Split(",");
            
            return (lhsType, lhsSpaces);
        }
            
        private static CastType t(Scope scope, string left, string right, string op)
        {
            // mat4<T, U> * vec4<T> -> vec4<U>
            (string lhsType, string[] lhsParams) = ParseType(left);
            (string rhsType, string[] rhsParams) = ParseType(right);

            var candidates = functions[op];

            Dictionary<string, string> s = new Dictionary<string, string>();
            
            foreach ((string[] parameters, string returnType) in candidates)
            {
                (string fnLhs, string[] fnLhsParams) = ParseType(parameters[0]);
                (string fnRhs, string[] fnRhsParams) = ParseType(parameters[1]);
                (string returnTypeType, string[] returnTypeParams) = ParseType(returnType);

                if (lhsType != fnLhs) continue;
                if (rhsType != fnRhs) continue;

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
                
                if (valid)
                {
                    TypeSymbol? type = scope[returnTypeType] as TypeSymbol;
                    var typeSpaces = returnTypeParams.Select(c => scope[s[c]] as SpaceSymbol).ToList();
                    return new CastType(type, typeSpaces);
                }
                
                s.Clear();
            }

            return null;
        }

        public static CastType? Resolve(Scope scope, string op, List<CastType> parameters)
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

            return t(scope, parameters[0].ToString(), parameters[1].ToString(), op);
        }
    }
}