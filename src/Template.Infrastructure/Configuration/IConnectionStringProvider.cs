namespace Template.Infrastructure.Configuration;

public interface IConnectionStringProvider
{
    // Ensures a connection string exists (may show a prompt once), then returns it.
    ValueTask<string> GetAsync(CancellationToken ct = default);

    // Proactively ensure availability (use in app startup so EF or others won’t trigger UI during DI).
    ValueTask EnsureAvailableAsync(CancellationToken ct = default);
}