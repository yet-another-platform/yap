using Types.Enums;

namespace Types.Interfaces.Model;

public interface IDeletedReason
{
    /// <summary>
    /// The reason why the entity was deleted
    /// </summary>
    public DeletedReason DeletedReason { get; set; }

    /// <summary>
    /// Name of the column containing deleted reason info in database
    /// </summary>
    public const string ColumnName = "deleted_reason";
}