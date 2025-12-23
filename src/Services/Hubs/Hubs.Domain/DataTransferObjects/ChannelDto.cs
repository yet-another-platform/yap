using Types.Interfaces.Model;

namespace Hubs.Domain.DataTransferObjects;

public class ChannelDto : IIdentifiable, IHubIdentifiable, IName, IDescription, ICreated, IUpdated
{
    public Guid Id { get; set; }
    public Guid HubId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Updated { get; set; }
}
