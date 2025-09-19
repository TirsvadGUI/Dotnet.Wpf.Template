namespace Template.Infrastructure.Configuration;

public interface IConnectionStringStore
{
    Task<string?> ReadAsync(CancellationToken ct = default);
    Task WriteAsync(string connectionString, CancellationToken ct = default);
}