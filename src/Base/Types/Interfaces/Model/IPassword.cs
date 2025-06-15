using System.Text.RegularExpressions;

namespace Types.Interfaces.Model;

public partial interface IPassword
{
    /// <summary>
    /// Gets or sets password value
    /// </summary>
    public string Password { get; set; }
    
    public static bool Validate(IPassword value)
    {
        return ValidationRegex().IsMatch(value.Password);
    }

    [GeneratedRegex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")]
    private static partial Regex ValidationRegex();
}