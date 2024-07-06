using MCH.Application.Common.Interfaces;
using MCH.Application.Common.Persistence;
using MCH.Infrastructure.Caching;
using MCH.Infrastructure.Common.Services;
using MCH.Infrastructure.Localization;
using MCH.Infrastructure.Persistence.ConnectionString;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Test;

public class Startup
{
    public static void ConfigureHost(IHostBuilder host) =>
        host.ConfigureHostConfiguration(config => config.AddJsonFile("appsettings.json"));

    public static void ConfigureServices(IServiceCollection services, HostBuilderContext context) =>
        services
            .AddTransient<IMemoryCache, MemoryCache>()
            .AddTransient<LocalCacheService>()
            .AddTransient<IDistributedCache, MemoryDistributedCache>()
            .AddTransient<ISerializerService, NewtonSoftService>()
            .AddTransient<DistributedCacheService>()

            .AddPOLocalization(context.Configuration)

            .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>();
}