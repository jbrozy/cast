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
    public CastType CastType { get; set;  }
    public string StructName { get; set; }
    public List<CastSymbol> Parameters { get; set; }
    public IDictionary<string, CastSymbol> Fields { get; set; }
    public IDictionary<FunctionKey, CastSymbol> Functions { get; set;  } = new Dictionary<FunctionKey, CastSymbol>();
    public string ParamName { get; set; }
    public CastSymbol? ReturnType { get; set; }
    public CastSymbol TypeSpace { get; set; }
    public CastSymbol? Constructor { get; set; }
    
    // for conversion matrices
    public (CastSymbol from, CastSymbol to)? Conversion; 
    
    public bool IsUniform { get; set; } = false;
    public string FunctionName { get; init; }
    public string SpaceName { get; set; } = "None";
    public string Identifier { get; set; }
    public bool IsReturn { get; set; } = false;
    public bool IsDeclaration { get; set; } = false;
    public bool AllowSwizzle { get; set; } = false;

    public bool IsStruct()
    {
        return CastType == CastType.STRUCT;
    }

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
        return new CastSymbol(this.CastType)
        {
            StructName = this.StructName,
            SpaceName = this.SpaceName,

            TypeSpace = this.TypeSpace,
            ReturnType = this.ReturnType,
            Constructor = this.Constructor,

            Functions = this.Functions,
            Fields = this.Fields,

            FunctionName = this.FunctionName,
            Identifier = this.Identifier,

            IsUniform = this.IsUniform,
            IsReturn = this.IsReturn,
            IsDeclaration = this.IsDeclaration,
            Conversion = this.Conversion,
        };
    }
}