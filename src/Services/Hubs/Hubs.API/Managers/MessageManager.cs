using Hubs.API.Constants.Database;
using Hubs.API.DatabaseServices.Interfaces;
using Hubs.API.Models;
using Hubs.Domain.DataTransferObjects;
using Types.Types;
using Types.Types.Option;

namespace Hubs.API.Managers;

public class MessageManager(ILogger<MessageManager> logger, IMessageDatabaseService messageDatabaseService, ChannelManager channelManager)
{
    public async Task<Option<Message>> CreateAsync(GuidChecked userId, GuidChecked channelId, NewMessageDto newMessageDto)
    {
        if (newMessageDto.Content.Length is < 1 or > MessagesTable.ContentMaxLength)
        {
            return new Error { Message = $"Invalid message content. Content length must be between 1 and {MessagesTable.ContentMaxLength}" };
        }

        var canSendMessageResult = await channelManager.CanUserSendMessage(channelId, userId);
        if (!canSendMessageResult.Ok)
        {
            return canSendMessageResult.Error;
        }

        if (!canSendMessageResult.Value)
        {
            return new Error { Message = "You can't send messages to this channel.", Type = ErrorType.Forbidden };
        }

        var message = new Message
        {
            UserId = userId,
            ChannelId = channelId,
            Content = newMessageDto.Content,
            Created = DateTimeOffset.UtcNow
        };

        long messageId = await messageDatabaseService.CreateAsync(message);
        if (messageId < 1)
        {
            logger.LogError("Failed to create message by user: {UserID} in channel: {ChannelId}", userId, channelId);
            return new Error { Message = "Failed to create message.", Type = ErrorType.ServiceError };
        }
        
        message.Id = messageId;
        return message;
    }
}