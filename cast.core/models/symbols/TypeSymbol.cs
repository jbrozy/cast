using System;

namespace cast.core.models.symbols
{
    public class TypeSymbolBuilder
    {
        private string name;
        private int requiredSpaces;
        private bool optionalSpaces;
        
        public static TypeSymbolBuilder Builder()
        {
            return new TypeSymbolBuilder();
        }

        public TypeSymbolBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public TypeSymbolBuilder WithRequiredSpaces(int requiredSpaces)
        {
            this.requiredSpaces = requiredSpaces;
            return this;
        }

        public TypeSymbolBuilder WithOptionalSpaces(bool optionalSpaces)
        {
            this.optionalSpaces = optionalSpaces;
            return this;
        }

        public TypeSymbol Build()
        {
            return new TypeSymbol(name, requiredSpaces, optionalSpaces);
        }
    }
    
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

        public bool AreSpacesOptional()
        {
            return OptionalSpaces;
        }
        
        public bool HasSpaces()
        {
            return RequiredSpaces > 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}