using System.Collections.Generic;
using System.Text;
using cast.core.models;
using cast.core.models.symbols;
using cast.core.visitor.configuration;

namespace cast.core.parser.programs
{
    public class GlslShaderProgram
    {
        public GLSLShaderConfiguration? Configuration { get; set; }
        public string Shader { get; set; } = string.Empty;
        
        /// <summary>
        /// the following fields are used for creating a graph
        /// </summary>
        public List<VariableSymbol> Uniforms { get; set; }
        public List<VariableSymbol> Inputs { get; set; }
        public List<VariableSymbol> Outputs { get; set; }
        public List<VariableSymbol> Textures { get; set; }

        public string GetShaderCode()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"#version {Configuration.GetVersion()}\n");
            builder.Append(Shader);
            return builder.ToString();
        }
    }
}