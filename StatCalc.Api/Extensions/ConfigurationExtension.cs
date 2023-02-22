using StatCalc.Infrastructure.Configurations;

namespace StatCalc.Api.Extensions;

public static class ConfigurationExtension
{
    public static void ConfigurationsSetUp(this IServiceCollection service, IConfiguration configuration)
    {
        var googleSection = configuration.GetSection(GoogleSettings.SectionName);
        service.Configure<GoogleSettings>(googleSection);
    }
}
