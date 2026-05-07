using System;
using System.Linq;
using cast.core.parser;
using cast.core.parser.programs;
using cast.core.visitor.configuration;

namespace cast.api.core
{
    public class CompilationService
    {
        public CompilationService()
        {
        }

        public CompilationResult Compile(string input)
        {
            GlslParser parser = new GlslParser();
            GlslShaderProgram result = parser.Parse(input);

            var s =  new CompilationResult()
            {
                Errors = parser.GetLogs().ToList(),
                Result = result.GetShaderCode()
            };

            return s;
        }
    }
}