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
            sb.AppendLine($"Incompatible type: '{left.CastType}'. Expected: '{right.CastType}'.");

            if (left.CastType == CastType.STRUCT)
            {
                if (!string.IsNullOrEmpty(left.StructName) && left.StructName != right.StructName)
                {
                    sb.AppendLine($"Incompatible type: '{left.StructName}'. Expected: '{right.StructName}'.");
                }
            }
        }
        
        if (left.SpaceName != right.SpaceName)
        {
            sb.AppendLine($"Incompatible space: '{left.SpaceName}'. Expected: '{right.SpaceName}'");
        }

        return sb.ToString();
    }
}