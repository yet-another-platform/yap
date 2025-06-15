using Types.Interfaces.Database;
using Types.Types;
using Types.Types.Option;
using Users.API.Models;

namespace Users.API.DatabaseServices.Interfaces;

public interface IUserDatabaseService : ICreate<User>
{
    public Task<User?> GetAsync(GuidChecked id, bool includeInvalid = false);
    public Task<bool> ExistsAsync(GuidChecked id, bool includeInvalid = false);
    public Task<User?> GetByEmailAsync(string email, bool includeInvalid = false);
    public Task<User?> GetByUsernameAsync(string username, bool includeInvalid = false);
}