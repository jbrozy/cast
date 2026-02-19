using Cast.core.scope;

namespace Cast.core.symbols;

public class FunctionSymbol : Symbol, IScope
{
    public List<VariableSymbol> Parameters { get; set; } = [];
    private Dictionary<string, Symbol> _symbols = new();
    
    #region flags
    public bool IsExternal { get; set; } = false;
    public override SymbolKind Kind => SymbolKind.Function;
    #endregion
    
    #region type info
    public override TypeSymbol? Type => ReturnTypeRef?.ResolvedType;
    public TypeReference ReturnTypeRef { get; set; } = new ();
    public string Signature => GetSignature();
    #endregion

    public string GetSignature()
    {
        string paramTypes = string.Join(", ", Parameters.Select(c => c.TypeRef.Name));
        if (Parameters.Count == 0)
        {
            throw new Exception($"{Name} has no symbols");
        }
        return $"{Name}({paramTypes})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not FunctionSymbol other) return false;
        return true;
    }

    public IScope? EnclosingScope { get; set; }
    public string ScopeName { get; }
    public void Define(Symbol symbol)
    {
        if (_symbols.ContainsKey(symbol.Name))
        {
            throw new Exception($"Variable {symbol.Name} is already defined");
        }
        _symbols.Add(symbol.Name, symbol);
    }

    public Symbol? Resolve(string name)
    {
        if (_symbols.TryGetValue(name, out Symbol? symbol)) return symbol;
        
        return EnclosingScope?.Resolve(name);
    }

    public List<Symbol> GetSymbols()
    {
        return _symbols.Values.ToList();
    }
}