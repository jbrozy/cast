using Antlr4.Runtime.Tree;

namespace Cast;

public class Scope<T>(Scope<T>? parent = null)
    where T : class
{
    public Scope<T>? Parent { get; } = parent;
    private Dictionary<string, T> _symbols = new();

    public IDictionary<string, T> GetAll()
    {
        return _symbols;
    }

    public T? Lookup(string name)
    {
        if (_symbols.TryGetValue(name, out var value)) return value;
        if (Parent != null) return Parent.Lookup(name);

        return null;
    }

    public void Define(string symbolName, T value)
    {
        if (_symbols.ContainsKey(symbolName)) throw new Exception($"Symbol: {symbolName} has already been defined.");
        _symbols[symbolName] = value;
    }

    public void Assign(string symbolName, T value)
    {
        if (!_symbols.ContainsKey(symbolName)) throw new Exception($"Symbol: {symbolName} not found in Scope.");
        _symbols[symbolName] = value;
    }

    public bool TryGetSymbol(string name, out T? symbol)
    {
        if (Exists(name))
        {
            symbol = Lookup(name);
            return true;
        }

        symbol = null;
        return false;
    }

    public bool Exists(string symbolName)
    {
        return _symbols.ContainsKey(symbolName);
    }
}