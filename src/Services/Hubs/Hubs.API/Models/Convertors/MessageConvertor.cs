using Hubs.Domain.DataTransferObjects;

namespace Hubs.API.Models.Convertors;

public static class MessageConvertor
{
    public static MessageDto ToDto(this Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            UserId = message.UserId,
            ChannelId = message.ChannelId,
            Content = message.Content,
            Created = message.Created,
            Updated = message.Updated,
        };
    }
    
    public static IEnumerable<MessageDto> ToDto(this IEnumerable<Message> messages) => messages.Select(ToDto);
}