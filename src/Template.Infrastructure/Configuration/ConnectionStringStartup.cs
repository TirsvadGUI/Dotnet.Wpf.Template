using Microsoft.Extensions.DependencyInjection;

namespace Template.Infrastructure.Configuration;

public static class ConnectionStringStartup
{
    public static async Task EnsureConnectionStringAsync(IServiceProvider services, CancellationToken ct = default)
    {
        var provider = services.GetRequiredService<IConnectionStringProvider>();
        await provider.EnsureAvailableAsync(ct).ConfigureAwait(false);
    }
}