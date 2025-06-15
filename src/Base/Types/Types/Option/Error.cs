using Types.DataTransferObjects;

namespace Types.Types.Option;

public class Error
{
    public required string Message { get; set; }
    public ErrorType Type { get; set; } = ErrorType.BadRequest;

    public GeneralErrorWrapper GetErrorWrapper()
    {
        return new GeneralErrorWrapper
        {
            Message = Message
        };
    }
}