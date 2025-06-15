using System.Data;
using Dapper;
using Types.Extensions;
using Types.Interfaces.Model;

namespace Database;

public abstract class DatabaseServiceBase<T>(Func<IDbConnection> connectionFactory, string tableName) where T : class
{
    protected Func<IDbConnection> ConnectionFactory { get; } = connectionFactory;
    private static readonly bool IsSoftDeletable = typeof(T).IsSoftDeletable();

    protected async Task<T> GetWithDeletedAsyncInternal(Guid id)
    {
        string query =
            $"""
                 SELECT * FROM app_talk.{tableName}
                     WHERE {IIdentifiable.ColumnName} = @id;
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<T>(query, new { id });
    }

    protected async Task<T?> GetAsyncInternal(Guid id)
    {
        string query;
        if (IsSoftDeletable)
        {
            query =
                $"""
                     SELECT * FROM app_talk.{tableName}
                         WHERE {IIdentifiable.ColumnName} = @id
                            AND {IDeleted.ColumnName} = false;
                 """;
        }
        else
        {
            query =
                $"""
                     SELECT * FROM app_talk.{tableName}
                         WHERE {IIdentifiable.ColumnName} = @id;
                 """;
        }

        using var connection = ConnectionFactory();
        return await connection.QuerySingleOrDefaultAsync<T>(query, new { id });
    }

    protected async Task<bool> DeleteAsyncInteral(Guid id)
    {
        string query = IsSoftDeletable ? $"""
                                       UPDATE app_talk.{tableName}
                                           SET
                                              {IDeleted.ColumnName} = true,
                                              {IUpdated.ColumnName} = now()
                                           WHERE {IIdentifiable.ColumnName} = @id
                                       RETURNING *;
                                   """ : $"""
                                              DELETE FROM app_talk.{tableName}
                                                  WHERE {IIdentifiable.ColumnName} = @id;
                                          """;

        using var connection = ConnectionFactory();
        return await connection.ExecuteAsync(query, new { id }) > 0;
    }

    protected async Task<bool> ExistsWithDeletedAsyncInternal(Guid id)
    {
        string query =
            $"""
                 SELECT EXISTS (
                     SELECT 1 FROM app_talk.{tableName}
                         WHERE {IIdentifiable.ColumnName} = @id
                 );
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, new { id });
    }

    protected async Task<bool> ExistsAsyncInternal(Guid id)
    {
        string query = IsSoftDeletable ? $"""
                                       SELECT EXISTS (
                                           SELECT 1 FROM {tableName}
                                               WHERE {IIdentifiable.ColumnName} = @id
                                                  AND {IDeleted.ColumnName} = false;
                                       );
                                   """ : $"""
                                              SELECT EXISTS (
                                                  SELECT 1 FROM {tableName}
                                                      WHERE {IIdentifiable.ColumnName} = @id
                                              );
                                          """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, new { id });
    }
    
    protected async Task<bool> UpdateAsyncInternal(string query, T model)
    {
        if (model is IUpdated updated)
        {
            updated.Updated = DateTimeOffset.UtcNow;
        }

        using var connection = ConnectionFactory();
        return await connection.ExecuteAsync(query, model) > 0;
    }

    protected async Task<Guid> CreateAsyncInternal(string query, T model)
    {
        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<Guid>(query, model);
    }
}