﻿[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

# ![Logo][logo] Dotnet.Wpf.Template

A clean WPF (.NET 9) starter focused on:
- Separation of concerns
- Dark and light themes
- Dependency Injection (DI)
- Connection strings saved encrypted. Auto popup for first time run or missing configuration.

## Table of Contents
- [About the project](#about-the-project)
- [Goals](#goals)
- [Prerequisites](#prerequisites)
- [Features](#features)
- [Quick start](#quick-start)
- [Project structure (suggested)](#project-structure-suggested)
- [Notes](#notes)
- [License](#license)
- [Contact](#contact)
- [Acknowledgements](#acknowledgements)
- [Contributing](#contributing)

## About the project
This project is a template for building WPF applications using .NET 9.
It incorporates best practices such as the MVVM pattern, dependency injection, and theme management. The template is designed to help developers get started quickly while maintaining a clean and organized codebase.

## Goals
- Provide a solid foundation for building WPF applications
- Promote best practices in software architecture
- Enable easy customization and extension

## Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio, Visual Studio Code or other C# ide with WPF support

## Features
- WPF (.NET 9)
- MVVM pattern
- Dependency Injection (DI) with Microsoft.Extensions.DependencyInjection
- Light and dark themes with easy switching
- Connection string saved encrypted in user AppData folder
- Popup windows for first time run or missing configuration
- Project structure keeping UI, domain, and infrastructure clearly separated

## Quick start
### 1. Clone and open in Visual Studio 2022.  
`git clone [repo-url]`

## Project structure (suggested)
Legend for levels and items:
- 🗂️ Level 1: repository root
- 📁 Level 2: top-level folder
- 📂 Level 3: subfolder
- 🗃️ Level 4+: deeper subfolders
- 🧩 Project (csproj)
- 📄 File
- 🎨 Theme resources
- ⚙️ Configuration
- 🧪 Tests

```text
🗂️ Dotnet.Wpf.Template
├─ 📁 src
│  ├─ 🧩 Template.App (WPF UI)
│  │  ├─ 📂 Configuration (Popup windows for first time run or missing configuration)
│  │  ├─ 📂 Views (MVVM)
│  │  ├─ 📂 ViewModels (MVVM)
│  │  ├─ 🎨 Themes
│  │  │  ├─ 📄 Theme.Light.xaml
│  │  │  └─ 📄 Theme.Dark.xaml
│  │  ├─ 📂 Resources (styles, converters, controls)
│  │  ├─ 📄 appsettings.json (optional, not for secrets)
│  │  ├─ 📄 appsettings.Development.json (optional, not for secrets)
│  │  ├─ 📄 App.xaml 
│  │  └─ 📄 App.xaml.cs (application bootstrap - Generic Host + DI)
│  ├─ 🧩 Template.Infrastructure (integrations)
│  │  ├─ 📂 Configuration (Read / Save Connection string)
│  │  ├─ 📂 Data (clients, context, mappers)
│  │  ├─ 📂 Repositories (data access)
│  │  ├─ 📂 Services (APIs, email, logging)
│  │  └─ 📄 DependencyInjection.cs
│  ├─ 🧩 Template.Application (use cases)
│  │  ├─ 📂 Abstractions (interfaces)
│  │  ├─ 📂 DTOs (data transfer objects)
│  │  ├─ 📂 Services (business logic)
│  │  └─ 📄 DependencyInjection.cs
│  └─ 🧩 Template.Domain (core models)
│     ├─ 📂 Models
│     └─ 📂 Abstractions (interfaces)
├─ 🧪 tests (optional)
│  └─ 🧩 Template.App.Tests
├─ 📄 README.md
└─ 📄 LICENSE.txt
```

- `App.xaml`, `App.xaml.cs` – application bootstrap (Generic Host + DI)
- `Views/`, `ViewModels/` – presentation (MVVM)
- `Infrastructure/` – external services (data access, APIs)
- `Themes/Theme.Light.xaml`, `Themes/Theme.Dark.xaml` – theme resources
- `appsettings.json` (optional, not for secrets)
- User secrets or environment variables for secrets

This keeps UI, domain, and infrastructure clearly separated.

## How it works

### Dependency Injection (DI)
In `App.xaml.cs` we build a Host and register services
- DI constructs `MainWindow` and can inject dependencies (like `MainViewModel`) into its constructor.

### MVVM in a nutshell
- View (`MainWindow.xaml`): defines the UI in XAML.
- ViewModel (`MainViewModel`): holds the data and commands (no UI code).
- Binding connects them (e.g., `{Binding Title}` displays a `Title` property from the ViewModel).

This keeps UI and logic separate and testable.

### Connection string flow
`Template.Infrastructure.Configuration.EncryptedConnectionStringProvider` tries:
1. Configuration (appsettings, env vars, or User Secrets)
2. Encrypted store (saved from a previous run)
3. UI prompt (popup) if still missing

If the popup is used and you click OK, it saves the encrypted connection string so you don’t have to enter it again.

### Themes (Light/Dark)
To use a theme, merge it in `App.xaml`:
```xml
<Application ...>
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- Pick one -->
                    <ResourceDictionary Source="Themes/Theme.Light.xaml" /> 
                <!-- or --> 
                    <!-- <ResourceDictionary Source="Themes/Theme.Dark.xaml" /> --> 
            </ResourceDictionary.MergedDictionaries> 
        </ResourceDictionary> 
    </Application.Resources> 
</Application>
```

## Notes
- Separation of concerns: keep UI (Views), state/logic (ViewModels), core logic (Domain), and integrations (Infrastructure) decoupled and wired via DI.
- Keep secrets out of source control; prefer environment variables or User Secrets.

## Common tasks
- Change window title: open `MainWindow.xaml` and set `Title="Your App"`.
- Add a service: register it in `App.xaml.cs` via `services.AddSingleton<IMyService, MyService>();`, then inject into constructors.
- Add a new View + ViewModel: create files in `Views/` and `ViewModels/`, register the View in DI if you want to resolve it from the container.


## License


## Contact
- Issues: [GitHub Issues][issues-url]
- LinkedIn: [Profile][linkedin-url]

## Acknowledgements
- .NET team and WPF community
- MVVM patterns and resources shared by the community

## Contributing
Contributions are welcome!  
- Fork the repo
- Create a feature branch
- Commit changes
- Open a Pull Request

<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/TirsvadGUI/Dotnet.Wpf.Template?style=for-the-badge
[contributors-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Template/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/TirsvadGUI/Dotnet.Wpf.Template?style=for-the-badge
[forks-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Template/network/members
[stars-shield]: https://img.shields.io/github/stars/TirsvadGUI/Dotnet.Wpf.Template?style=for-the-badge
[stars-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Template/stargazers
[issues-shield]: https://img.shields.io/github/issues/TirsvadGUI/Dotnet.Wpf.Template?style=for-the-badge
[issues-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Template/issues
[license-shield]: https://img.shields.io/github/license/TirsvadGUI/Dotnet.Wpf.Template?style=for-the-badge
[license-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Template/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/jens-TirsvadGUI-nielsen-13b795b9/
[githubIssue-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Template/issues/
[repo-size-shield]: https://img.shields.io/github/repo-size/TirsvadGUI/Dotnet.Wpf.Template?style=for-the-badge
[repo-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Template.git

[logo]: https://raw.githubusercontent.com/TirsvadGUI/Dotnet.Wpf.Template/master/images/logo/32x32/logo.png