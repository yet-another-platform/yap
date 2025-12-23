using Types.Interfaces.Model;

namespace Hubs.Domain.DataTransferObjects;

public class HubDto : IIdentifiable, IUserIdentifiable, IName, ICreated, IUpdated
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Updated { get; set; }
    public List<ChannelDto> ChannelList { get; set; } = [];
}