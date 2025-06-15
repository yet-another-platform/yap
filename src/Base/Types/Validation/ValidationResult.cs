namespace Types.Validation;

public class ValidationResult
{
    public List<ValidationError> Errors { get; set; } = [];
    public bool IsValid => Errors.Count == 0;
}