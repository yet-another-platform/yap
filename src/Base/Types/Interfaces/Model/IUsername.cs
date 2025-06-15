using System.ComponentModel.DataAnnotations.Schema;
using Types.Extensions;

namespace Types.Interfaces.Model;

public interface IUsername
{
    /// <summary>
    /// Username value
    /// </summary>
    public string Username { get; set; }

    public const string ColumnName = "username";
    public const int MaxLength = 64;
    public const int MinLength = 2;

    public static bool Validate(IUsername? value)
    {
        if (value is null)
        {
            return false;
        }

        return value.Username.Length is <= MaxLength and >= MinLength && value.Username.IsAsciiOnly();
    }
}