using System.Collections.Generic;
using System.Linq;
using cast.core.models.symbols;

namespace cast.core.models
{
    public class Scope
    {
        public Scope? Parent { get; }
        
        private readonly HashSet<AbstractSymbol> _symbols = new HashSet<AbstractSymbol>();

        public AbstractSymbol? this[string name]
        {
            get
            {
                if  (_symbols.Any(s => s.Name == name))
                    return _symbols.FirstOrDefault(s => s.Name == name);

                if (Parent == null)
                    return null;
                
                return Parent[name];
            }
        }

        public Scope(Scope parent = null)
        {
            Parent = parent;
        }

        public bool Define(AbstractSymbol symbol)
        {
            return _symbols.Add(symbol);
        }
    }
}