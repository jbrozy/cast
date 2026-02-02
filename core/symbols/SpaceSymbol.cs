namespace Cast.core.symbols;

public class SpaceSymbol : Symbol
{
    public SpaceSymbol? ParentSpace { get; set; }
    public override SymbolKind Kind => SymbolKind.Space;
}