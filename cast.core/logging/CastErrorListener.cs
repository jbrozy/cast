using System;
using System.IO;
using Antlr4.Runtime;

namespace cast.core.logging
{
    public class CastErrorListener : IAntlrErrorListener<IToken>
    {
        private readonly ErrorLogger _errorLogger;
        public CastErrorListener(ErrorLogger errorLogger)
        {
            _errorLogger = errorLogger;
        }
        
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine,
            string msg, RecognitionException e)
        {
            _errorLogger.Log(line, offendingSymbol, msg);
        }

        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg, RecognitionException e)
        {
            _errorLogger.Log(offendingSymbol, msg);
        }
    }
}