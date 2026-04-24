using System.Collections.Generic;

namespace cast.core.models
{
    public class StructSymbol : TypeSymbol
    {
        public Dictionary<string, TypeSymbol> Fields { get; } = new Dictionary<string, TypeSymbol>();

        public StructSymbol(string name) : base(name, 0, false)
        {
        }

        public void AddField(string name, TypeSymbol type)
        {
            if (Fields.ContainsKey(name))
            {
                // TODO: custom exception
                throw new System.ArgumentException($"Field with name {name} already exists");
            }
            
            Fields.Add(name, type);
        }
    }
}