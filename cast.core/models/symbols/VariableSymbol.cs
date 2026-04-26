using cast.core.models.symbols;

namespace cast.core.models
{
    public class VariableSymbol : AbstractSymbol
    {
        public CastType Type { get; }
        private Modifier Modifier { get; set; }
        
        public VariableSymbol(string name, CastType type, Modifier modifier = Modifier.NONE) : base(name)
        {
            Type = type;
            Modifier = modifier;
        }
    }
}