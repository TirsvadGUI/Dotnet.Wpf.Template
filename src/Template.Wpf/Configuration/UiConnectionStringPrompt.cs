using Template.Infrastructure.Configuration;

namespace Template.Wpf.Configuration;

public sealed class UiConnectionStringPrompt : IConnectionStringPrompt
{
    public Task<string?> PromptAsync(CancellationToken ct = default)
    {
        // Ensure we open the dialog on the UI thread
        if (System.Windows.Application.Current?.Dispatcher?.CheckAccess() == true)
        {
            return Task.FromResult(ShowDialog());
        }

        return System.Windows.Application.Current!.Dispatcher.InvokeAsync(ShowDialog).Task;

        static string? ShowDialog()
        {
            var win = new ConnectionStringSetupWindow
            {
                Owner = System.Windows.Application.Current?.MainWindow
            };
            var ok = win.ShowDialog() == true;
            return ok ? win.ConnectionString : null;
        }
    }
}