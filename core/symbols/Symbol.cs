using Cast.core.scope;

namespace Cast.core.symbols;

public abstract class Symbol
{
    public string Name { get; set; } = "";
    public virtual Symbol? Type { get; set; }
    public abstract SymbolKind Kind { get; }
    public IScope? Scope { get; set; }
    public StorageQualifier Qualifier { get; set; } = StorageQualifier.None;
    public override string ToString() => $"{Kind} {Name}";
}