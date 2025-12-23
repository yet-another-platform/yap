using Hubs.API.Models;
using Types.Interfaces.Database;
using Types.Types;

namespace Hubs.API.DatabaseServices.Interfaces;

public interface IChannelDatabaseService : ICrud<Channel>
{
    public Task<List<Channel>> ListForHub(GuidChecked hubId);
    public Task<List<Channel>> ListForHubAndUser(GuidChecked hubId, GuidChecked userId);
    public Task<bool> IsUserMember(GuidChecked channelId, GuidChecked userId);
}