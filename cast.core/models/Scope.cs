using System.Collections.Generic;
using cast.core.models.symbols;

namespace cast.core.models
{
    public class Scope
    {
        public Scope? Parent { get; }
        
        private readonly Dictionary<string, AbstractSymbol> _symbols = new Dictionary<string, AbstractSymbol>();

        public AbstractSymbol? this[string name]
        {
            get
            {
                if (_symbols.TryGetValue(name, out var symbol))
                    return symbol;

                return Parent?[name];
            }
        }

        public Scope(Scope parent = null)
        {
            Parent = parent;
        }

        public bool Define(AbstractSymbol symbol)
        {
            return _symbols.TryAdd(symbol.Name, symbol);
        }
    }
}