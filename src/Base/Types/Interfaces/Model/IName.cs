using System.Text.RegularExpressions;
using Types.Extensions;

namespace Types.Interfaces.Model;

public partial interface IName
{
    /// <summary>
    /// Name value
    /// </summary>
    public string Name { get; set; }

    public const string ColumnName = "name";
    
    public const int MaxLength = 128; // Always update the value in validation regex as well!!
    public const int MinLength = 2; // Always update the value in validation regex as well!!

    public static bool Validate(IName value)
    {
        if (string.IsNullOrWhiteSpace(value.Name))
        {
            return false;
        }
        
        return ValidationRegex().IsMatch(value.Name);
    }

    // The length must mach MaxLength and MinLength constants!
    [GeneratedRegex("^[a-zA-Z0-9]{2,128}$")]
    private static partial Regex ValidationRegex();
}