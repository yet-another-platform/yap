using System.Data;
using Database;
using Hubs.API.Constants.Database;
using Hubs.API.DatabaseServices.Interfaces;
using Hubs.API.Models;
using Types.Interfaces.Model;
using Types.Types.Option;

namespace Hubs.API.DatabaseServices;

public class MessageDatabaseService(Func<IDbConnection> connectionFactory) : DatabaseServiceBase<long, Message>(connectionFactory, MessagesTable.TableName), IMessageDatabaseService
{
    public async Task<long> CreateAsync(Message entity)
    {
        const string query = $"""
                              INSERT INTO {MessagesTable.TableName} (
                                  {IChannelIdentifiable.ColumnName},
                                  {IUserIdentifiable.ColumnName},
                                  {MessagesTable.Content},
                                  {ICreated.ColumnName})
                              VALUES (
                                  @channelId,
                                  @userId,
                                  @content,
                                  @created
                              )
                              RETURNING {IIdentifiable.ColumnName};
                              """;

        return await CreateAsyncInternal(query, entity);
    }

    public async Task<Option<Message>> GetAsync(long id) => await GetAsyncInternal(id);

    public async Task<bool> UpdateAsync(Message entity)
    {
        const string query = $"""
                              UPDATE {MessagesTable.TableName} SET
                                  {MessagesTable.Content} = @content,
                                  {IUpdated.ColumnName} = @updated
                              WHERE {IIdentifiable.ColumnName} = @id
                              RETURNING *;
                              """;

        return await UpdateAsyncInternal(query, entity);
    }

    public async Task<bool> ExistsAsync(long id) => await ExistsAsyncInternal(id);
}