using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Types.Interfaces.Model;

namespace Hubs.Domain.DataTransferObjects;

public class MessageDto : IIdentifiable<long>, IChannelIdentifiable, IUserIdentifiable, ICreated, IUpdated
{
    public long Id { get; set; }
    public Guid ChannelId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Updated { get; set; }
}