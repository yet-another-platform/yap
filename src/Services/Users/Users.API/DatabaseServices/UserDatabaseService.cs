using System.Data;
using System.Text;
using Dapper;
using Database;
using Database.Extensions;
using Types.Interfaces.Model;
using Types.Types;
using Types.Types.Option;
using Users.API.Constants.Database;
using Users.API.Database;
using Users.API.DatabaseServices.Interfaces;
using Users.API.Extensions;
using Users.API.Helpers;
using Users.API.Models;
using Users.Domain.Enums;

namespace Users.API.DatabaseServices;

public class UserDatabaseService : DatabaseServiceBase<User>, IUserDatabaseService
{
    public UserDatabaseService(Func<IDbConnection> connectionFactory) : base(connectionFactory, UsersTable.TableName)
    {
    }

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

    public async Task<User?> GetAsync(GuidChecked id, bool includeInvalid = false)
    {
        var dynamicParameters = new { id }.ToDynamicParameters();
        var query = new StringBuilder($"SELECT * FROM {UsersTable.TableName} WHERE {IIdentifiable.ColumnName} = @id");
        query.AppendValidUserStateFilter(dynamicParameters, includeInvalid);
        query.Append(';');
        var connection = ConnectionFactory();
        return await connection.QueryFirstOrDefaultAsync<User>(query.ToString(), dynamicParameters);
    }


    public async Task<bool> UpdateAsync(User entity)
    {
        const string query = $"""
                              UPDATE {UsersTable.TableName} SET
                                  {IEmail.ColumnName} = @email,
                                  {IUsername.ColumnName} = @username,
                                  {UsersTable.PasswordHash} = @passwordHash,
                                  {UsersTable.State} = @state,
                                  {IUpdated.ColumnName} = @updated
                              WHERE {IIdentifiable.ColumnName} = @id
                              RETURNING *;
                              """;

        return await UpdateAsyncInternal(query, entity);
    }

    public async Task<bool> ExistsAsync(GuidChecked id, bool includeInvalid = false)
    {
        var query = new StringBuilder($"""
                                       SELECT EXISTS (
                                           SELECT 1 FROM {UsersTable.TableName}
                                               WHERE {IIdentifiable.ColumnName} = @id
                                       );
                                       """);
        var dynamicParameters = new { id }.ToDynamicParameters();
        query.AppendValidUserStateFilter(dynamicParameters, includeInvalid);
        query.Append(';');

        var connection = ConnectionFactory();
        return await connection.QueryFirstOrDefaultAsync<bool>(query.ToString(), dynamicParameters);
    }

    public async Task<User?> GetByEmailAsync(string email, bool includeInvalid = false)
    {
        var query = new StringBuilder($"""
                                       SELECT * FROM {UsersTable.TableName}
                                           WHERE {IEmail.ColumnName} = @email
                                       """);
        var dynamicParameters = new { email }.ToDynamicParameters();
        query.AppendValidUserStateFilter(dynamicParameters, includeInvalid);
        query.Append(';');

        using var connection = ConnectionFactory();
        return await connection.QueryFirstOrDefaultAsync<User>(query.ToString(), dynamicParameters);
    }

    public async Task<User?> GetByUsernameAsync(string username, bool includeInvalid = false)
    {
        var query = new StringBuilder($"""
                                       SELECT * FROM {UsersTable.TableName}
                                           WHERE {IUsername.ColumnName} = @username
                                       """);
        var dynamicParameters = new { username }.ToDynamicParameters();
        query.AppendValidUserStateFilter(dynamicParameters, includeInvalid);
        query.Append(';');

        using var connection = ConnectionFactory();
        return await connection.QueryFirstOrDefaultAsync<User>(query.ToString(), dynamicParameters);
    }
}