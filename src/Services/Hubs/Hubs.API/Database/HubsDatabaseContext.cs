using Database.Extensions;
using Hubs.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hubs.API.Database;

public class HubsDatabaseContext(DbContextOptions<HubsDatabaseContext> options) : DbContext(options)
{
    public const string SchemaName = "hubs_service";

    public DbSet<Hub> Hubs { get; set; }
    public DbSet<HubMembership> HubMemberships { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<ChannelMembership> ChannelMemberships { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        modelBuilder.SetDefaultValues();

        modelBuilder.Entity<HubMembership>().HasOne<Hub>().WithMany().HasForeignKey(x => x.HubId);
        modelBuilder.Entity<HubMembership>().HasIndex(hm => new { hm.HubId, hm.UserId }).IsUnique();
        
        modelBuilder.Entity<Channel>().HasOne<Hub>().WithMany().HasForeignKey(x => x.HubId);
        
        modelBuilder.Entity<ChannelMembership>().HasOne<Channel>().WithMany().HasForeignKey(x => x.ChannelId);
        modelBuilder.Entity<ChannelMembership>().HasIndex(cm => new { cm.ChannelId, cm.UserId }).IsUnique();
        
        modelBuilder.Entity<Message>().HasOne<Channel>().WithMany().HasForeignKey(x => x.ChannelId);
    }
}