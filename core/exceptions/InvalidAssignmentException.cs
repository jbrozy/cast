using System.Text;
using Antlr4.Runtime;

namespace Cast.core.exceptions;

[Serializable]
public class InvalidAssignmentException : CastException
{
    public InvalidAssignmentException(ParserRuleContext ctx, CastSymbol left, CastSymbol right) : base(buildMessage(ctx, left, right))
    {
    }

    private static string buildMessage(ParserRuleContext ctx, CastSymbol left, CastSymbol right)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(GetLoc(ctx));
        
        if (left.CastType != right.CastType)
        {
            if (left.CastType != CastType.STRUCT && right.CastType == CastType.STRUCT)
            {
                sb.AppendLine($"Expected type: '{left.CastType}'. Given: '{right.StructName}'.");
            }
            else
            {
                sb.AppendLine($"Expected type: '{left.CastType}'. Given: '{right.CastType}'.");
            }

            if (left.CastType == CastType.STRUCT)
            {
                if (!string.IsNullOrEmpty(left.StructName) && left.StructName != right.StructName)
                {
                    sb.AppendLine($"Expected type: '{left.StructName}'. Given: '{right.StructName}'.");
                }
            }
        }
        
        if (left.SpaceName != right.SpaceName)
        {
            sb.AppendLine($"Expected space: '{left.SpaceName}'. Given: '{right.SpaceName}'");
        }

        return sb.ToString();
    }
}