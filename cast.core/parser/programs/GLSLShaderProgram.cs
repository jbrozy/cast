using System.Text;
using cast.core.visitor.configuration;

namespace cast.core.parser.programs
{
    public class GLSLShaderProgram
    {
        public GLSLShaderConfiguration? Configuration { get; set; }
        public string Shader { get; set; }

        public string GetShaderCode()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"#version {Configuration.GetVersion()}");
            builder.Append(Shader);
            return builder.ToString();
        }
    }
}