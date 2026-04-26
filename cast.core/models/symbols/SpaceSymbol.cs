namespace cast.core.models
{
    public class SpaceSymbol : AbstractSymbol
    {
        public SpaceSymbol(string name) : base(name)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}