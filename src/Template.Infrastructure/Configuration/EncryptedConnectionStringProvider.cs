using Microsoft.Extensions.Configuration;

namespace Template.Infrastructure.Configuration;

public sealed class EncryptedConnectionStringProvider : IConnectionStringProvider
{
    private readonly IConfiguration _configuration;
    private readonly IConnectionStringStore _store;
    private readonly IConnectionStringPrompt? _prompt;
    private string? _cached;

    public EncryptedConnectionStringProvider(
        IConfiguration configuration,
        IConnectionStringStore store,
        IConnectionStringPrompt? prompt = null) // optional; provided by WPF project
    {
        _configuration = configuration;
        _store = store;
        _prompt = prompt;
    }

    public async ValueTask EnsureAvailableAsync(CancellationToken ct = default)
    {
        _ = await GetAsync(ct).ConfigureAwait(false);
    }

    public async ValueTask<string> GetAsync(CancellationToken ct = default)
    {
        // 1) Configuration (appsettings, env var, user secrets, etc.)
        _cached ??= _configuration.GetConnectionString("Default");
        if (!string.IsNullOrWhiteSpace(_cached))
            return _cached!;

        // 2) Encrypted store
        _cached = await _store.ReadAsync(ct).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(_cached))
            return _cached!;

        // 3) Prompt (if available)
        if (_prompt is not null)
        {
            var entered = await _prompt.PromptAsync(ct).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(entered))
            {
                _cached = entered!;
                // Persist encrypted for next runs
                await _store.WriteAsync(_cached, ct).ConfigureAwait(false);
                return _cached!;
            }
        }

        throw new InvalidOperationException("No connection string was provided. Supply it via configuration or complete the prompt.");
    }
}