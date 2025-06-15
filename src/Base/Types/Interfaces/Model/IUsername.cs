using System.Text.RegularExpressions;
using Types.Extensions;

namespace Types.Interfaces.Model;

public partial interface IUsername
{
    /// <summary>
    /// Username value
    /// </summary>
    public string Username { get; set; }

    public const string ColumnName = "username";
    
    public const int MaxLength = 64; // Always update the value in validation regex as well!!
    public const int MinLength = 2; // Always update the value in validation regex as well!!

    public static bool Validate(IUsername value)
    {
        if (string.IsNullOrWhiteSpace(value.Username))
        {
            return false;
        }
        
        return ValidationRegex().IsMatch(value.Username);
    }

    // The length must mach MaxLength and MinLength constants!
    [GeneratedRegex("^[a-zA-Z0-9]{2,64}$")]
    private static partial Regex ValidationRegex();
}