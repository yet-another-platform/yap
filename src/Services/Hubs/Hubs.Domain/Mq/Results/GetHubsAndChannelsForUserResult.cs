using Types.Types;

namespace Hubs.Domain.Mq.Results;

public record GetHubsAndChannelsForUserResult
{
    public IReadOnlyCollection<GuidChecked> HubIds { get; init; } = [];
    public IReadOnlyCollection<GuidChecked> ChannelIds { get; init; } = [];
}