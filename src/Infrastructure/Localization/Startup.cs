using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Localization;

namespace MCH.Infrastructure.Localization;

internal static class Startup
{
    internal static IServiceCollection AddPOLocalization(this IServiceCollection services, IConfiguration config)
    {
        var localizationSettings = config.GetSection(nameof(LocalizationSettings)).Get<LocalizationSettings>();

        if (localizationSettings?.EnableLocalization is true
            && localizationSettings.ResourcesPath is not null)
        {
            //---below line translated text for a specific language or culture.
            //--PO files store translations as objects, making it easy to manage
            services.AddPortableObjectLocalization(options => options.ResourcesPath = localizationSettings.ResourcesPath);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                if (localizationSettings.SupportedCultures != null)
                {
                    var supportedCultures = localizationSettings.SupportedCultures.Select(x => new CultureInfo(x)).ToList();
                    // --SupportedCultures use for convert dates, numbers, and currencies in local lang
                    options.SupportedCultures = supportedCultures;
                    //-- SupportedUICultures use for convert layout[UI] text,labels,messages,buttons in local lang
                    options.SupportedUICultures = supportedCultures;
                }

                options.DefaultRequestCulture = new RequestCulture(localizationSettings.DefaultRequestCulture ?? "en-US");
                options.FallBackToParentCultures = localizationSettings.FallbackToParent ?? true;
                //---ParentCultures mean "en" as a parent culture of "en-US"
                options.FallBackToParentUICultures = localizationSettings.FallbackToParent ?? true;
            });

            services.AddSingleton<ILocalizationFileLocationProvider, MCHPoFileLocationProvider>();
        }

        return services;
    }
}