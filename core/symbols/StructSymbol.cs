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
    
    public Symbol? ResolveConstructor(List<Symbol> parameters)
    {
        string signature = Name + "(" + parameters.Select(o => o.Name).Aggregate((a, b) => $"{a}, {b}") + ")";
        return Constructors.FirstOrDefault(c => c.GetSignature() ==  signature);
    }

    public void Define(Symbol symbol)
    {
        Fields.Add(symbol.Name, symbol);
    }
    
    public FunctionSymbol ResolveFunction(string name)
    {
        // Handle Swizzling if enabled
        if (this.AllowSwizzle)
        {
            throw new NotImplementedException($"Resolve has not been implemented for struct");
        }
        return Functions.FirstOrDefault(o => o.Name == name);
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

    public List<Symbol> GetSymbols()
    {
        return Fields.Select(o => o.Value).ToList();
    }

    public Symbol? ResolveFunctionOverload(string name, List<Symbol> overloads)
    {
        if (overloads.Count == 0)
        {
            throw new NotImplementedException($"{Name} has no overloads");
        }

        if (overloads.Any(o => o?.Name == null))
        {
            Console.WriteLine("debug");
        }
        string signature = overloads.Select(o => o.Name).Aggregate((a, b) => $"{a}, {b}");
        foreach (var function in Functions)
        {
            if (function.Name != name) continue;
            string functionSig = function.Parameters.Select(o => o.TypeRef.Name).Aggregate((a, b) => $"{a}, {b}");
            if (signature == functionSig)
            {
                Console.WriteLine($"Function Signature: {functionSig}, Given Signature: {signature}");
                return function;
            }
        }

        return null;
    }
}