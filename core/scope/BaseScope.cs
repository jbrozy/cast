using Cast.core.symbols;

namespace Cast.core.scope;

public class BaseScope(IScope? parent, string name) : IScope
{
    private readonly Dictionary<string, Symbol> _symbols = new();
    
    public IScope? EnclosingScope { get; set; } = parent;
    public string ScopeName { get; set; } = name;

    public void Define(Symbol symbol)
    {
        if (_symbols.ContainsKey(symbol.Name))
        {
            throw new Exception("Duplicate symbol name: " + symbol.Name);
        }

        symbol.Scope = this;
        _symbols[symbol.Name] = symbol;
    }

    public Symbol? Resolve(string name)
    {
        if (_symbols.TryGetValue(name, out var symbol))
        {
            return symbol;
        }

        if (EnclosingScope != null)
        {
            return EnclosingScope.Resolve(name);
        }
        
        throw new Exception("Symbol not found: " + name);
    }

    public List<Symbol> GetSymbols()
    {
        throw new NotImplementedException();
    }
}