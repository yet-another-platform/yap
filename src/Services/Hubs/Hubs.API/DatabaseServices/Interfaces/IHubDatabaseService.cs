using Hubs.API.Models;
using Types.Interfaces.Database;
using Types.Types;

namespace Hubs.API.DatabaseServices.Interfaces;

public interface IHubDatabaseService : ICreate<Hub>, IUpdate<Hub>, IGet<Hub>, IExists
{
    public Task<List<Hub>> ListJoinedForUser(GuidChecked userId);
    public Task<bool> IsUserMember(GuidChecked hubId, GuidChecked userId);
    public Task<bool> AddUserMembership(HubMembership hubMembership);
}