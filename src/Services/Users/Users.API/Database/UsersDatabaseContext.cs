using Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Users.API.Models;

namespace Users.API.Database;

public class UsersDatabaseContext(DbContextOptions<UsersDatabaseContext> options) : DbContext(options)
{
    public const string SchemaName = "users_service";

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        modelBuilder.SetDefaultValues();

        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
    }
}