namespace Template.Infrastructure.Configuration;

// Implement this in the WPF project. It should return null if the user cancels.
public interface IConnectionStringPrompt
{
    Task<string?> PromptAsync(CancellationToken ct = default);
}