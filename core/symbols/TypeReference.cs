using Antlr4.Runtime.Dfa;

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

    public bool Equals(TypeReference? other)
    {
        if (Name != other?.Name) return false;
        if (ResolvedType != other?.ResolvedType) return false;
        if (RawArgs.Count != other?.RawArgs.Count) return false;
        for (int i = 0; i < RawArgs.Count; i++){
            if (RawArgs[i] != other?.RawArgs[i]) return false;
        }
        
        return true;
    }
}