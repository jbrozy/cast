namespace Cast.core.exceptions;

public class InvalidConversionMatrixException(string leftSpace, string rightSpace) : Exception
{
    public override string ToString()
    {
        return $"Unable to create conversion matrix with from '{leftSpace}' to '{rightSpace}'.";
    }
}