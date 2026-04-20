# ElBruno.QRCodeGenerator

[![CI Build](https://github.com/elbruno/ElBruno.QRCodeGenerator/actions/workflows/build.yml/badge.svg)](https://github.com/elbruno/ElBruno.QRCodeGenerator/actions/workflows/build.yml)
[![Publish to NuGet](https://github.com/elbruno/ElBruno.QRCodeGenerator/actions/workflows/publish.yml/badge.svg)](https://github.com/elbruno/ElBruno.QRCodeGenerator/actions/workflows/publish.yml)
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
| **ElBruno.QRCodeGenerator.Image** | [![NuGet](https://img.shields.io/nuget/v/ElBruno.QRCodeGenerator.Image)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Image) | [![Downloads](https://img.shields.io/nuget/dt/ElBruno.QRCodeGenerator.Image)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Image) | PNG/JPEG/WebP bitmap QR codes (SkiaSharp) |
| **ElBruno.QRCodeGenerator.Ascii** | [![NuGet](https://img.shields.io/nuget/v/ElBruno.QRCodeGenerator.Ascii)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Ascii) | [![Downloads](https://img.shields.io/nuget/dt/ElBruno.QRCodeGenerator.Ascii)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Ascii) | ASCII art QR codes for text output |
| **ElBruno.QRCodeGenerator.Pdf** | [![NuGet](https://img.shields.io/nuget/v/ElBruno.QRCodeGenerator.Pdf)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Pdf) | [![Downloads](https://img.shields.io/nuget/dt/ElBruno.QRCodeGenerator.Pdf)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Pdf) | PDF documents with embedded QR codes |
| **ElBruno.QRCodeGenerator.Tool** | [![NuGet](https://img.shields.io/nuget/v/ElBruno.QRCodeGenerator.Tool)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Tool) | [![Downloads](https://img.shields.io/nuget/dt/ElBruno.QRCodeGenerator.Tool)](https://www.nuget.org/packages/ElBruno.QRCodeGenerator.Tool) | `qrgen` global dotnet tool for all formats |

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

See [src/samples/BasicQRCode](src/samples/BasicQRCode) for the complete CLI example.

---

## ElBruno.QRCodeGenerator.Payloads

**Fluent payload builders for WiFi, vCard, Email, SMS, Geo, and URL QR codes**

Zero external dependencies — pure string formatting that works with any QR code renderer.

### Installation

```bash
dotnet add package ElBruno.QRCodeGenerator.Payloads
```

### Quick Start

```csharp
using ElBruno.QRCodeGenerator.Payloads;

// WiFi network QR code
var wifi = PayloadBuilder.Wifi("MyNetwork", "MyPassword123");
Console.WriteLine(wifi.GetPayloadString());
// Output: WIFI:T:WPA;S:MyNetwork;P:MyPassword123;;

// vCard contact
var vcard = PayloadBuilder.VCard("Bruno Capuano")
    .WithPhone("+1234567890", VCardPhoneType.Mobile)
    .WithEmail("bruno@example.com", VCardEmailType.Work)
    .WithOrganization("Contoso");
Console.WriteLine(vcard.GetPayloadString());

// Combine with any renderer (e.g., CLI)
QRCode.Print(wifi.GetPayloadString());
```

📂 [Full Payloads sample](src/samples/PayloadsDemo) | 📖 [API Reference](docs/api-reference.md)

---

## ElBruno.QRCodeGenerator.Svg

**Generate resolution-independent SVG QR codes**

### Installation

```bash
dotnet add package ElBruno.QRCodeGenerator.Svg
```

### Quick Start

```csharp
using ElBruno.QRCodeGenerator.Svg;

// Generate an SVG QR code
var svg = SvgQRCode.Generate("https://github.com/elbruno");
File.WriteAllText("qrcode.svg", svg);

// Custom colors and sizing
var customSvg = SvgQRCode.Generate("Hello, World!", svgOptions: new SvgOptions
{
    ForegroundColor = "#003366",
    BackgroundColor = "#f0f0f0",
    ModuleSize = 8
});
```

📂 [Full SVG sample](src/samples/SvgQRCodeDemo) | 📖 [API Reference](docs/api-reference.md)

---

## ElBruno.QRCodeGenerator.Image

**Generate PNG, JPEG, and WebP QR code bitmaps using SkiaSharp**

### Installation

```bash
dotnet add package ElBruno.QRCodeGenerator.Image
```

### Quick Start

```csharp
using ElBruno.QRCodeGenerator.Image;

// Save as PNG
byte[] png = ImageQRCode.ToPng("https://github.com/elbruno");
File.WriteAllBytes("qrcode.png", png);

// Save as JPEG with custom quality
byte[] jpg = ImageQRCode.ToJpeg("Hello, World!", quality: 90);
File.WriteAllBytes("qrcode.jpg", jpg);
```

📂 [Full Image sample](src/samples/ImageQRCodeDemo) | 📖 [API Reference](docs/api-reference.md)

---

## ElBruno.QRCodeGenerator.Ascii

**Generate QR codes as ASCII art text**

### Installation

```bash
dotnet add package ElBruno.QRCodeGenerator.Ascii
```

### Quick Start

```csharp
using ElBruno.QRCodeGenerator.Ascii;

// Default block style
AsciiQRCode.Print("https://github.com/elbruno");

// Hash style for text files
var ascii = AsciiQRCode.Generate("Hello!", asciiOptions: new AsciiOptions
{
    Style = AsciiStyle.Hash
});
File.WriteAllText("qrcode.txt", ascii);
```

📂 [Full ASCII sample](src/samples/AsciiQRCodeDemo) | 📖 [API Reference](docs/api-reference.md)

---

## ElBruno.QRCodeGenerator.Pdf

**Embed QR codes in PDF documents**

### Installation

```bash
dotnet add package ElBruno.QRCodeGenerator.Pdf
```

### Quick Start

```csharp
using ElBruno.QRCodeGenerator.Pdf;

// Generate a PDF with a QR code
PdfQRCode.Save("https://github.com/elbruno", "qrcode.pdf");

// With title and custom options
PdfQRCode.Save("https://github.com/elbruno", "styled.pdf", pdfOptions: new PdfOptions
{
    Title = "Scan Me!",
    ModuleSize = 3.0
});
```

📂 [Full PDF sample](src/samples/PdfQRCodeDemo) | 📖 [API Reference](docs/api-reference.md)

---

## ElBruno.QRCodeGenerator.Tool

**`qrgen` — global dotnet tool for all QR code formats**

### Installation

```bash
dotnet tool install -g ElBruno.QRCodeGenerator.Tool
```

### Quick Start

```bash
# Console output (default)
qrgen "https://github.com/elbruno"

# Save as SVG
qrgen "Hello World" --format svg --output hello.svg

# Save as PNG with custom colors
qrgen "Hello" --format png --output hello.png --fg "#003366" --bg "#f0f0f0"

# ASCII art
qrgen "Hello" --format ascii --ascii-style dot

# PDF with title
qrgen "https://example.com" --format pdf --output qr.pdf --title "Scan Me"
```

---

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

👤 **[Bruno Capuano (ElBruno)](https://github.com/elbruno)**

- 📝 **Blog**: [elbruno.com](https://elbruno.com)
- 📺 **YouTube**: [youtube.com/elbruno](https://youtube.com/elbruno)
- 🔗 **LinkedIn**: [linkedin.com/in/elbruno](https://linkedin.com/in/elbruno)
- 𝕏 **Twitter**: [twitter.com/elbruno](https://twitter.com/elbruno)
- 🎙️ **Podcast**: [notienenombre.com](https://notienenombre.com)

## Acknowledgments

Built with [QRCoder](https://github.com/codebude/QRCoder) — the most popular .NET QR encoder.
