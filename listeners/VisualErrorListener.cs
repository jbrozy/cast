using Antlr4.Runtime;

namespace Cast.listeners;

public class VisualErrorListener : BaseErrorListener
{
    private readonly string[] _lines;

    public VisualErrorListener(string sourceContent)
    {
        _lines = sourceContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    }

    public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
        RecognitionException e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[Syntax Error] {msg}");
        Console.ResetColor();
        
        int lineIndex = line - 1;

        if (lineIndex >= 0 && lineIndex < _lines.Length)
        {
            string errorLine = _lines[lineIndex];
            
            // Print the actual line of code
            Console.WriteLine($"   {errorLine}");
            
            // Create a string of spaces followed by a caret '^'
            // charPositionInLine tells us exactly how many spaces to indent
            string indicator = new string(' ', charPositionInLine) + "^--- Here";
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"   {indicator}");
            Console.ResetColor();
        }
    }
}