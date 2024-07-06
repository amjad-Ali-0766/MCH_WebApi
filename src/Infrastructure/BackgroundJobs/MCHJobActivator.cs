using Finbuckle.MultiTenant;
using MCH.Infrastructure.Auth;
using MCH.Infrastructure.Common;
using MCH.Infrastructure.Multitenancy;
using MCH.Shared.Multitenancy;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;

namespace MCH.Infrastructure.BackgroundJobs;

// ----This below class "MCHJobActivator" code is in charge of creating, managing, and disposing of job instances,
// making sure they are properly set up and cleaned up when they're done.
public class MCHJobActivator : JobActivator
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MCHJobActivator(IServiceScopeFactory scopeFactory) =>
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    // below function---a fresh, isolated environment for a specific job to run in.
    // Think of it like a clean room for each job
    public override JobActivatorScope BeginScope(PerformContext context) =>
        new Scope(context, _scopeFactory.CreateScope());

    
    private class Scope : JobActivatorScope, IServiceProvider
    {
        //PerformContext represents the current job execution and have current job information
        private readonly PerformContext _context;
        //"IServiceScope" is an object that represents the parent scope.
        //A scope is like a container that holds a set of services (dependencies) that can be used by the job.

        private readonly IServiceScope _scope;

        public Scope(PerformContext context, IServiceScope scope)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));

            ReceiveParameters();
        }
        // ---it retrieves the tenant info and user ID from the job parameters

        private void ReceiveParameters()
        {
            var tenantInfo = _context.GetJobParameter<MCHTenantInfo>(MultitenancyConstants.TenantIdName);
            if (tenantInfo is not null)
            {
                //---sets the tenant info on the IMultiTenantContextAccessor service.
                _scope.ServiceProvider.GetRequiredService<IMultiTenantContextAccessor>()
                    .MultiTenantContext = new MultiTenantContext<MCHTenantInfo>
                    {
                        TenantInfo = tenantInfo
                    };
            }
            //---sets the user ID on the ICurrentUserInitializer service.
            string userId = _context.GetJobParameter<string>(QueryStringKeys.UserId);
            if (!string.IsNullOrEmpty(userId))
            {
                _scope.ServiceProvider.GetRequiredService<ICurrentUserInitializer>()
                    .SetCurrentUserId(userId);
            }
        }
        //-- resolve dependencies(services) for the job
        public override object Resolve(Type type) =>
            ActivatorUtilities.GetServiceOrCreateInstance(this, type);


        //--If the type is PerformContext,then return "current _context" Otherwise return "service instance"
        object? IServiceProvider.GetService(Type serviceType) =>
            serviceType == typeof(PerformContext)
                ? _context
                : _scope.ServiceProvider.GetService(serviceType);
    }
}