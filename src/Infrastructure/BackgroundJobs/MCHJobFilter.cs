using System.Security.Claims;
using Finbuckle.MultiTenant;
using MCH.Infrastructure.Common;
using MCH.Shared.Multitenancy;
using Hangfire.Client;
using Hangfire.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MCH.Infrastructure.BackgroundJobs;

//---IClientFilter is Hangfire filter that allows modifying job execution
//-ilter is likely used to inject tenant-specific and user-specific information into Hangfire jobs,
//making them multi-tenant aware and user-aware.
public class MCHJobFilter : IClientFilter
{
    private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

    private readonly IServiceProvider _services;

    //-resolve dependencies
    public MCHJobFilter(IServiceProvider services) => _services = services;

    //----before a job is created
    public void OnCreating(CreatingContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        Logger.InfoFormat("Set TenantId and UserId parameters to job {0}.{1}...", context.Job.Method.ReflectedType?.FullName, context.Job.Method.Name);

        using var scope = _services.CreateScope();

        var httpContext = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
        _ = httpContext ?? throw new InvalidOperationException("Can't create a TenantJob without HttpContext.");

        var tenantInfo = scope.ServiceProvider.GetRequiredService<ITenantInfo>();
        context.SetJobParameter(MultitenancyConstants.TenantIdName, tenantInfo);

        string? userId = httpContext.User.GetUserId();
        context.SetJobParameter(QueryStringKeys.UserId, userId);
    }

    public void OnCreated(CreatedContext context) =>
        Logger.InfoFormat(
            "Job created with parameters {0}",
            context.Parameters.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + ";" + s2));
}