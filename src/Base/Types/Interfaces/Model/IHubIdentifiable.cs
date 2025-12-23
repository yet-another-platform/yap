namespace Types.Interfaces.Model;

public interface IHubIdentifiable
{
    /// <summary>
    /// ID of related hub
    /// </summary>
    public Guid HubId { get; set; }

    /// <summary>
    /// Name of the column containing ID of the related hub
    /// </summary>
    public const string ColumnName = "hub_id";
}