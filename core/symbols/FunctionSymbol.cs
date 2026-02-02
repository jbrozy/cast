namespace Cast.core.symbols;

public class FunctionSymbol : Symbol
{
    public List<VariableSymbol> Parameters { get; set; } = [];
    
    #region flags
    public bool IsExternal { get; set; } = false;
    public override SymbolKind Kind => SymbolKind.Function;
    #endregion
    
    #region type info
    public override Symbol? Type => ReturnTypeRef?.ResolvedType;
    public TypeReference ReturnTypeRef { get; set; } = new ();
    #endregion

    public string GetSignature()
    {
        string paramTypes = string.Join("", Parameters.Select(c => c.Type.ToString()));
        return $"{Name}({paramTypes})";
    }
}