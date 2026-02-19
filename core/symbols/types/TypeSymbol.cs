namespace Cast.core.symbols;

public class TypeSymbol : Symbol
{
    public override SymbolKind Kind { get; } = SymbolKind.BuiltIn;
    
    public List<FunctionSymbol> Functions { get; set; } = new();

    public FunctionSymbol? ResolveFunctionBySig(string signature)
    {
        return Functions.FirstOrDefault(f => f.GetSignature() == signature);
    }

    public FunctionSymbol ResolveFunction(string name, List<TypeSymbol> parameters)
    {        
        if (parameters.Count == 0)
        {
            throw new NotImplementedException($"{Name} has no parameters");
        }
        
        string signature = parameters.Select(o => o.Name).Aggregate((a, b) => $"{a}, {b}");
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