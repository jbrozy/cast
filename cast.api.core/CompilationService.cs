using System.Linq;
using cast.core.parser;
using cast.core.parser.programs;

namespace cast.api.core
{
    public class CompilationService
    {
        public CompilationResult Compile(string input)
        {
            if (string.IsNullOrEmpty(input)) return default(CompilationResult);
            GlslParser parser = new GlslParser();
            GlslShaderProgram result = parser.Parse(input);

            return new CompilationResult()
            {
                Errors = parser.GetLogs().ToList(),
                Result = result.GetShaderCode()
            };
        }
    }
}