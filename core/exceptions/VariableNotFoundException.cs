namespace Cast.core.exceptions;

public class VariableNotFoundException(string variable) : Exception
{
    public override string ToString()
    {
        return $"Variable: '{variable}' not found in Scope.";
    }
}