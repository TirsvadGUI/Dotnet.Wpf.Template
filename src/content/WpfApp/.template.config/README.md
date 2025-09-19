# WPF Application Template

This template creates a basic WPF (Windows Presentation Foundation) application with customizable settings.

## Features

- Configurable target framework (.NET 6.0, 7.0, or 8.0 Windows)
- Optional nullable reference types
- Optional implicit usings
- Basic MainWindow with sample content
- Proper WPF project structure

## Usage

```bash
dotnet new wpfapp -n MyWpfApp
```

## Parameters

- `--Framework`: Target framework (net6.0-windows, net7.0-windows, net8.0-windows)
- `--EnableNullable`: Enable nullable reference types (true/false)
- `--EnableImplicitUsings`: Enable implicit usings (true/false)