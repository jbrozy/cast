using System.Collections.Generic;
using cast.core.models;

namespace cast.core.visitor.configuration
{
    public class GLSLShaderConfiguration : BaseConfiguration
    {
        public int Version { get; set; }
        public bool Core { get; set; }
        public bool Compatibility { get; set; }
        
        public List<CastType> Uniforms { get; set; }
        public List<CastType> Inputs { get; set; }
        public List<CastType> Outputs { get; set; }
        public List<CastType> Textures { get; set; }
        
        public string GetVersion()
        {
            return Version.ToString();
        }
    }
}