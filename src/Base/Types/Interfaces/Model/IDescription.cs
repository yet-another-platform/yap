namespace Types.Interfaces.Model;

public interface IDescription
{
    /// <summary>
    /// Description value
    /// </summary>
    public string Description { get; set; }

    public const string ColumnName = "description";
    
    public const int MaxLength = 1024;

    public static bool Validate(IDescription value)
    {
        return value.Description.Length <= MaxLength;
    }
}