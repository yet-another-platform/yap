namespace Types.DataTransferObjects;

public class GeneralErrorWrapper
{
    public bool Error { get; private init; } = true;
    public required string Message { get; init; }
}