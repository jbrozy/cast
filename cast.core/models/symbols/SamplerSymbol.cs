namespace cast.core.models.symbols
{
    public class SamplerSymbol : AbstractSymbol
    {
        /*
         * sampler2D<vec3> a;
         */
        private bool _allowType;
        
        public SamplerSymbol(string name, bool allowType = true) : base(name)
        {
            _allowType = allowType;
        }
    }
}