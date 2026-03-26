# ElBruno.QRCodeGenerator

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/elbruno/ElBruno.QRCodeGenerator/blob/main/LICENSE)
[![GitHub](https://img.shields.io/github/stars/elbruno/ElBruno.QRCodeGenerator?style=social)](https://github.com/elbruno/ElBruno.QRCodeGenerator)

## A family of .NET libraries for QR code generation 🟦

ElBruno.QRCodeGenerator is a suite of .NET libraries for generating QR codes in various formats — from console rendering to image generation. Built for developers who need flexible, easy-to-use QR code tools.

## Packages

| Package | NuGet | Downloads | Description |
|---------|-------|-----------|-------------|
| **ElBruno.QRCodeGenerator.CLI** | [![NuGet](https://img.shields.io/nuget/v/ElBruno.QRCodeGenerator.CLI)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.CLI) | [![Downloads](https://img.shields.io/nuget/dt/ElBruno.QRCodeGenerator.CLI)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.CLI) | Console QR codes with Unicode blocks |
| **ElBruno.QRCodeGenerator.Image** | _Coming soon_ | | Generate QR codes as PNG/SVG images |
| **ElBruno.QRCodeGenerator.Core** | _Coming soon_ | | Shared core library |

---

## ElBruno.QRCodeGenerator.CLI

**Generate and display QR codes in the console using Unicode block characters**

The CLI package is a lightweight .NET library that creates beautiful QR codes in your terminal with Unicode block characters. Perfect for CLI tools, server logs, and terminal applications.

## Features

- 🎯 **Simple API** - Generate QR codes with one line of code: `QRCode.Print("your text")`
- 🖥️ **Console-Optimized** - Uses Unicode block characters for perfect 2:1 aspect ratio in terminals
- 🎨 **Theme Support** - Built-in light/dark theme support with color inversion
- 📦 **Multi-Target** - .NET 8.0 and .NET 10.0 support
- ✅ **Battle-Tested** - Built on [QRCoder](https://github.com/codebude/QRCoder), the most popular .NET QR encoder
- 🔧 **Configurable** - Adjust error correction, quiet zone, and rendering options

## Installation

```bash
dotnet add package ElBruno.QRCodeGenerator.CLI
```

Or add directly to your `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="ElBruno.QRCodeGenerator.CLI" Version="1.0.0" />
</ItemGroup>
```

## Quick Start

### Print to Console

```csharp
using ElBruno.QRCodeGenerator.CLI;

// Simplest possible usage
QRCode.Print("https://github.com/elbruno");
```

### Get QR Code as String

```csharp
using ElBruno.QRCodeGenerator.CLI;

// Generate QR code and capture as string
string qrCode = QRCode.Generate("Hello, World!");
Console.WriteLine(qrCode);
```

## API Reference

### QRCode.Generate()

Generates a QR code as a string of Unicode block characters.

```csharp
/// <summary>
/// Generates a QR code as a string of Unicode block characters.
/// The returned string can be printed to console or saved to a file.
/// </summary>
/// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
/// <param name="options">Optional configuration for QR generation and rendering.</param>
/// <returns>A string containing the QR code rendered with Unicode block characters.</returns>
public static string Generate(string text, QRCodeOptions? options = null)
```

### QRCode.Print()

Generates and prints a QR code directly to `Console.Out`.

```csharp
/// <summary>
/// Generates and prints a QR code directly to Console.Out.
/// </summary>
/// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
/// <param name="options">Optional configuration for QR generation and rendering.</param>
public static void Print(string text, QRCodeOptions? options = null)
```

### QRCodeOptions

Configuration class for QR code generation and rendering.

```csharp
public class QRCodeOptions
{
    /// <summary>
    /// Gets or sets the error correction level.
    /// Default is M (medium, ~15% recovery).
    /// </summary>
    public ErrorCorrectionLevel ErrorCorrection { get; set; } = ErrorCorrectionLevel.M;

    /// <summary>
    /// Gets or sets whether to invert the colors (light background, dark foreground).
    /// Useful for light-themed terminals. Default is false.
    /// </summary>
    public bool InvertColors { get; set; } = false;

    /// <summary>
    /// Gets or sets the size of the quiet zone (border) around the QR code in modules.
    /// QR spec recommends at least 4, but 1 works for most scanners. Default is 1.
    /// </summary>
    public int QuietZoneSize { get; set; } = 1;
}
```

### ErrorCorrectionLevel

Enum for QR code error correction levels.

```csharp
public enum ErrorCorrectionLevel
{
    /// <summary>Low - recovers approximately 7% of data</summary>
    L = 0,

    /// <summary>Medium - recovers approximately 15% of data (recommended default)</summary>
    M = 1,

    /// <summary>Quartile - recovers approximately 25% of data</summary>
    Q = 2,

    /// <summary>High - recovers approximately 30% of data</summary>
    H = 3
}
```

## Configuration

### Dark Terminal (Default)

```csharp
using ElBruno.QRCodeGenerator.CLI;

var options = new QRCodeOptions
{
    ErrorCorrection = ErrorCorrectionLevel.M,
    QuietZoneSize = 1
};

QRCode.Print("https://example.com", options);
```

### Light Terminal

```csharp
using ElBruno.QRCodeGenerator.CLI;

var options = new QRCodeOptions
{
    InvertColors = true,  // Swap light/dark for light backgrounds
    ErrorCorrection = ErrorCorrectionLevel.H,
    QuietZoneSize = 2
};

QRCode.Print("https://example.com", options);
```

### High Error Correction

```csharp
using ElBruno.QRCodeGenerator.CLI;

var options = new QRCodeOptions
{
    ErrorCorrection = ErrorCorrectionLevel.H  // 30% recovery
};

QRCode.Print("Important data", options);
```

## Error Correction Levels

Choose based on your use case:

| Level | Recovery | Best For |
|-------|----------|----------|
| **L** | ~7% | Simple URLs, low risk of damage |
| **M** | ~15% | Default, balanced choice |
| **Q** | ~25% | Printed codes, high damage risk |
| **H** | ~30% | Critical data, maximum robustness |

## Samples

This repository includes sample console applications demonstrating various features:

### BasicQRCode Sample

```bash
cd samples/BasicQRCode
dotnet run
```

Demonstrates:
- Simple QR code generation
- Custom options configuration
- Using Generate() and Print() methods

See [samples/BasicQRCode](samples/BasicQRCode) for the complete example.

## Building from Source

### Prerequisites

- .NET 8.0 SDK or later
- Git

### Build Steps

```bash
git clone https://github.com/elbruno/ElBruno.QRCodeGenerator.git
cd ElBruno.QRCodeGenerator
dotnet build
```

### Running Tests

```bash
dotnet test
```

## Project Structure

```
ElBruno.QRCodeGenerator/
├── src/
│   ├── ElBruno.QRCodeGenerator.CLI/        # Console QR code package
│   │   ├── QRCode.cs                       # Main API class
│   │   ├── QRCodeOptions.cs                # Configuration classes
│   │   └── ElBruno.QRCodeGenerator.CLI.csproj
│   ├── ElBruno.QRCodeGenerator.Image/      # [Coming soon] Image generation
│   └── ElBruno.QRCodeGenerator.Core/       # [Coming soon] Shared core
├── tests/
│   └── ElBruno.QRCodeGenerator.CLI.Tests/
├── samples/
│   └── BasicQRCode/                        # Example applications
├── docs/
│   ├── publishing.md                       # NuGet publishing guide
│   └── nuget-logo-prompt.md                # NuGet icon design prompts
├── README.md
├── CHANGELOG.md
├── LICENSE
└── global.json
```

## Documentation

- [Publishing Guide](docs/publishing.md) - How to publish to NuGet
- [Changelog](docs/CHANGELOG.md) - Version history and release notes

## License

MIT License - see [LICENSE](LICENSE) for details.

## Author

👤 **Bruno Capuano** (ElBruno)

- Blog: https://elbruno.com
- YouTube: https://youtube.com/@inthelabs
- LinkedIn: https://linkedin.com/in/inthelabs
- Twitter: https://twitter.com/inthelabs
- Podcast: https://inthelabs.dev

## Acknowledgments

Built with [QRCoder](https://github.com/codebude/QRCoder) — the most popular .NET QR encoder.
