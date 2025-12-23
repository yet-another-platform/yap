using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hubs.API.Database;

public sealed class HubsDatabaseContextFactory
    : IDesignTimeDbContextFactory<HubsDatabaseContext>
{
    public HubsDatabaseContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            configuration.GetConnectionString("PostgreSQL")
            ?? throw new InvalidOperationException(
                "PostgreSQL connection string not found.");

        var options = new DbContextOptionsBuilder<HubsDatabaseContext>()
            .UseNpgsql(connectionString, x =>
            {
                x.MigrationsAssembly("Hubs.API");
                x.MigrationsHistoryTable(
                    "__MigrationsHistory",
                    HubsDatabaseContext.SchemaName);
            })
            .Options;

        return new HubsDatabaseContext(options);
    }
}