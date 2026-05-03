namespace cast.core.models.symbols
{
    public abstract class AbstractSymbol
    {
        public string Name { get; set; }
        
        protected AbstractSymbol(string name) 
        {
            Name = name;
        }
    }
}