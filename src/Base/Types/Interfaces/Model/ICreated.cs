namespace Types.Interfaces.Model;

public interface ICreated
{
    /// <summary>
    /// DateTime when the item was created
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// Name of the column containing created info in database
    /// </summary>
    public const string ColumnName = "created";
}