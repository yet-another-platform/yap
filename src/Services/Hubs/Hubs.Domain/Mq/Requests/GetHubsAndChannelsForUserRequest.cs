using Types.Types;

namespace Hubs.Domain.Mq.Requests;

public record GetHubsAndChannelsForUserRequest
{
    public GuidChecked UserId { get; init; }
}