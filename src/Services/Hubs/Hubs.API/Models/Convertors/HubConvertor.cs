using Hubs.Domain.DataTransferObjects;

namespace Hubs.API.Models.Convertors;

public static class HubConvertor
{
    public static HubDto ToDto(this Hub hub)
    {
        return new HubDto
        {
            Id = hub.Id,
            UserId = hub.UserId,
            Name = hub.Name,
            Created = hub.Created,
            Updated = hub.Updated
        };
    }

    public static IEnumerable<HubDto> ToDto(this IEnumerable<Hub> hubs) => hubs.Select(ToDto);
}