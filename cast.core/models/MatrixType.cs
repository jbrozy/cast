using System.Collections.Generic;
using cast.core.models.symbols;

namespace cast.core.models
{
    public class MatrixType : CastType
    {
        public SpaceSymbol From => Spaces[0];
        public SpaceSymbol To => Spaces[1];
            
        public MatrixType(TypeSymbol name, List<SpaceSymbol>? spaces = null) : base(name, spaces)
        {
        }
    }
}