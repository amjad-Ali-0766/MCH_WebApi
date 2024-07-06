using Finbuckle.MultiTenant.Stores;
using MCH.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MCH.Infrastructure.Multitenancy;

public class TenantDbContext : EFCoreStoreDbContext<MCHTenantInfo>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MCHTenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
    }
}