namespace Cast.core.symbols;

public class VariableSymbol : Symbol
{
    public CastParser.ExpressionContext? Initializer { get; set; }
    public TypeReference TypeRef { get; set; } = new();
    public override SymbolKind Kind => SymbolKind.Variable;
    
    public int ArraySize { get; set; }
    public int Offset { get; set; }

    public override TypeSymbol? Type
    {
        get => TypeRef.ResolvedType;
        set => TypeRef.ResolvedType = value;
    }
}