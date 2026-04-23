using System.Collections.Generic;
using System.Linq;

namespace cast.core.models
{
    public class Scope
    {
        private readonly HashSet<AbstractSymbol> _symbols = new HashSet<AbstractSymbol>();

        public AbstractSymbol? this[string name] => _symbols.ToList().FirstOrDefault(x => x.Name == name);

        public bool Add(AbstractSymbol symbol)
        {
            return _symbols.Add(symbol);
        }
    }
}