using System;

namespace cast.core.models
{
    public class TypeSymbol : AbstractSymbol
    {
        private int RequiredSpaces { get; }
        private bool OptionalSpaces { get; }
        
        public TypeSymbol(string name, int requiredSpaces, bool optionalSpaces = true) : base(name)
        {
            if (requiredSpaces > 2)
            {
                throw new Exception("Cannot create a Type with more than 2 spaces");
            }
            
            RequiredSpaces = requiredSpaces;
            OptionalSpaces = optionalSpaces;
        }

        public bool HasSpaces()
        {
            return RequiredSpaces > 0;
        }
    }
}