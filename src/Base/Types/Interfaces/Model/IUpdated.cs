namespace Types.Interfaces.Model;

public interface IUpdated
{
    /// <summary>
    /// DateTime when the item was updated for the last time
    /// </summary>
    public DateTimeOffset? Updated { get; set; }

    /// <summary>
    /// Name of the column containing updated info in database
    /// </summary>
    public const string ColumnName = "updated";
}