using System.Collections.Generic;

namespace cast.api.core
{
    public class CompilationResult
    {
        public string Result { get; set; }
        public List<string> Errors { get; set; }
    }
}