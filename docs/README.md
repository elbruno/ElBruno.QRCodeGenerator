# ElBruno.QRCodeGenerator Documentation

## Overview

ElBruno.QRCodeGenerator is a suite of .NET libraries for generating QR codes in various formats.

### Current Packages

#### ElBruno.QRCodeGenerator.CLI

A .NET library that generates QR codes optimized for console/terminal display using Unicode block characters.

### Coming Soon

- **ElBruno.QRCodeGenerator.Image** - Generate QR codes as PNG/SVG images
- **ElBruno.QRCodeGenerator.Core** - Shared core functionality

---

## ElBruno.QRCodeGenerator.CLI

### Installation

```bash
dotnet add package ElBruno.QRCodeGenerator.CLI
```

## API Reference

### QRCode Class

The main entry point for generating and displaying QR codes.

#### Methods

##### `Print(string text, QRCodeOptions? options = null)`

Generates a QR code and prints it directly to the console.

**Parameters:**
- `text` - The text or URL to encode
- `options` - Optional configuration (default: null)

**Example:**
```csharp
QRCode.Print("https://github.com/elbruno");
```

##### `Generate(string text, QRCodeOptions? options = null)`

Generates a QR code and returns it as a string.

**Parameters:**
- `text` - The text or URL to encode
- `options` - Optional configuration (default: null)

**Returns:** String containing the QR code with Unicode characters

**Example:**
```csharp
string qrCode = QRCode.Generate("Hello, World!");
File.WriteAllText("qrcode.txt", qrCode);
```

### QRCodeOptions Class

Configuration options for QR code generation.

#### Properties

##### `ErrorCorrection`

Type: `ErrorCorrectionLevel`  
Default: `ErrorCorrectionLevel.M`

Error correction level for the QR code. Higher levels allow more data recovery if the QR code is damaged.

##### `InvertColors`

Type: `bool`  
Default: `false`

Inverts the QR code colors (white on black vs black on white). Useful for light terminal themes.

##### `QuietZoneSize`

Type: `int`  
Default: `1`

Size of the quiet zone (border) around the QR code in modules.

### ErrorCorrectionLevel Enum

- `L` - Low (~7% recovery capability)
- `M` - Medium (~15% recovery capability)
- `Q` - Quartile (~25% recovery capability)
- `H` - High (~30% recovery capability)

## Examples

### Basic URL QR Code

```csharp
using ElBruno.QRCodeGenerator.CLI;

QRCode.Print("https://github.com/elbruno");
```

### High Error Correction

```csharp
var options = new QRCodeOptions
{
    ErrorCorrection = ErrorCorrectionLevel.H
};

QRCode.Print("https://example.com", options);
```

### Light Theme Terminal

```csharp
var options = new QRCodeOptions
{
    InvertColors = true
};

QRCode.Print("https://example.com", options);
```

### Save to File

```csharp
string qrCode = QRCode.Generate("Contact: john@example.com");
await File.WriteAllTextAsync("contact-qr.txt", qrCode);
```

## Technical Details

### Unicode Rendering

The library uses Unicode block characters to represent the QR code in the console:

- `█` - Full block (2 pixels)
- `▀` - Upper half block
- `▄` - Lower half block
- ` ` - Space (white)

This approach provides 2:1 aspect ratio correction, making the QR codes scannable directly from the terminal.

### Dependencies

- **QRCoder** - QR code encoding engine
- **.NET 8.0 or .NET 10.0** - Runtime

## Best Practices

1. **Use appropriate error correction**: Higher levels create larger QR codes but are more resilient
2. **Keep text short**: Shorter data produces smaller, more scannable QR codes
3. **Test in your terminal**: Different terminals may render Unicode characters differently
4. **Consider your audience's theme**: Use `InvertColors` for light terminal users

## Troubleshooting

### QR Code Not Scanning

- Increase error correction level to `H`
- Reduce the amount of data being encoded
- Ensure adequate quiet zone (border)
- Check terminal font supports Unicode block characters

### Rendering Issues

- Verify terminal supports UTF-8 encoding
- Try different terminal emulators
- Adjust terminal font size for better visibility

## Contributing

Contributions welcome! See the main repository for contribution guidelines.
