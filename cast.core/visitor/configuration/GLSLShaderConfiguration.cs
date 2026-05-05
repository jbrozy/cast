using System.Collections.Generic;
using cast.core.models;

namespace cast.core.visitor.configuration
{
    public class GLSLShaderConfiguration : BaseConfiguration
    {
        public int Version { get; set; }
        public bool Core { get; set; }
        public bool Compatibility { get; set; }
        
        public string GetVersion()
        {
            return Version.ToString();
        }
    }
}