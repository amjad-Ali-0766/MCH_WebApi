using MCH.Infrastructure.Multitenancy;

namespace MCH.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(MCHTenantInfo tenant, CancellationToken cancellationToken);
}