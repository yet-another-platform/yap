using Types.Types.Option;

namespace Types.Validation;

public class ValidationResult
{
    public List<ValidationError> Errors { get; set; } = [];
    public bool IsValid => Errors.Count == 0;

    public Error ToError()
    {
        if (IsValid)
        {
            throw new InvalidOperationException("Validation result is valid.");
        }
        
        return new Error
            { Message = string.Join('\n', Errors.Select(e => e.Message)) };
    }
}