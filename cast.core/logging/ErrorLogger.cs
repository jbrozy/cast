using System;
using System.Collections.Generic;

namespace cast.core.logging
{
    public class ErrorLogger
    {
        private List<string> errors = new List<string>();
        private string[]? _sourceLines;

        public bool HasErrors => errors.Count > 0;

        public void SetSource(string source)
        {
            _sourceLines = source.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public void Log(Antlr4.Runtime.IToken token, string message)
        {
            int line = token.Line;
            int column = token.Column;
            string src = "";

            if (_sourceLines != null && line > 0 && line <= _sourceLines.Length)
            {
                src = _sourceLines[line - 1];
                if (src.Length > 60)
                    src = src.Substring(0, 57) + "...";
            }

            errors.Add(!string.IsNullOrEmpty(src)
                ? $"[Line {line + 1}:{column}] Error: {message} \n  -> {src}"
                : $"[Line {line + 1}:{column}] Error: {message}");
            
            // Console.WriteLine($"[Line {line + 1}:{column}] Error: {message} \n  -> {src}");
        }

        public void Log(int line, int column, string message)
        {
            string src = "";

            if (_sourceLines != null && line > 0 && line <= _sourceLines.Length)
            {
                src = _sourceLines[line - 1];
                if (src.Length > 60)
                    src = src.Substring(0, 57) + "...";
            }

            errors.Add($"[Line {line + 1}:{column}] Error: {message}");
            if (!string.IsNullOrEmpty(src))
                errors.Add($"  -> {src}");
        }

        public void Log(string message)
        {
            errors.Add($"Error: {message}");
        }

        public List<string> Errors => errors;

        public void Print()
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
                Console.WriteLine();
            }
        }
    }
}
