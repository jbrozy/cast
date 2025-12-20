namespace Cast;

public class FunctionKey
{
    public string Name { get; set; }
    public List<CastSymbol> Types { get; set; }
    
    private FunctionKey(){}

    public static FunctionKey Of(string name, List<CastSymbol> types = null)
    {
        return new FunctionKey
        {
            Name = name,
            Types = types
        };
    }

    private int GetStableHashCode(string str)
    {
        unchecked
        {
            var hash = 23;
            foreach (var c in str) hash = hash * 31 + c;
            return hash;
        }
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Name);
        if (Types != null)
            foreach (var type in Types)
            {
                hash.Add(type.CastType.ToString());
                // if this is set, I think we have to implement function overrides for vecN<A> * vecN<A> aswell
                // should we maybe do this? this will require a lot more default implementations...
                // if(!string.IsNullOrEmpty(type.SpaceName)) hash.Add(type.SpaceName);
            }

        return hash.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        bool valid;
        var other = (FunctionKey)obj;
        valid = other.Name == Name;

        if (other.Types.Count != Types.Count) valid = false;

        for (var i = 0; i < Types.Count; ++i)
            if (Types[i].CastType != other.Types[i].CastType)
                valid = false;

        return valid;
    }
}