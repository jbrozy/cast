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
            _sourceLines = source.Split('\n');
        }

        public void Log(Antlr4.Runtime.IToken token, string message)
        {
            int line = token.Line;
            int column = token.Column;
            string tokenText = token.Text;

            string error = $"[Line {line + 1}:{column}] Semantic Error: {message}";

            if (_sourceLines != null && line > 0 && line <= _sourceLines.Length)
            {
                string sourceLine = _sourceLines[line - 1].Trim();
                if (!string.IsNullOrEmpty(sourceLine))
                    error += $" at `{sourceLine}`";
            }

            errors.Add(error);
        }

        public void Log(int column, int line, string message)
        {
            errors.Add($"error: {message}");
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
