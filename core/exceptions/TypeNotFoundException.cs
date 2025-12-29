namespace Cast.core.exceptions;

public class TypeNotFoundException(string type) : Exception
{
    public override string ToString()
    {
        return $"Type '{type}' not found in scope.";
    }
}