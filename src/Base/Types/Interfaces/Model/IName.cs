namespace Types.Interfaces.Model;

public interface IName
{
    /// <summary>
    /// Name value
    /// </summary>
    public string Name { get; set; }

    public const string ColumnName = "name";
    
    public const int MaxLength = 32;
    public const int MinLength = 2;

    public static bool Validate(IName value)
    {
        return value.Name.Length is >= MinLength and <= MaxLength;
    }
}