namespace Cast.core.exceptions;

public class SpaceNotFoundException(string spaceName) : Exception
{
    public override string ToString()
    {
        return $"Space with name '{spaceName}' not found.";
    }
}