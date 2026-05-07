using System;
using System.Collections.Generic;

namespace cast.core.logging
{
    public class ErrorLogger
    {
        private List<string> errors = new List<string>();
        public bool HasErrors => errors.Count > 0;
        
        public void Log(Antlr4.Runtime.IToken token, string message)
        {
            int line = token.Line;
            int column = token.Column;
            errors.Add($"[Line {line}:{column}] Semantic Error: {message}");
        }
        
        public List<string> Errors => errors;

        public void Print()
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
        }
    }
}