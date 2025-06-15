namespace Types.Interfaces.Model;

public interface IIdentifiable<TId>
{
    /// <summary>
    /// ID of the entity
    /// </summary>
    public TId Id { get; set; }

    /// <summary>
    /// Name of the column containing ID in database
    /// </summary>
    public const string ColumnName = "id";
}

public interface IIdentifiable : IIdentifiable<Guid>;