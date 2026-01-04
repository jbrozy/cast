using System.Runtime.CompilerServices;
using System.Text;
using Antlr4.Runtime;

namespace Cast.core.exceptions;

public class VariableNotFoundException : CastException
{
    public VariableNotFoundException(ParserRuleContext ctx, string variable) : base(buildMessage(ctx, variable))
    {
    }

    private static string buildMessage(ParserRuleContext ctx, string variable)
    {
        string prefix = GetLoc(ctx);
        StringBuilder message = new StringBuilder();
        message.AppendLine(prefix);
        message.AppendLine($"Variable '{variable}' was not found in scope.");
        return message.ToString();
    }
}