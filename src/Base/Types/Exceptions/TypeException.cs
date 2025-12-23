namespace Types.Exceptions;

public class TypeException: Exception
{
    public TypeException()
    {
    }
    
    public TypeException(string message) : base(message)
    {
    }
}