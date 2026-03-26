# API Reference - ElBruno.QRCodeGenerator.CLI

Complete documentation for the ElBruno.QRCodeGenerator.CLI package.

## QRCode Class

The main entry point for QR code generation.

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

**Example:**

```csharp
using ElBruno.QRCodeGenerator.CLI;

string qrCode = QRCode.Generate("Hello, World!");
Console.WriteLine(qrCode);
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

**Example:**

```csharp
using ElBruno.QRCodeGenerator.CLI;

QRCode.Print("https://github.com/elbruno");
```

## QRCodeOptions

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

## ErrorCorrectionLevel

Enum for QR code error correction levels. Determines how much of the QR code can be damaged or obscured while still being scannable.

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

## Configuration Examples

### Dark Terminal (Default)

For standard dark-themed terminals, use the default settings:

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

For light-themed terminals, invert the colors to ensure visibility:

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

For critical data or codes that may be printed or photographed:

```csharp
using ElBruno.QRCodeGenerator.CLI;

var options = new QRCodeOptions
{
    ErrorCorrection = ErrorCorrectionLevel.H  // 30% recovery
};

QRCode.Print("Important data", options);
```

## Error Correction Levels

| Level | Recovery | Best For |
|-------|----------|----------|
| **L** | ~7% | Simple URLs, low risk of damage |
| **M** | ~15% | Default, balanced choice |
| **Q** | ~25% | Printed codes, high damage risk |
| **H** | ~30% | Critical data, maximum robustness |

Choose the error correction level based on:
- **How robust** the QR code needs to be against damage, dirt, or partial obscuring
- **How large** you want the resulting QR code (higher error correction → larger code)
- **What medium** the code will be displayed on (printed, photographed, digital)
