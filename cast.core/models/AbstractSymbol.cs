namespace cast.core.models
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