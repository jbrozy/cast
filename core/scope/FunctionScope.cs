using Cast.core.symbols;

namespace Cast.core.scope;

public class FunctionScope : IScope
{
    private readonly Dictionary<string, Symbol> _symbols = new();
    public IScope? EnclosingScope { get; set; }
    public string ScopeName { get; }
    
    public void Define(Symbol symbol)
    {
        if (_symbols.ContainsKey(symbol.Name))
        {
            throw new Exception($"Symbol {symbol.Name} is already defined");
        }

        symbol.Scope = this;
        _symbols[symbol.Name] = symbol;
    }

    public Symbol? Resolve(string name)
    {
        if (_symbols.ContainsKey(name)) return _symbols[name];
        if (EnclosingScope != null) return EnclosingScope.Resolve(name);
        return null;
    }

    public List<Symbol> GetSymbols()
    {
        return _symbols.Values.ToList();
    }
}