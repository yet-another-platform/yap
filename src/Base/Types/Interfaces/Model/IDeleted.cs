using Types.Enums;

namespace Types.Interfaces.Model;

public interface IDeleted
{
    /// <summary>
    /// Information whether the item is deleted or not
    /// </summary>
    public bool Deleted { get; set; }
   
    /// <summary>
    /// The reason why the entity was deleted
    /// </summary>
    public DeletedReason DeletedReason { get; set; }

    /// <summary>
    /// Name of the column containing deleted info in database
    /// </summary>
    public const string ColumnName = "deleted";
}