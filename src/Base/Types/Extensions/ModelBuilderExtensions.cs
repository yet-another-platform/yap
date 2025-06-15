using Microsoft.EntityFrameworkCore;
using Types.Enums;
using Types.Interfaces.Model;

namespace Types.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder SetDefaultValues(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Select(x => x.ClrType))
        {
            if (typeof(IIdentifiable).IsAssignableFrom(entityType))
            {
                modelBuilder.Entity(entityType).HasIndex(nameof(IIdentifiable.Id)).IsUnique();
                modelBuilder.Entity(entityType).Property<Guid>(nameof(IIdentifiable.Id))
                    .HasDefaultValueSql("gen_random_uuid()");
            }

            if (typeof(ICreated).IsAssignableFrom(entityType))
            {
                modelBuilder.Entity(entityType).Property<DateTimeOffset>(nameof(ICreated.Created))
                    .HasDefaultValueSql("now()");
            }

            if (typeof(IUpdated).IsAssignableFrom(entityType))
            {
                modelBuilder.Entity(entityType).Property<DateTimeOffset?>(nameof(IUpdated.Updated))
                    .HasDefaultValueSql(null);
            }

            if (typeof(IDeleted).IsAssignableFrom(entityType))
            {
                modelBuilder.Entity(entityType).Property<bool>(nameof(IDeleted.Deleted))
                    .HasDefaultValue(false);
            }
            
            if (typeof(IDeletedReason).IsAssignableFrom(entityType))
            {
                modelBuilder.Entity(entityType).Property<DeletedReason>(nameof(IDeletedReason.DeletedReason))
                    .HasDefaultValue(DeletedReason.Unknown);
            }
        }

        return modelBuilder;
    }
}