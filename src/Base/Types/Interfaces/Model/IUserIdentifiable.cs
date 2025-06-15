namespace Types.Interfaces.Model;

public interface IUserIdentifiable
{
    /// <summary>
    /// ID of the parent user of the entity
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Name of the column containing ID of the parent user in database
    /// </summary>
    public const string ColumnName = "user_id";
}