using System.Data;
using Dapper;
using Database;
using Database.Extensions;
using Hubs.API.Constants.Database;
using Hubs.API.DatabaseServices.Interfaces;
using Hubs.API.Models;
using Types.Interfaces.Model;
using Types.Types;
using Types.Types.Option;

namespace Hubs.API.DatabaseServices;

public class HubDatabaseService(Func<IDbConnection> connectionFactory)
    : DatabaseServiceBase<Hub>(connectionFactory, HubsTable.TableName), IHubDatabaseService
{
    public async Task<Guid> CreateAsync(Hub entity)
    {
        const string query = $"""
                              INSERT INTO {HubsTable.TableName} (
                                  {IUserIdentifiable.ColumnName},
                                  {IName.ColumnName},
                                  {ICreated.ColumnName})
                              VALUES (
                                  @userId,
                                  @name,
                                  @created
                              )
                              RETURNING {IIdentifiable.ColumnName};
                              """;

        return await CreateAsyncInternal(query, entity);
    }

    public async Task<bool> UpdateAsync(Hub entity)
    {
        const string query = $"""
                              UPDATE {HubsTable.TableName} SET
                                  {IName.ColumnName} = @name,
                                  {IUpdated.ColumnName} = @updated
                              WHERE {IIdentifiable.ColumnName} = @id
                              RETURNING *;
                              """;

        return await UpdateAsyncInternal(query, entity);
    }

    public async Task<List<Hub>> ListJoinedForUser(GuidChecked userId)
    {
        var parameters = new { userId }.ToDynamicParameters();
        const string query = $"""
                              SELECT {HubsTable.Prefix}.* FROM {HubsTable.TableName} AS {HubsTable.Prefix}
                              JOIN {HubMembershipsTable.TableName} AS {HubMembershipsTable.Prefix}
                                  ON {HubMembershipsTable.Prefix}.{IHubIdentifiable.ColumnName} = {HubsTable.Prefix}.{IIdentifiable.ColumnName}
                              WHERE {HubMembershipsTable.Prefix}.{IUserIdentifiable.ColumnName} = @userId;
                              """;

        using var connection = ConnectionFactory();
        var result = await connection.QueryAsync<Hub>(query, parameters);
        return result.ToList();
    }

    public async Task<bool> IsUserMember(GuidChecked hubId, GuidChecked userId)
    {
        var parameters = new { hubId, userId }.ToDynamicParameters();
        const string query = $"""
                              SELECT EXISTS (
                                  SELECT 1 FROM {HubMembershipsTable.TableName}
                                  WHERE {IHubIdentifiable.ColumnName} = @hubId
                                      AND {IUserIdentifiable.ColumnName} = @userId
                              )
                              """;
        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, parameters);
    }

    public async Task<bool> AddUserMembership(HubMembership hubMembership)
    {
        const string query = $"""
                              INSERT INTO {HubMembershipsTable.TableName} (
                                  {IHubIdentifiable.ColumnName},
                                  {IUserIdentifiable.ColumnName},
                                  {ICreated.ColumnName}
                              ) VALUES (
                                  @hubId,
                                  @userId,
                                  @created
                              );
                              """;
        using var connection = ConnectionFactory();
        return await connection.ExecuteAsync(query, hubMembership) > 0;
    }

    public async Task<Option<Hub>> GetAsync(GuidChecked id) => await GetAsyncInternal(id);
    public async Task<bool> ExistsAsync(GuidChecked id) => await ExistsAsyncInternal(id);
}