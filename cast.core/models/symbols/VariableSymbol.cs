namespace cast.core.models
{
    public class VariableSymbol : AbstractSymbol
    {
        private CastType Type { get; set; }
        
        public VariableSymbol(string name, CastType type) : base(name)
        {
            Type = type;
        }
    }
}