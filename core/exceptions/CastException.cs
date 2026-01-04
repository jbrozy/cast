namespace Cast.core.exceptions;

public abstract class CastException : Exception
{
    public CastException(string msg) : base(msg)
    {
    }
    
    protected static string GetLoc(Antlr4.Runtime.ParserRuleContext context)
    {
        return $"[{context.Start.Line}:{context.Start.Column}] {context.GetText()}";
    }
}