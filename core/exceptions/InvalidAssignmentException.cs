using System.Text;

namespace Cast.core.exceptions;

public class InvalidAssignmentException(CastSymbol left, CastSymbol right) : Exception
{
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (left.CastType != right.CastType)
        {
            sb.Append($"Incompatible type: '{left.StructName}'. Expected: '{right.StructName}'\n");

            if (left.CastType == CastType.STRUCT)
            {
                if (left.StructName != right.StructName)
                {
                    sb.Append($"Incompatible type: '{left.StructName}'. Expected: '{right.StructName}'\n");
                }
            }
        }
        
        if (left.SpaceName != right.SpaceName)
        {
            sb.Append($"Incompatible space: '{left.SpaceName}'. Expected: '{right.SpaceName}'\n");
        }
        
        return sb.ToString();
    }
}