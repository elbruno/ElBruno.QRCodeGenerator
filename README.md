# ElBruno.QRCodeGenerator

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/elbruno/ElBruno.QRCodeGenerator/blob/main/LICENSE)
[![GitHub](https://img.shields.io/github/stars/elbruno/ElBruno.QRCodeGenerator?style=social)](https://github.com/elbruno/ElBruno.QRCodeGenerator)

## A family of .NET libraries for QR code generation 🟦

ElBruno.QRCodeGenerator is a suite of .NET libraries for generating QR codes in various formats — from console rendering to image generation. Built for developers who need flexible, easy-to-use QR code tools.

## Packages

| Package | NuGet | Downloads | Description |
|---------|-------|-----------|-------------|
| **ElBruno.QRCodeGenerator.CLI** | [![NuGet](https://img.shields.io/nuget/v/ElBruno.QRCodeGenerator.CLI)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.CLI) | [![Downloads](https://img.shields.io/nuget/dt/ElBruno.QRCodeGenerator.CLI)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.CLI) | Console QR codes with Unicode blocks |
| **ElBruno.QRCodeGenerator.Payloads** | [![NuGet](https://img.shields.io/nuget/v/ElBruno.QRCodeGenerator.Payloads)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Payloads) | [![Downloads](https://img.shields.io/nuget/dt/ElBruno.QRCodeGenerator.Payloads)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Payloads) | Fluent payload builders (WiFi, vCard, SMS, Geo, Email, URL) |
| **ElBruno.QRCodeGenerator.Svg** | [![NuGet](https://img.shields.io/nuget/v/ElBruno.QRCodeGenerator.Svg)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Svg) | [![Downloads](https://img.shields.io/nuget/dt/ElBruno.QRCodeGenerator.Svg)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Svg) | Resolution-independent SVG QR codes |

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

## Samples

This repository includes sample console applications demonstrating various features:

### BasicQRCode Sample

```bash
cd src/samples/BasicQRCode
dotnet run
```

Demonstrates:
- Simple QR code generation
- Custom options configuration
- Using Generate() and Print() methods

See [src/samples/BasicQRCode](src/samples/BasicQRCode) for the complete example.

## Building from Source

```bash
git clone https://github.com/elbruno/ElBruno.QRCodeGenerator.git
cd ElBruno.QRCodeGenerator
dotnet build
dotnet test
```

## Documentation

- [API Reference](docs/api-reference.md) - Complete API documentation and configuration examples
- [Publishing Guide](docs/publishing.md) - How to publish to NuGet (trusted publishing via OIDC)
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
