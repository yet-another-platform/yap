using System.Text.RegularExpressions;

namespace Types.Interfaces.Model;

public partial interface IEmail
{
    /// <summary>
    /// Email value
    /// </summary>
    public string? Email { get; set; }

    public const string ColumnName = "email";
    public const int MaxLength = 254;

    public static bool Validate(IEmail value)
    {
        return value.Email is not null && ValidationRegex().IsMatch(value.Email);
    }

    [GeneratedRegex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$")]
    private static partial Regex ValidationRegex();
}