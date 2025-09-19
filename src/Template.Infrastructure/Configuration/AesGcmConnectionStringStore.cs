using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Template.Infrastructure.Configuration;

public sealed class AesGcmConnectionStringStore : IConnectionStringStore
{
    private const int KeySize = 32;      // 256-bit
    private const int NonceSize = 12;    // 96-bit (recommended for GCM)
    private const int TagSize = 16;      // 128-bit tag
    private const byte Version = 1;

    private readonly string _dir;
    private readonly string _filePath;
    private readonly string _keyFilePath;
    private readonly byte[] _entropy;

    public AesGcmConnectionStringStore()
    {
        var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var appName = asm.GetName().Name ?? "TemplateApp";
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _dir = Path.Combine(appData, appName, "secrets");
        Directory.CreateDirectory(_dir);
        _filePath = Path.Combine(_dir, "connectionString.dat");
        _keyFilePath = Path.Combine(_dir, "key.bin");
        _entropy = Encoding.UTF8.GetBytes(appName + "|conn");
    }

    public async Task<string?> ReadAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_filePath)) return null;

        byte[] key = await GetOrCreateKeyAsync(ct).ConfigureAwait(false);
        byte[] data = await File.ReadAllBytesAsync(_filePath, ct).ConfigureAwait(false);
        if (data.Length < 1 + NonceSize + TagSize) return null;
        if (data[0] != Version) return null;

        var nonce = data.AsSpan(1, NonceSize);
        var tag = data.AsSpan(1 + NonceSize, TagSize);
        var cipher = data.AsSpan(1 + NonceSize + TagSize);

        var plain = new byte[cipher.Length];
        try
        {
            using var aes = new AesGcm(key, TagSize);
            aes.Decrypt(nonce, cipher, tag, plain, _entropy);
            return Encoding.UTF8.GetString(plain);
        }
        catch
        {
            return null;
        }
        finally
        {
            CryptographicOperations.ZeroMemory(plain);
        }
    }

    public async Task WriteAsync(string connectionString, CancellationToken ct = default)
    {
        byte[] key = await GetOrCreateKeyAsync(ct).ConfigureAwait(false);
        var plain = Encoding.UTF8.GetBytes(connectionString);

        var nonce = new byte[NonceSize];
        RandomNumberGenerator.Fill(nonce);

        var cipher = new byte[plain.Length];
        var tag = new byte[TagSize];

        using (var aes = new AesGcm(key, TagSize))
        {
            aes.Encrypt(nonce, plain, cipher, tag, _entropy);
        }

        var output = new byte[1 + NonceSize + TagSize + cipher.Length];
        output[0] = Version;
        Buffer.BlockCopy(nonce, 0, output, 1, NonceSize);
        Buffer.BlockCopy(tag, 0, output, 1 + NonceSize, TagSize);
        Buffer.BlockCopy(cipher, 0, output, 1 + NonceSize + TagSize, cipher.Length);

        await File.WriteAllBytesAsync(_filePath, output, ct).ConfigureAwait(false);

        CryptographicOperations.ZeroMemory(plain);
        CryptographicOperations.ZeroMemory(cipher);
        CryptographicOperations.ZeroMemory(tag);
        CryptographicOperations.ZeroMemory(nonce);
    }

    private async Task<byte[]> GetOrCreateKeyAsync(CancellationToken ct)
    {
        if (File.Exists(_keyFilePath))
            return await File.ReadAllBytesAsync(_keyFilePath, ct).ConfigureAwait(false);

        var key = new byte[KeySize];
        RandomNumberGenerator.Fill(key);

        await File.WriteAllBytesAsync(_keyFilePath, key, ct).ConfigureAwait(false);

        try
        {
            if (!OperatingSystem.IsWindows())
            {
                // Restrict to rw------- on Unix-like systems
                File.SetUnixFileMode(_keyFilePath, System.IO.UnixFileMode.UserRead | System.IO.UnixFileMode.UserWrite);
            }
        }
        catch
        {
            // Best-effort; ignore if not supported.
        }

        return key;
    }
}