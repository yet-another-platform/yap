using Microsoft.EntityFrameworkCore;
using Types.Extensions;

namespace Hubs.API.Database;

public class HubsDatabaseContext(DbContextOptions<HubsDatabaseContext> options) : DbContext(options)
{
    public const string SchemaName = "hubs_service";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        modelBuilder.SetDefaultValues();
    }
}