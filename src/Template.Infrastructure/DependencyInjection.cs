using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Infrastructure.Configuration;

namespace Template.Infrastructure;

public static class DependencyInjection
{
    // Register services that can discover or collect the connection string on first run.
    public static IServiceCollection AddShelfMarketInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Make sure IConfiguration is available to downstream services
        services.AddSingleton(configuration);

        // Encrypted store (DPAPI, CurrentUser scope)
        services.AddSingleton<IConnectionStringStore, DpapiConnectionStringStore>();

        // Provider that resolves the connection string (config -> encrypted store -> prompt)
        services.AddSingleton<IConnectionStringProvider, EncryptedConnectionStringProvider>();

        // NOTE: The prompt itself is app-specific (WPF). Register IConnectionStringPrompt in the WPF project.
        return services;
    }
}
