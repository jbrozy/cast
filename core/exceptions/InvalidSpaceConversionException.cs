namespace Cast.core.exceptions;

public class InvalidSpaceConversionException(string given, string expected) : Exception
{
    public override string ToString()
    {
        return $"Invalid Space conversion: Given: {given}, Expected: {expected}";
    }
}