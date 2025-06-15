using System.Data;
using Database;
using Types.Interfaces.Model;
using Types.Types.Option;
using Users.API.Constants.Database;
using Users.API.DatabaseServices.Interfaces;
using Users.API.Models;

namespace Users.API.DatabaseServices;

public class UserDatabaseService(Func<IDbConnection> connectionFactory) : DatabaseServiceBase<User>(connectionFactory, UsersTable.TableName), IUserDatabaseService
{
    public async Task<Guid> CreateAsync(User entity)
    {
        const string query = $"""
                        INSERT INTO {UsersTable.TableName} (
                            {IEmail.ColumnName},
                            {IUsername.ColumnName},
                            {UsersTable.PasswordHash},
                            {UsersTable.State},
                            {ICreated.ColumnName})
                        VALUES (
                            @email,
                            @username,
                            @passwordHash,
                            @state,
                            @created
                        )
                        RETURNING {IIdentifiable.ColumnName};
                        """;

        return await CreateAsyncInternal(query, entity);
    }

    public async Task<Option<User>> GetAsync(Guid id) => await GetAsyncInternal(id);

    public async Task<bool> UpdateAsync(User entity)
    {
        const string query = $"""
                              UPDATE {UsersTable.TableName} SET
                                  {IEmail.ColumnName},
                                  {IUsername.ColumnName},
                                  {UsersTable.PasswordHash},
                                  {UsersTable.State},
                                  {IUpdated.ColumnName})
                              RETURNING *;
                              """;

        return await UpdateAsyncInternal(query, entity);
    }

    public async Task<bool> ExistsAsync(Guid id) => await ExistsAsyncInternal(id);
}