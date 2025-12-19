namespace Cast;

public enum CastType
{
    INT,
    FLOAT,
    STRUCT,
    VOID,
    BOOL,
    SPACE,
    IDENTIFIER,
    FUNCTION
}

public class CastSymbol
{
    public CastType CastType { get; }
    public string StructName { get; set; }
    public List<CastSymbol> Parameters { get; }
    public IDictionary<string, CastSymbol> Fields { get; set; }
    public IDictionary<FunctionKey, CastSymbol> Functions { get; } = new Dictionary<FunctionKey, CastSymbol>();
    public string ParamName { get; set; }
    public CastSymbol? ReturnType { get; set; }
    public CastSymbol? TypeSpace { get; set; }
    public CastSymbol? Constructor { get; set; }
    public bool IsUniform { get; set; } = false;
    public string FunctionName { get; set; }
    public string SpaceName { get; set; }
    public string Identifier { get; set; }
    public bool IsReturn { get; set; } = false;
    public bool IsDeclaration { get; set; } = false;

    public bool IsStruct()
    {
        return CastType == CastType.STRUCT;
    }

    // When this is set to true, the field ConversionFrom and ConversionTo are set
    public bool IsConversionMatrix { get; set; } = false;
    public CastSymbol? ConversionFrom { get; set; }
    public CastSymbol? ConversionTo { get; set; }

    public bool IsFunction => CastType == CastType.FUNCTION;

    public static CastSymbol Int => new(CastType.INT);
    public static CastSymbol Float => new(CastType.FLOAT);
    public static CastSymbol Void => new(CastType.VOID);
    public static CastSymbol Bool => new(CastType.BOOL);

    public static CastSymbol ID(string identifier)
    {
        return new CastSymbol(CastType.IDENTIFIER) { Identifier = identifier };
    }

    public static CastSymbol Space(string spaceName)
    {
        return new CastSymbol(CastType.SPACE)
        {
            SpaceName = spaceName
        };
    }

    public static CastSymbol Function(string functionName, IEnumerable<CastSymbol> paramTypes, CastSymbol returnType)
    {
        return new CastSymbol(functionName, paramTypes, returnType);
    }

    private CastSymbol(CastType castType)
    {
        CastType = castType;
    }

    private CastSymbol(string functionName, IEnumerable<CastSymbol> paramTypes, CastSymbol returnType)
    {
        CastType = CastType.FUNCTION;
        FunctionName = functionName;
        Parameters = paramTypes.ToList();
        ReturnType = returnType;
    }

    public static CastSymbol Struct(string structName, IDictionary<string, CastSymbol> fields)
    {
        return new CastSymbol(CastType.STRUCT)
        {
            StructName = structName,
            Fields = fields
        };
    }

    public override bool Equals(object obj)
    {
        if (obj is CastSymbol other)
        {
            if (CastType == other.CastType) return true;
            if (CastType == CastType.STRUCT) return StructName == other.StructName;
        }

        return false;
    }

    public CastSymbol Clone()
    {
        return (CastSymbol)MemberwiseClone();
    }
}