namespace Types.Interfaces.Model;

public interface IChannelIdentifiable
{
    /// <summary>
    /// ID of related channel
    /// </summary>
    public Guid ChannelId { get; set; }

    /// <summary>
    /// Name of the column containing ID of the related hub
    /// </summary>
    public const string ColumnName = "channel_id";
}