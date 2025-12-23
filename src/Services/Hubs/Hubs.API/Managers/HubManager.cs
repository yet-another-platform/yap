using Hubs.API.DatabaseServices.Interfaces;
using Hubs.API.Models;
using Hubs.Domain.DataTransferObjects;
using Types.Extensions;
using Types.Types;
using Types.Types.Option;
using Types.Validation;

namespace Hubs.API.Managers;

public class HubManager(
    ILogger<HubManager> logger,
    IHubDatabaseService hubDatabaseService,
    IChannelDatabaseService channelDatabaseService,
    Validator<NewHubDto> newHubDtoValidator)
{
    public async Task<Option<Hub>> CreateAsync(GuidChecked userId, NewHubDto newHubDto)
    {
        var validationResult = newHubDtoValidator.Validate(newHubDto);
        if (!validationResult.IsValid)
        {
            return validationResult.ToError();
        }

        var currentTime = DateTimeOffset.UtcNow;
        var hub = new Hub
        {
            UserId = userId,
            Name = newHubDto.Name,
            Created = currentTime
        };

        var hubId = await hubDatabaseService.CreateAsync(hub);
        if (hubId.IsEmpty())
        {
            logger.LogError("Failed to create hub in database");
            return new Error { Message = "Failed to create hub in database", Type = ErrorType.ServiceError };
        }

        hub.Id = hubId;

        bool defaultChannelSuccess = await CreateDefaultChannelForHub(hubId, currentTime);
        if (!defaultChannelSuccess)
        {
            logger.LogError("Failed to default channel in database for hub: {HubId}", hubId);
            return new Error { Message = "Failed to create default channel in database", Type = ErrorType.ServiceError };
        }

        if (!await JoinUserAsync(hubId, userId))
        {
            return new Error { Message = "Failed to join owner to newly created hub", Type = ErrorType.ServiceError };
        }

        return hub;
    }

    public async Task<bool> JoinUserAsync(GuidChecked hubId, GuidChecked userId)
    {
        var membership = new HubMembership
        {
            HubId = hubId,
            UserId = userId,
            Created = DateTimeOffset.UtcNow
        };

        return await hubDatabaseService.AddUserMembership(membership);
    }

    public async Task<Option<List<Hub>>> ListJoinedForUser(GuidChecked userId)
    {
        return await hubDatabaseService.ListJoinedForUser(userId);
    }

    public async Task<Option<bool>> IsUserOwner(GuidChecked hubId, GuidChecked userId)
    {
        var hubResult = await hubDatabaseService.GetAsync(hubId);
        if (!hubResult.Ok)
        {
            return hubResult.Error;
        }
        
        return hubResult.Value.UserId == userId;
    }

    public async Task<bool> IsUserMember(GuidChecked hubId, GuidChecked userId)
    {
        return await hubDatabaseService.IsUserMember(hubId, userId);
    }

    private async Task<bool> CreateDefaultChannelForHub(GuidChecked hubId, DateTimeOffset createdAt)
    {
        var channel = new Channel
        {
            HubId = hubId,
            Name = "general",
            Created = createdAt,
        };

        var channelId = await channelDatabaseService.CreateAsync(channel);
        return channelId.IsNotEmpty();
    }
}