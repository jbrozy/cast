namespace cast.core.models.symbols
{
    public class VariableSymbol : AbstractSymbol
    {
        public CastType Type { get; }
        public CastType Payload { get; } // only for samplers
        private Modifier Modifier { get; set; }
        
        public VariableSymbol(string name, CastType type, Modifier modifier = Modifier.NONE) : base(name)
        {
            Type = type;
            Modifier = modifier;
        }
    }
}