using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cast.core.models.symbols
{
    public class FunctionSymbol : AbstractSymbol
    {
        // Todo: maybe scope inside here?
        private readonly List<AbstractSymbol> _parameters;
        private readonly CastType _returnType;
        
        private Scope _scope { get; set; }
        
        public FunctionSymbol(string name, CastType returnType, List<AbstractSymbol>? parameters = null) : base(name)
        {
            _parameters = parameters ?? new List<AbstractSymbol>();
            _returnType = returnType;
        }

        public void SetScope(Scope scope)
        {
            _scope = scope;
        }

        public Scope GetFunctionScope()
        {
            return _scope;
        }

        public override string ToString()
        {
            StringBuilder functionSignature = new StringBuilder();
            string functionParameters = string.Join(", ", _parameters.Select(p => p.ToString())); 
            functionSignature.Append($"{_returnType} {Name}");
            functionSignature.Append($" ({functionParameters})");
            return functionSignature.ToString();
        }

        public CastType ReturnType()
        {
            return _returnType;
        }
    }
}