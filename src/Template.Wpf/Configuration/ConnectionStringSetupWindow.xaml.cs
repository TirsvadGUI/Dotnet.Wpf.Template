using System.Text;
using System.Windows;

namespace Template.Wpf.Configuration;

public partial class ConnectionStringSetupWindow : Window
{
    public string? ConnectionString { get; private set; }

    public ConnectionStringSetupWindow()
    {
        InitializeComponent();
    }

    private void OnOkClick(object sender, RoutedEventArgs e)
    {
        var server = ServerText.Text.Trim();
        var database = DatabaseText.Text.Trim();
        var encrypt = EncryptCheck.IsChecked == true;
        var trust = TrustServerCertCheck.IsChecked == true;

        if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(database))
        {
            MessageBox.Show(this, "Server and Database are required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var sb = new StringBuilder();
        sb.Append($"Server={server};Database={database};");
        sb.Append($"Encrypt={(encrypt ? "True" : "False")};");
        if (trust) sb.Append("Trust Server Certificate=True;");

        if (IntegratedRadio.IsChecked == true)
        {
            sb.Append("Integrated Security=True;");
        }
        else
        {
            var user = UserText.Text.Trim();
            var pwd = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show(this, "User and Password are required for SQL Authentication.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            sb.Append($"User ID={user};Password={pwd};Persist Security Info=False;");
        }

        ConnectionString = sb.ToString();
        DialogResult = true;
        Close();
    }
}
