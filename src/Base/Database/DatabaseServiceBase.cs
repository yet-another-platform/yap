using System.Data;
using Dapper;
using Types.Extensions;
using Types.Interfaces.Model;
using Types.Types.Option;

namespace Database;

public abstract class DatabaseServiceBase<TId, T>(Func<IDbConnection> connectionFactory, string tableName) where T : class, IIdentifiable<TId>
{
    protected Func<IDbConnection> ConnectionFactory { get; } = connectionFactory;
    private static readonly bool IsSoftDeletable = typeof(T).IsSoftDeletable();

    protected async Task<T> GetWithDeletedAsyncInternal(TId id)
    {
        string query =
            $"""
                 SELECT * FROM {tableName}
                     WHERE {IIdentifiable.ColumnName} = @id;
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<T>(query, new { id });
    }

    protected async Task<Option<T>> GetAsyncInternal(TId id)
    {
        string query;
        if (IsSoftDeletable)
        {
            query =
                $"""
                     SELECT * FROM {tableName}
                         WHERE {IIdentifiable.ColumnName} = @id
                            AND {IDeleted.ColumnName} = false;
                 """;
        }
        else
        {
            query =
                $"""
                     SELECT * FROM {tableName}
                         WHERE {IIdentifiable.ColumnName} = @id;
                 """;
        }

        using var connection = ConnectionFactory();
        var result = await connection.QuerySingleOrDefaultAsync<T>(query, new { id });
        return CreateOptionResult(result);
    }

    protected async Task<bool> DeleteAsyncInternal(TId id)
    {
        string query = IsSoftDeletable
            ? $"""
                   UPDATE {tableName}
                       SET
                          {IDeleted.ColumnName} = true,
                          {IUpdated.ColumnName} = now()
                       WHERE {IIdentifiable.ColumnName} = @id
                   RETURNING *;
               """
            : $"""
                   DELETE FROM {tableName}
                       WHERE {IIdentifiable.ColumnName} = @id;
               """;

        using var connection = ConnectionFactory();
        return await connection.ExecuteAsync(query, new { id }) > 0;
    }

    protected async Task<bool> ExistsWithDeletedAsyncInternal(TId id)
    {
        string query =
            $"""
                 SELECT EXISTS (
                     SELECT 1 FROM {tableName}
                         WHERE {IIdentifiable.ColumnName} = @id
                 );
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, new { id });
    }

    protected async Task<bool> ExistsAsyncInternal(TId id)
    {
        string query = IsSoftDeletable
            ? $"""
                   SELECT EXISTS (
                       SELECT 1 FROM {tableName}
                           WHERE {IIdentifiable.ColumnName} = @id
                              AND {IDeleted.ColumnName} = false
                   );
               """
            : $"""
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

    protected async Task<TId> CreateAsyncInternal(string query, T model)
    {
        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<TId>(query, model);
    }

    protected Option<T> CreateOptionResult(T? value)
    {
        return value is not null ? new Option<T>(value) : new Option<T>(new Error { Message = "Not found", Type = ErrorType.NotFound });
    }
}

public abstract class DatabaseServiceBase<T>(Func<IDbConnection> connectionFactory, string tableName)
    : DatabaseServiceBase<Guid, T>(connectionFactory, tableName) where T : class, IIdentifiable<Guid>;