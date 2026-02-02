using Cast.core.symbols;

namespace Cast.core.scope;

public class StructScope : IScope
{
    private readonly Dictionary<string, Symbol> _symbols = new();
    
    public IScope? EnclosingScope { get; set; }
    public string ScopeName => $"struct";
    
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
        return _symbols.GetValueOrDefault(name);
    }
}