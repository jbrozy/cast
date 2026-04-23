using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cast.core.models
{
    public class FunctionSymbol : AbstractSymbol
    {
        // Todo: maybe scope inside here?
        private readonly List<AbstractSymbol> _parameters;
        private readonly CastType _returnType;
        
        public FunctionSymbol(string name, CastType returnType, List<AbstractSymbol>? parameters = null) : base(name)
        {
            _parameters = parameters ?? new List<AbstractSymbol>();
            _returnType = returnType;
        }

        public override string ToString()
        {
            StringBuilder functionSignature = new StringBuilder();
            string functionParameters = string.Join(", ", _parameters.Select(p => p.ToString())); 
            functionSignature.Append($"{_returnType} {Name}");
            functionSignature.Append($" ({functionParameters})");
            return functionSignature.ToString();
        }
    }
}