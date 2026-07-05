using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Atn;
using cast.core.models.symbols;

namespace cast.core.models
{
    public class CastType
    {
        public static readonly CastType ErrorType = new CastType(new TypeSymbol("ERROR_TYPE", 0, false));
        
        public TypeSymbol Type { get; set; }
        public List<SpaceSymbol> Spaces { get; }

        public bool IsReturn { get; set; } = false;

        public CastType(TypeSymbol type, List<SpaceSymbol>? spaces = null)
        {
            Type = type;
            Spaces = spaces ?? new List<SpaceSymbol>();
        }

        public override string ToString()
        {
            // return base type if no spaces are set
            if (Spaces.Count == 0)
                return Type?.Name ?? "ErrorType";
            
            // otherwise return base type and spaces
            string spaceNames = string.Join(", ", Spaces?.Select(s => s.ToString()));
            return $"{Type.Name}<{spaceNames}>";
        }

        public override bool Equals(object obj)
        {
            if (obj is CastType other)
            {
                if (this.Type?.Name != other.Type?.Name) return false;
                if (this.Spaces.Count != other.Spaces.Count) return false;

                for (int i = 0; i < this.Spaces.Count; i++)
                {
                    if (this.Spaces[i].Name != other.Spaces[i].Name) return false;
                }
                return true;
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            int hash = Type.Name.GetHashCode();
            foreach (var space in Spaces)
                hash = unchecked(hash * 23 + space.GetHashCode());
            return hash;
        }

        public bool IsAssignable(CastType rhs)
        {
            // 1. Der Basis-Typ muss IMMER stimmen (vec4 kann nicht vec3 werden)
            if (this.Type?.Name != rhs.Type?.Name) return false;

            // vec4<World> = vec4(...) -- upcast allowed
            // vec4 = vec4<World>(...) -- downcast is allowed
            if (this.Spaces.Count == 0 || rhs.Spaces.Count == 0) 
            {
                return true;
            }

            if (this.Spaces.Count != rhs.Spaces.Count) return false;
            for (int i = 0; i < this.Spaces.Count; i++)
            {
                if (this.Spaces[i].Name != rhs.Spaces[i].Name)
                {
                    return false;
                }
            }

            return true;
        }
    }
}