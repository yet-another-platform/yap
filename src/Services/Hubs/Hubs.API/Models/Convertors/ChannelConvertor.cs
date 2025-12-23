using Hubs.Domain.DataTransferObjects;

namespace Hubs.API.Models.Convertors;

public static class ChannelConvertor
{
    public static ChannelDto ToDto(this Channel channel)
    {
        return new ChannelDto()
        {
            Id = channel.Id,
            HubId = channel.HubId,
            Name = channel.Name,
            Description = channel.Description,
            Created = channel.Created,
            Updated = channel.Updated,
        };
    }
    
    public static IEnumerable<ChannelDto> ToDto(this IEnumerable<Channel> channels) => channels.Select(ToDto);
}