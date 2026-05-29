using System;
using System.Collections.Generic;

namespace cast.core.registry
{
    public class InternalFunction
    {
        public string Name { get; set; } = string.Empty;
        public string ReturnType { get; set; } = string.Empty;
        public List<string> Parameters { get; set; } = new List<string>();
        
        public InternalFunction(string name, string returnType, List<string> parameters)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}