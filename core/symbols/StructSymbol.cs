using Cast.core.scope;

namespace Cast.core.symbols;

public class StructSymbol : Symbol, IScope
{
    public IScope? EnclosingScope { get; set; }
    public override SymbolKind Kind => SymbolKind.Struct;
    public string ScopeName => $"struct {Name}";
    
    #region flags
    public bool AllowSwizzle { get; set; }= false;
    #endregion
    
    public Dictionary<string, Symbol> Fields { get; set; } = new();

    public List<FunctionSymbol> Functions { get; set; } = new();
    public List<FunctionSymbol> Constructors { get; set; } = new();

    public Symbol? ResolveMember(string memberName)
    {
        return Fields.GetValueOrDefault(memberName);
    }

    public void Define(Symbol symbol)
    {
        Fields.Add(symbol.Name, symbol);
    }

    public Symbol Resolve(string name)
    {
        if (Fields.TryGetValue(name, out var s)) return s;
        
        // Handle Swizzling if enabled
        if (this.AllowSwizzle)
        {
            throw new NotImplementedException($"Resolve has not been implemented for struct");
        }
        return null;
    }
}