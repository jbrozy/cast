namespace Cast.core;

public class TypeChecker
{
    public static bool Check(CastSymbol lhs, CastSymbol rhs)
    {
        // incompatible spaces
        if (lhs.SpaceName != rhs.SpaceName) return false;
        
        // incompatible structs
        if (lhs.StructName != rhs.StructName) return false;
        
        return true;
    }
}