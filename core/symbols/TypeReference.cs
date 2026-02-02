namespace Cast.core.symbols;

public class TypeReference
{
    public string Name { get; set; }
    public List<string> RawArgs { get; set; } = new();
    public Symbol? ResolvedType { get; set; }
    public List<Symbol> ResolvedArgs { get; set; } = new();
    
    public override string ToString()
    {
        if (RawArgs.Count == 0) return Name;
        return $"{Name}<{string.Join(", ", RawArgs)}>";
    }
}