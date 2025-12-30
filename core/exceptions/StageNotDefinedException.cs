namespace Cast.core.exceptions;

public class StageNotDefinedException : Exception
{
    public override string ToString()
    {
        return "The stage has not been set.";
    }
}