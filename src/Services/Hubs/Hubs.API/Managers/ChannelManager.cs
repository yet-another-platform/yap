using Hubs.API.DatabaseServices.Interfaces;
using Hubs.API.Models;
using Hubs.Domain.DataTransferObjects;
using Types.Extensions;
using Types.Types;
using Types.Types.Option;
using Types.Validation;

namespace Hubs.API.Managers;

public class ChannelManager(
    ILogger<ChannelManager> logger,
    IChannelDatabaseService channelDatabaseService,
    HubManager hubManager,
    Validator<NewChannelDto> newChannelDtoValidator)
{
    public async Task<Option<Channel>> CreateAsync(GuidChecked userId, GuidChecked hubId, NewChannelDto newChannelDto)
    {
        var validationResult = newChannelDtoValidator.Validate(newChannelDto);
        if (!validationResult.IsValid)
        {
            return validationResult.ToError();
        }
        
        var isUserOwnerResult = await hubManager.IsUserOwner(hubId, userId);
        if (!isUserOwnerResult.Ok)
        {
            return isUserOwnerResult.Error;
        }

        if (!isUserOwnerResult.Value)
        {
            return new Error { Message = "You are not owner of provided hub", Type = ErrorType.Forbidden };
        }

        var channel = new Channel
        {
            HubId = hubId,
            Name = newChannelDto.Name,
            Description = newChannelDto.Description,
            Created = DateTimeOffset.UtcNow
        };

        var channelId = await channelDatabaseService.CreateAsync(channel);
        if (channelId.IsEmpty())
        {
            return new Error { Message = "Failed to create channel in database", Type = ErrorType.ServiceError };
        }
        
        channel.Id = channelId;
        return channel;
    }

    public async Task<Option<List<Channel>>> ListForHubAsync(GuidChecked hubId, GuidChecked userId)
    {
        bool isMember = await hubManager.IsUserMember(hubId, userId);
        if (!isMember)
        {
            return new Error { Message = "You are not member of provided hub", Type = ErrorType.Forbidden };
        }
        
        var isOwnerResult = await hubManager.IsUserOwner(hubId, userId);
        if (!isOwnerResult.Ok)
        {
            return isOwnerResult.Error;
        }

        if (isOwnerResult.Value)
        {
            return await channelDatabaseService.ListForHub(hubId);
        }
        
        return await channelDatabaseService.ListForHubAndUser(hubId, userId);
    }

    public async Task<Option<bool>> CanUserSendMessage(GuidChecked channelId, GuidChecked userId)
    {
        var channel = await channelDatabaseService.GetAsync(channelId);
        if (!channel.Ok)
        {
            return channel.Error;
        }
        
        var isOwnerResult = await hubManager.IsUserOwner(channel.Value.HubId, userId);
        if (!isOwnerResult.Ok)
        {
            return isOwnerResult.Error;
        }

        if (isOwnerResult.Value)
        {
            return true;
        }
        
        return await channelDatabaseService.IsUserMember(channelId, userId);
    }
}