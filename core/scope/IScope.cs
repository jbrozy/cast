using Cast.core.symbols;

namespace Cast.core.scope;

public interface IScope
{
    IScope? EnclosingScope { get; set; }
    string ScopeName { get; }

    void Define(Symbol symbol);
    Symbol? Resolve(string name);
}