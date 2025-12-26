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

public class ChannelDatabaseService(Func<IDbConnection> connectionFactory)
    : DatabaseServiceBase<Channel>(connectionFactory, ChannelsTable.TableName), IChannelDatabaseService
{
    public async Task<Guid> CreateAsync(Channel entity)
    {
        const string query = $"""
                              INSERT INTO {ChannelsTable.TableName} (
                                  {IHubIdentifiable.ColumnName},
                                  {IName.ColumnName},
                                  {IDescription.ColumnName},
                                  {ICreated.ColumnName})
                              VALUES (
                                  @hubId,
                                  @name,
                                  @description,
                                  @created
                              )
                              RETURNING {IIdentifiable.ColumnName};
                              """;

        return await CreateAsyncInternal(query, entity);
    }

    public async Task<bool> UpdateAsync(Channel entity)
    {
        const string query = $"""
                              UPDATE {ChannelsTable.TableName} SET
                                  {IName.ColumnName} = @name,
                                  {IDescription.ColumnName} = @description,
                                  {IUpdated.ColumnName} = @updated
                              WHERE {IIdentifiable.ColumnName} = @id
                              RETURNING *;
                              """;

        return await UpdateAsyncInternal(query, entity);
    }

    public async Task<Option<Channel>> GetAsync(GuidChecked id) => await GetAsyncInternal(id);

    public async Task<List<Channel>> ListForHub(GuidChecked hubId)
    {
        var parameters = new { hubId }.ToDynamicParameters();
        const string query = $"""
                              SELECT * FROM {ChannelsTable.TableName}
                              WHERE {IHubIdentifiable.ColumnName} = @hubId;
                              """;

        using var connection = ConnectionFactory();
        var result = await connection.QueryAsync<Channel>(query, parameters);
        return result.ToList();
    }

    public async Task<List<Channel>> ListForHubAndUser(GuidChecked hubId, GuidChecked userId)
    {
        var parameters = new { hubId, userId }.ToDynamicParameters();
        const string query = $"""
                              SELECT {ChannelsTable.Prefix}.* FROM {ChannelsTable.TableName} AS {ChannelsTable.Prefix}
                                  JOIN {ChannelMembershipsTable.TableName} AS {ChannelMembershipsTable.Prefix}
                                       ON {ChannelMembershipsTable.Prefix}.{IChannelIdentifiable.ColumnName} = {ChannelsTable.Prefix}.{IIdentifiable.ColumnName}
                              WHERE {ChannelsTable.Prefix}.{IHubIdentifiable.ColumnName} = @hubId
                                  AND {ChannelMembershipsTable.Prefix}.{IUserIdentifiable.ColumnName} = @userId;
                              """;

        using var connection = ConnectionFactory();
        var result = await connection.QueryAsync<Channel>(query, parameters);
        return result.ToList();
    }

    public async Task<List<Channel>> ListForHubsAndUser(List<GuidChecked> hubIds, GuidChecked userId)
    {
        var parameters = new { hubIds = hubIds.Select(id => id.Value).ToList(), userId }.ToDynamicParameters();
        const string query = $"""
                              SELECT {ChannelsTable.Prefix}.* FROM {ChannelsTable.TableName} AS {ChannelsTable.Prefix}
                                  JOIN {ChannelMembershipsTable.TableName} AS {ChannelMembershipsTable.Prefix}
                                       ON {ChannelMembershipsTable.Prefix}.{IChannelIdentifiable.ColumnName} = {ChannelsTable.Prefix}.{IIdentifiable.ColumnName}
                              WHERE {ChannelsTable.Prefix}.{IHubIdentifiable.ColumnName} = ANY(@hubIds)
                                  AND {ChannelMembershipsTable.Prefix}.{IUserIdentifiable.ColumnName} = @userId;
                              """;

        using var connection = ConnectionFactory();
        var result = await connection.QueryAsync<Channel>(query, parameters);
        return result.ToList();
    }

    public async Task<bool> IsUserMember(GuidChecked channelId, GuidChecked userId)
    {
        var parameters = new { channelId, userId }.ToDynamicParameters();
        const string query = $"""
                              SELECT EXISTS (
                                  SELECT 1 FROM {ChannelMembershipsTable.TableName}
                                  WHERE {IChannelIdentifiable.ColumnName} = @channelId
                                      AND {IUserIdentifiable.ColumnName} = @userId
                              );
                              """;
        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, parameters);
    }

    public async Task<bool> ExistsAsync(GuidChecked id) => await ExistsAsyncInternal(id);
}