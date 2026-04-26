using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                return Type.Name;
            
            // otherwise return base type and spaces
            string spaceNames = string.Join(", ", Spaces.Select(s => s.ToString()));
            return $"{Type.Name}<{spaceNames}>";
        }

        public override bool Equals(object obj)
        {
            if (obj is CastType objType)
            {
                string given = objType.ToString();
                string expected = this.ToString();
                
                // Todo: actual validation of parameters
                
                return given == expected;
            }
            
            return false;
        }
    }
}