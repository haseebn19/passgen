# Password Generator

<img src="logo.png" width="250">

A modern WPF password generator built with .NET 8 and MVVM architecture. Generate strong, customizable passwords with real-time strength evaluation.

## Features

- **Character Set Options**: Lowercase, uppercase, numbers, and symbols
- **Password Length**: Configurable from 4 to 128 characters
- **Unique Characters**: Guarantee a minimum number of unique characters
- **Strength Indicator**: Real-time password strength evaluation using Zxcvbn
- **System Theme**: Title bar matches Windows dark/light mode
- **Clipboard Support**: One-click copy to clipboard

## Requirements

- Windows 10/11
- .NET 8.0 Runtime

## Building

```powershell
git clone https://github.com/haseebn19/passgen.git
cd passgen/PassgenWPF
dotnet build
```

## Running

```powershell
dotnet run
```

Or open `passgen.sln` in Visual Studio 2022.

## Project Structure

```
PassgenWPF/
├── App.xaml                 # Application entry point
├── Converters/              # Value converters for XAML bindings
├── Helpers/                 # Window utilities (dark mode)
├── Themes/                  # Colors and control styles
├── ViewModels/              # MVVM view models
└── Views/                   # XAML windows
```

## License

[MIT License](https://opensource.org/licenses/MIT)
