using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Template.Infrastructure;
using Template.Infrastructure.Configuration;
using Template.Wpf.Configuration;
using Template.Wpf.ViewModels;

namespace Template.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private IHost? _host;
    public ServiceProvider? ServiceProvider { get; private set; }
    public IConfiguration Configuration { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables();
            })
            .ConfigureServices((ctx, services) =>
            {
                // Infrastructure + connection string pipeline
                services.AddShelfMarketInfrastructure(ctx.Configuration);
                services.AddSingleton<IConnectionStringPrompt, UiConnectionStringPrompt>();

                // ViewModels
                services.AddTransient<MainViewModel>();

                // Views
                services.AddTransient<MainWindow>();
            })
            .Build();

        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow = mainWindow;
        mainWindow.Show();

        // Ensure connection string first; shows ConnectionStringSetupWindow if missing
        await ConnectionStringStartup.EnsureConnectionStringAsync(_host.Services);

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
            await _host.StopAsync(TimeSpan.FromSeconds(5));
        _host?.Dispose();
        base.OnExit(e);
    }
}
