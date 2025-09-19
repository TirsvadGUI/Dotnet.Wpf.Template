using System.Reflection;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace Template.Infrastructure.Configuration;

public sealed class DpapiConnectionStringStore : IConnectionStringStore
{
    private readonly string _filePath;
    private readonly byte[] _entropy;

    public DpapiConnectionStringStore()
    {
        var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var appName = asm.GetName().Name ?? "TemplateApp";
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dir = Path.Combine(appData, appName, "secrets");
        Directory.CreateDirectory(dir);
        _filePath = Path.Combine(dir, "connectionString.dat");
        _entropy = Encoding.UTF8.GetBytes(appName + "|conn");
    }

    [SupportedOSPlatform("windows")]
    public async Task<string?> ReadAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_filePath)) return null;

        try
        {
            var cipher = await File.ReadAllBytesAsync(_filePath, ct).ConfigureAwait(false);
            var plain = ProtectedData.Unprotect(cipher, _entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(plain);
        }
        catch
        {
            // Corrupted or unreadable; treat as not present.
            return null;
        }
    }

    [SupportedOSPlatform("windows")]
    public async Task WriteAsync(string connectionString, CancellationToken ct = default)
    {
        var plain = Encoding.UTF8.GetBytes(connectionString);
        var cipher = ProtectedData.Protect(plain, _entropy, DataProtectionScope.CurrentUser);
        await File.WriteAllBytesAsync(_filePath, cipher, ct).ConfigureAwait(false);
    }
}