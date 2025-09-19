# Dotnet.Wpf.Template

A .NET template for creating WPF (Windows Presentation Foundation) applications with customizable framework versions and project settings.

## Installation

Install the template from NuGet:

```bash
dotnet new install TirsvadGUI.WpfApp.Template
```

Or install from local source:

```bash
dotnet new install ./src/content/WpfApp
```

## Usage

After installation, you can create new WPF applications using:

```bash
dotnet new wpfapp -n MyWpfApplication
```

### Template Parameters

The template supports the following parameters:

- `--Framework` (default: `net8.0-windows`): The target framework for the project
  - `net8.0-windows`: Target .NET 8.0 (Windows)
  - `net7.0-windows`: Target .NET 7.0 (Windows)  
  - `net6.0-windows`: Target .NET 6.0 (Windows)

- `--EnableNullable` (default: `true`): Whether to enable nullable reference types
- `--EnableImplicitUsings` (default: `true`): Whether to enable implicit usings

### Examples

Create a WPF app targeting .NET 6.0 with nullable disabled:

```bash
dotnet new wpfapp -n MyApp --Framework net6.0-windows --EnableNullable false
```

Create a WPF app with minimal features:

```bash
dotnet new wpfapp -n MinimalApp --EnableNullable false --EnableImplicitUsings false
```

## Generated Project Structure

The template creates a basic WPF application with:

- `App.xaml` and `App.xaml.cs` - Application entry point
- `MainWindow.xaml` and `MainWindow.xaml.cs` - Main application window
- Project file with appropriate WPF settings and target framework

## Requirements

- .NET SDK 6.0 or later
- Windows operating system (for building and running WPF applications)

## Uninstallation

To remove the template:

```bash
dotnet new uninstall TirsvadGUI.WpfApp.Template
```

## Contributing

This template is part of the TirsvadGUI project. For issues and contributions, please visit the [GitHub repository](https://github.com/TirsvadGUI/Dotnet.Wpf.Template).

## License

This project is licensed under the GNU Affero General Public License v3.0. See the LICENSE file for details.