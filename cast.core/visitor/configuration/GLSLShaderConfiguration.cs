using System.Collections.Generic;
using cast.core.models;

namespace cast.core.visitor.configuration
{
    public class GLSLShaderConfiguration : BaseConfiguration
    {
        public int Version { get; set; }
        public string Profile { get; set; } = string.Empty;
        
        public string GetVersion()
        {
            return string.IsNullOrEmpty(Profile) ? Version.ToString() : $"{Version} {Profile}";
        }
    }
}