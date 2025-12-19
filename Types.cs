namespace Cast;

public class Types
{
    public static CastSymbol ResolveType(string typeName)
    {
        switch (typeName)
        {
            case "int":
                return CastSymbol.Int;

            case "float":
                return CastSymbol.Float;

            case "bool":
                return CastSymbol.Bool;

            case "void":
                return CastSymbol.Void;

            case "mat3":
            case "mat4":
                return CastSymbol.Struct(typeName, new Dictionary<string, CastSymbol>());
            case "vec2":
            case "vec3":
            case "vec4":
                return CastSymbol.Struct(typeName, new Dictionary<string, CastSymbol>());
            default:
                return CastSymbol.Struct(typeName, new Dictionary<string, CastSymbol>());
        }
    }
}