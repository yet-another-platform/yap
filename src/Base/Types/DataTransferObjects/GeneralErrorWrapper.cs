namespace Types.DataTransferObjects;

public class GeneralErrorWrapper
{
    public readonly bool Error = true;
    public required string Message { get; init; }
}