namespace Types.Exceptions;

public class MissingClaimException : Exception
{
    public MissingClaimException() : base()
    {
    }

    public MissingClaimException(string message) : base(message)
    {
    }

    public MissingClaimException(string message, Exception inner) : base(message, inner)
    {
    }
}