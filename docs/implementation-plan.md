# ElBruno.QRCodeGenerator Library Expansion — Implementation Plan

**Owner:** Neo (Architect)  
**Date:** 2026-03-26  
**Status:** Planning (Not yet implemented)  
**For:** Bruno Capuano

---

## Vision

ElBruno.QRCodeGenerator evolves from a single-package CLI library into a modular ecosystem for QR code generation across multiple output formats and use cases. Each package is independently useful; all reuse the QRCoder engine for encoding.

| Package | Status | Format | Use Case |
|---------|--------|--------|----------|
| `ElBruno.QRCodeGenerator.CLI` | ✅ Exists | Console | Terminal/REPL output |
| `ElBruno.QRCodeGenerator.Payloads` | Tier 1 | Structured Data | WiFi, vCard, SMS, Geo, Email |
| `ElBruno.QRCodeGenerator.Svg` | Tier 1 | Vector (SVG) | Web, Email, Docs, Print |
| `ElBruno.QRCodeGenerator.Image` | Tier 2 | Bitmap (PNG/JPEG) | Embedded assets, reports |
| `ElBruno.QRCodeGenerator.Tool` | Tier 2 | Global Tool | CLI for batch processing |
| ANSI Colors (CLI v1.1) | Tier 2 | Console Enhancement | 256-color & truecolor terminals |
| `ElBruno.QRCodeGenerator.Pdf` | Tier 3 | PDF Vector | Document embedding |
| `ElBruno.QRCodeGenerator.Ascii` | Tier 3 | ASCII Art | Legacy terminal output |

---

## Solution Structure

### Directory Layout
```
src/
├── ElBruno.QRCodeGenerator.CLI/           # Existing
├── ElBruno.QRCodeGenerator.Payloads/      # Tier 1
├── ElBruno.QRCodeGenerator.Svg/           # Tier 1
├── ElBruno.QRCodeGenerator.Image/         # Tier 2
└── (Tool, PDF, ASCII added later)

src/tests/
├── ElBruno.QRCodeGenerator.CLI.Tests/     # Existing
├── ElBruno.QRCodeGenerator.Payloads.Tests/
├── ElBruno.QRCodeGenerator.Svg.Tests/
└── ElBruno.QRCodeGenerator.Image.Tests/

src/samples/
├── BasicQRCode/                           # Existing
├── PayloadsDemo/                          # Tier 1
├── SvgQRCodeDemo/                         # Tier 1
├── ImageQRCodeDemo/                       # Tier 2
└── ToolDemo/                              # Tier 2
```

### Solution File Updates
- **ElBruno.QRCodeGenerator.slnx** — Add new projects in order:
  1. Payloads (lib + tests)
  2. Svg (lib + tests)
  3. Image (lib + tests)
  4. Tool project (later)

### Directory.Build.props Considerations
- **Shared NuGet metadata:** All packages inherit Author, Company, RepositoryUrl, License, etc.
- **Package properties:** Keep PackageId and Description unique per package.
- **SDK version:** Continue targeting net8.0 and net10.0 across all libraries.
- **No breaking changes to existing props** — only add new PackageDescription entries if needed.

---

## Tier 1 — Build First (High Impact, Small Effort)

These packages have **zero rendering dependencies**, pure string output, and unlock immediate value for structured QR use cases and web/document integration.

### Package 1: ElBruno.QRCodeGenerator.Payloads

#### Purpose
Fluent builder API for common QR payload formats. Developers pass formatted strings directly to the main QRCode class. No rendering — just string formatting.

#### Project Structure
```
src/ElBruno.QRCodeGenerator.Payloads/
├── ElBruno.QRCodeGenerator.Payloads.csproj
├── Payloads/
│   ├── IPayload.cs                # Interface for payload builders
│   ├── WiFiPayload.cs             # WiFi connection (WPA/WEP/open)
│   ├── VCardPayload.cs            # Contact card (vCard 4.0)
│   ├── EmailPayload.cs            # mailto: links
│   ├── SmsPayload.cs              # SMS (sms: scheme)
│   ├── GeoPayload.cs              # Geographic coordinates (geo:)
│   └── UrlPayload.cs              # Enhanced URL builder
├── Builders/
│   └── PayloadBuilder.cs           # Fluent factory/registry
└── Utils/
    └── PayloadValidator.cs        # Input validation
```

#### Public API Surface

```csharp
namespace ElBruno.QRCodeGenerator.Payloads;

// Interface for all payloads
public interface IPayload
{
    /// <summary>Returns the formatted QR string (e.g., "WIFI:T:WPA;S:SSID;P:pwd;;").</summary>
    string GetPayloadString();
}

// WiFi — returns WIFI: scheme (Android/iPhone standard)
public class WiFiPayload : IPayload
{
    public WiFiPayload(string ssid, string password, WiFiAuthType authType = WiFiAuthType.WPA);
    public WiFiPayload WithHidden(bool hidden = true);
    public WiFiPayload WithMetered(bool metered = false);
    public string GetPayloadString(); // "WIFI:T:WPA;S:SSID;P:PASSWORD;;"
}

public enum WiFiAuthType { Open, WEP, WPA, WPA2, WPA3 }

// vCard — contact card for address book import
public class VCardPayload : IPayload
{
    public VCardPayload(string fullName);
    public VCardPayload WithPhone(string phone, VCardPhoneType type = VCardPhoneType.Mobile);
    public VCardPayload WithEmail(string email, VCardEmailType type = VCardEmailType.Personal);
    public VCardPayload WithOrganization(string org);
    public VCardPayload WithUrl(string url);
    public VCardPayload WithAddress(string street, string city, string state, string zip, string country);
    public string GetPayloadString(); // vCard 4.0 format
}

public enum VCardPhoneType { Mobile, Home, Work }
public enum VCardEmailType { Personal, Work }

// Email — mailto: scheme
public class EmailPayload : IPayload
{
    public EmailPayload(string to);
    public EmailPayload WithSubject(string subject);
    public EmailPayload WithBody(string body);
    public EmailPayload WithCc(string cc);
    public EmailPayload WithBcc(string bcc);
    public string GetPayloadString(); // "mailto:to@example.com?subject=...&body=..."
}

// SMS — sms: scheme
public class SmsPayload : IPayload
{
    public SmsPayload(string phoneNumber);
    public SmsPayload WithMessage(string message);
    public string GetPayloadString(); // "sms:+1234567890?body=..."
}

// Geo — geographic coordinates (maps, etc.)
public class GeoPayload : IPayload
{
    public GeoPayload(double latitude, double longitude);
    public GeoPayload WithAltitude(double meters);
    public GeoPayload WithUncertainty(double meters);
    public string GetPayloadString(); // "geo:latitude,longitude;u=uncertainty"
}

// URL — enhanced URL builder with validation
public class UrlPayload : IPayload
{
    public UrlPayload(string url);
    public UrlPayload WithScheme(string scheme = "https");
    public string GetPayloadString(); // Validated URL with scheme
}

// Factory for fluent construction
public static class PayloadBuilder
{
    public static WiFiPayload Wifi(string ssid, string password) => new(ssid, password);
    public static VCardPayload VCard(string fullName) => new(fullName);
    public static EmailPayload Email(string to) => new(to);
    public static SmsPayload Sms(string phoneNumber) => new(phoneNumber);
    public static GeoPayload Geo(double lat, double lon) => new(lat, lon);
    public static UrlPayload Url(string url) => new(url);
}
```

#### Usage Example
```csharp
// WiFi payload
var wifi = PayloadBuilder.Wifi("MyNetwork", "securePassword")
    .WithHidden(false);
QRCode.Print(wifi.GetPayloadString());

// vCard payload
var contact = PayloadBuilder.VCard("John Doe")
    .WithPhone("+1234567890", VCardPhoneType.Mobile)
    .WithEmail("john@example.com", VCardEmailType.Work)
    .WithOrganization("ACME Corp");
QRCode.Print(contact.GetPayloadString());
```

#### Dependencies
- `QRCoder` (same as CLI)
- **None for Payloads itself** — pure string formatting

#### Test Strategy
- **Unit tests (100% coverage):**
  - WiFi: All auth types, hidden networks, metered flags
  - vCard: Single/multiple fields, field escaping, special characters
  - Email: URL encoding of subject/body, multiple recipients
  - SMS: Phone number formats, message encoding
  - Geo: Coordinate precision, altitude/uncertainty handling
  - Url: Scheme validation, special characters
- **Integration tests:** Generate real QR codes with Payloads, verify they encode successfully
- **Validation tests:** Invalid inputs (empty SSID, negative coordinates) should raise ArgumentException

#### NuGet Metadata
```xml
<PackageId>ElBruno.QRCodeGenerator.Payloads</PackageId>
<Title>ElBruno QR Code Generator — Payloads</Title>
<Description>Fluent API builders for structured QR code payloads: WiFi, vCard, Email, SMS, Geographic coordinates, and URLs.</Description>
<Tags>qrcode,payloads,wifi,vcard,sms,geo,email,structured-data</Tags>
```

#### Build Order Dependency
- Depends on: Nothing (can build independently)
- Unlocks: Image package, Tool package

---

### Package 2: ElBruno.QRCodeGenerator.Svg

#### Purpose
Convert QR code matrix from QRCoder into SVG markup. Resolution-independent, works in web browsers, emails, documents, and print. Pure string output — no image rendering library required.

#### Project Structure
```
src/ElBruno.QRCodeGenerator.Svg/
├── ElBruno.QRCodeGenerator.Svg.csproj
├── Renderers/
│   ├── SvgRenderer.cs            # Core SVG generation from bool[,] matrix
│   └── SvgOptions.cs             # SVG-specific options
└── Extensions/
    └── QRCodeExtensions.cs       # .ToSvg() method on QRCode
```

#### Public API Surface

```csharp
namespace ElBruno.QRCodeGenerator.Svg;

// Rendering options for SVG output
public class SvgOptions
{
    public string ForegroundColor { get; set; } = "#000000";       // Module color
    public string BackgroundColor { get; set; } = "#ffffff";       // Quiet zone color
    public int ModuleSize { get; set; } = 10;                      // Pixels per module
    public int QuietZone { get; set; } = 1;                        // Modules of padding
    public bool IncludeXmlDeclaration { get; set; } = true;
    public string ViewBoxOnly { get; set; } = "";                  // Override viewBox (advanced)
}

// Core renderer — static utility for QRCode matrix → SVG
public static class SvgRenderer
{
    /// <summary>
    /// Converts a QR code matrix to SVG markup.
    /// </summary>
    /// <param name="qrCodeMatrix">bool[,] from QRCoder.QRCodeData</param>
    /// <param name="options">SVG rendering options</param>
    /// <returns>Complete SVG document as string</returns>
    public static string GenerateSvg(bool[,] qrCodeMatrix, SvgOptions? options = null);
    
    /// <summary>
    /// Converts to SVG and returns as inline string (no XML declaration, use in <embed> tags).</summary>
    public static string GenerateSvgInline(bool[,] qrCodeMatrix, SvgOptions? options = null);
}

// Extension method on QRCode (integrates with existing API)
public static class QRCodeExtensions
{
    /// <summary>Generate QR code SVG from text (highest-level API).</summary>
    public static string ToSvg(
        string text, 
        QRCodeOptions? options = null, 
        SvgOptions? svgOptions = null
    );
    
    /// <summary>Generate QR code SVG with fluent configuration.</summary>
    public static string ToSvgWith(
        string text,
        Action<QRCodeOptions> configureQr,
        Action<SvgOptions>? configureSvg = null
    );
}
```

#### SVG Output Structure
```svg
<?xml version="1.0" encoding="utf-8"?>
<svg xmlns="http://www.w3.org/2000/svg" 
     viewBox="0 0 210 210"
     width="210"
     height="210">
  <rect width="210" height="210" fill="#ffffff"/>
  <rect x="10" y="10" width="10" height="10" fill="#000000"/>
  <!-- ... more modules as rects ... -->
</svg>
```

#### Usage Example
```csharp
using ElBruno.QRCodeGenerator.Svg;

// Simplest form
string svg = QRCode.ToSvg("https://example.com");
File.WriteAllText("qrcode.svg", svg);

// With custom colors
var svgOpts = new SvgOptions 
{ 
    ForegroundColor = "#0066cc", 
    BackgroundColor = "#f0f0f0",
    ModuleSize = 12 
};
string styled = QRCode.ToSvg("https://example.com", svgOptions: svgOpts);

// Fluent API
string fluent = QRCode.ToSvgWith(
    "WiFi: MyNetwork",
    qr => qr.WithErrorCorrection(ErrorCorrectionLevel.High),
    svg => svg.ForegroundColor = "#333"
);
```

#### Dependencies
- `QRCoder` (same as CLI)
- **No rendering dependencies** — pure XML string construction

#### Test Strategy
- **Unit tests (100% coverage):**
  - SVG well-formedness: Valid XML output, proper root/rect elements
  - Module encoding: Each true module → visible rect, each false → invisible
  - Color properties: HEX color validation, custom colors in SVG
  - ViewBox scaling: Correct viewBox calculation for module size + quiet zone
  - Edge cases: Tiny QR codes (version 1), large codes (version 40)
- **Integration tests:**
  - Round-trip: Generate SVG, parse with XDocument, verify structure
  - Browser validation: SVGs viewable in Chrome, Firefox, Safari (manual spot-check)
  - Email compatibility: SVG renders in Gmail, Outlook, Apple Mail (spot-check)
- **Performance:** Large QR codes (100×100+ modules) should generate in <100ms

#### NuGet Metadata
```xml
<PackageId>ElBruno.QRCodeGenerator.Svg</PackageId>
<Title>ElBruno QR Code Generator — SVG</Title>
<Description>Generate QR codes as resolution-independent SVG markup. Perfect for web, email, documents, and print.</Description>
<Tags>qrcode,svg,vector,web,email,printing</Tags>
```

#### Build Order Dependency
- Depends on: Nothing (can build independently)
- Unlocks: PDF package, web integration scenarios

---

## Tier 2 — After Tier 1 Ships

### Package 3: ElBruno.QRCodeGenerator.Image

#### Purpose
PNG/JPEG bitmap generation. Requires an image rendering library. **Dependency decision needed before implementation.**

#### Dependency Decision Matrix

| Library | Pros | Cons | Recommendation |
|---------|------|------|-----------------|
| **ImageSharp** | Pure .NET, cross-platform, supports PNG/JPEG/WebP | Requires license after 30-day eval (check licensing terms) | **Primary choice** if licensing aligns; modern, well-maintained |
| **SkiaSharp** | Google-backed, bindings to Skia, excellent quality | Native binaries per platform, larger package, steeper learning curve | **Fallback** if ImageSharp licensing problematic; more control over rendering |
| **System.Drawing** (Windows only) | Built-in to Windows, no extra dependencies | Windows-only, limited cross-platform support, not recommended | **Not recommended** — breaks .NET cross-platform goals |

**Recommended decision:** Use **ImageSharp** with fallback to **SkiaSharp** depending on licensing requirements.

#### Project Structure (Template — Adjust Based on Dependency)
```
src/ElBruno.QRCodeGenerator.Image/
├── ElBruno.QRCodeGenerator.Image.csproj
├── Renderers/
│   ├── ImageRenderer.cs          # Core bitmap generation
│   └── ImageOptions.cs
└── Extensions/
    └── QRCodeExtensions.cs
```

#### Public API Surface (Conceptual)

```csharp
namespace ElBruno.QRCodeGenerator.Image;

public enum ImageFormat { Png, Jpeg }

public class ImageOptions
{
    public ImageFormat Format { get; set; } = ImageFormat.Png;
    public int ModuleSize { get; set; } = 10;                    // Pixels per module
    public int QuietZone { get; set; } = 1;                      // Modules of padding
    public string ForegroundColor { get; set; } = "#000000";
    public string BackgroundColor { get; set; } = "#ffffff";
    public int JpegQuality { get; set; } = 85;                   // 0-100
}

public static class ImageRenderer
{
    public static byte[] GenerateImage(bool[,] qrCodeMatrix, ImageOptions? options = null);
}

public static class QRCodeExtensions
{
    public static byte[] ToImage(string text, QRCodeOptions? options = null, ImageOptions? imgOptions = null);
    public static byte[] ToPng(string text, QRCodeOptions? options = null);
    public static byte[] ToJpeg(string text, QRCodeOptions? options = null, int quality = 85);
}
```

#### Usage Example
```csharp
using ElBruno.QRCodeGenerator.Image;

// Save as PNG
byte[] png = QRCode.ToPng("https://example.com");
File.WriteAllBytes("qrcode.png", png);

// Save as JPEG with options
var imgOpts = new ImageOptions 
{ 
    Format = ImageFormat.Jpeg,
    JpegQuality = 90,
    ModuleSize = 8
};
byte[] jpg = QRCode.ToImage("https://example.com", imgOptions: imgOpts);
File.WriteAllBytes("qrcode.jpg", jpg);
```

#### Dependencies
- `QRCoder`
- **ImageSharp** (recommended) or **SkiaSharp** (fallback)

#### Test Strategy
- **Unit tests:** File format validation, dimension correctness, color encoding
- **Integration tests:** Round-trip validation using external image libraries
- **Performance:** Generation should be <200ms even for large QR codes
- **Visual spot-checks:** Manual validation that generated images are scannable

#### NuGet Metadata
```xml
<PackageId>ElBruno.QRCodeGenerator.Image</PackageId>
<Title>ElBruno QR Code Generator — Image</Title>
<Description>Generate QR codes as PNG or JPEG bitmaps using [ImageSharp|SkiaSharp].</Description>
<Tags>qrcode,png,jpeg,image,bitmap</Tags>
```

#### Build Order Dependency
- Depends on: CLI (for QRCodeOptions)
- Unlocks: Tool package, Core extraction point
- **Core Extraction Trigger:** When Image ships and duplication is clear, evaluate extracting shared QRCoder wrapper to ElBruno.QRCodeGenerator.Core

---

### Package 4: ElBruno.QRCodeGenerator.Tool

#### Purpose
Global dotnet tool for CLI users. Ties together Console, SVG, and Image renderers. Single-purpose: batch QR generation from command line.

#### Project Structure
```
src/ElBruno.QRCodeGenerator.Tool/
├── ElBruno.QRCodeGenerator.Tool.csproj   # Packed as tool
├── Program.cs                             # Entry point
├── Commands/
│   ├── GenerateCommand.cs                # Main logic
│   └── OptionsParsing.cs
└── Output/
    └── OutputWriter.cs                   # File/console routing
```

#### CLI Interface

```bash
# Install
dotnet tool install -g elbruno.qrcodegenerator.tool

# Usage
qrgen "text to encode" [options]

# Options
qrgen "text" \
  --format console|svg|png|jpeg          # Output format (default: console)
  --output path/to/file                  # Write to file (default: stdout)
  --module-size 10                       # Pixels/chars per module
  --fg #000000 --bg #ffffff              # Colors (hex)
  --error-correction low|medium|high     # ECC level
  --invert                               # Invert colors (console mode)
  --quality 85                           # JPEG quality 0-100
```

#### Usage Examples
```bash
# Simple console output
qrgen "https://example.com"

# Save as SVG
qrgen "Contact: John@example.com" --format svg --output contact.svg

# Batch PNG generation
qrgen "Item-001" --format png --output item-001.png --module-size 5
qrgen "Item-002" --format png --output item-002.png --module-size 5

# Styled console (inverted, high ECC)
qrgen "IMPORTANT" --invert --error-correction high

# JPEG with custom size
qrgen "https://example.com" --format jpeg --output qr.jpg --quality 90
```

#### Dependencies
- `ElBruno.QRCodeGenerator.CLI`
- `ElBruno.QRCodeGenerator.Svg`
- `ElBruno.QRCodeGenerator.Image`
- System.CommandLine or similar (for CLI parsing)

#### NuGet Tool Metadata
```xml
<PackageId>ElBruno.QRCodeGenerator.Tool</PackageId>
<PackAsTool>true</PackAsTool>
<ToolCommandName>qrgen</ToolCommandName>
<Title>ElBruno QR Code Generator Tool</Title>
<Description>Global dotnet tool for command-line QR code generation in console, SVG, PNG, or JPEG formats.</Description>
<Tags>qrcode,tool,cli,console,svg,png</Tags>
```

#### Test Strategy
- **Command parsing:** Verify all flags parse correctly
- **File I/O:** Output to file, verify content written
- **Format routing:** Ensure --format flag routes to correct renderer
- **Integration:** Test end-to-end with real files

---

### Package 5: ANSI Color Support (CLI v1.1 Enhancement)

#### Purpose
Enhance the existing CLI package with 256-color and truecolor ANSI escape sequence support. Existing grayscale mode remains; color is opt-in.

#### Changes to ElBruno.QRCodeGenerator.CLI

**Add to QRCodeOptions:**
```csharp
public class QRCodeOptions
{
    // ... existing properties ...

    /// <summary>Foreground (module) color. Default: ConsoleColor.White.</summary>
    public ConsoleColor? ForegroundColor { get; set; } = ConsoleColor.White;

    /// <summary>Background (quiet zone) color. Default: ConsoleColor.Black.</summary>
    public ConsoleColor? BackgroundColor { get; set; } = ConsoleColor.Black;

    /// <summary>Use 256-color ANSI palette. Requires terminal support.</summary>
    public bool Use256ColorMode { get; set; } = false;

    /// <summary>Use truecolor (24-bit) ANSI. Requires modern terminal (iTerm2, Windows Terminal, etc.).</summary>
    public bool UseTrueColor { get; set; } = false;

    /// <summary>Custom RGB color for foreground (when UseTrueColor = true).</summary>
    public (byte R, byte G, byte B)? TrueColorForeground { get; set; } = null;

    /// <summary>Custom RGB color for background (when UseTrueColor = true).</summary>
    public (byte R, byte G, byte B)? TrueColorBackground { get; set; } = null;
}
```

**Update ConsoleRenderer:**
```csharp
public static class ConsoleRenderer
{
    // Existing method
    public static void PrintQRCode(QRCodeData qrCodeData, QRCodeOptions options);

    // New helpers (internal)
    private static string BuildAnsi256Color(int colorCode, bool isForeground);
    private static string BuildAnsiTrueColor(byte r, byte g, byte b, bool isForeground);
    private static string ApplyAnsiColors(string output, QRCodeOptions options);
}
```

#### Usage Example
```csharp
using ElBruno.QRCodeGenerator;

// Standard console colors
var opts1 = new QRCodeOptions { ForegroundColor = ConsoleColor.Cyan };
QRCode.Print("Hello", opts1);

// 256-color palette
var opts2 = new QRCodeOptions 
{ 
    Use256ColorMode = true,
    ForegroundColor = ConsoleColor.Green  // Mapped to 256-color index
};
QRCode.Print("Colored", opts2);

// Truecolor (24-bit)
var opts3 = new QRCodeOptions
{
    UseTrueColor = true,
    TrueColorForeground = (0, 102, 204),   // Nice blue
    TrueColorBackground = (240, 240, 240)   // Light gray
};
QRCode.Print("Modern Terminal", opts3);
```

#### ANSI Escape Sequence Reference
```
Standard ConsoleColor: \u001b[30m-\u001b[37m (foreground), \u001b[40m-\u001b[47m (background)
256-color: \u001b[38;5;Nm (foreground), \u001b[48;5;Nm (background), N=0-255
Truecolor:  \u001b[38;2;R;G;Bm (foreground), \u001b[48;2;R;G;Bm (background)
Reset:      \u001b[0m
```

#### Test Strategy
- **Unit tests:** ANSI escape sequence generation
- **Manual spot-checks:** Color output in iTerm2, Windows Terminal, VS Code terminal
- **Backward compatibility:** Existing grayscale mode unaffected

#### Backward Compatibility
- **No breaking changes** — all new properties are optional with sensible defaults
- Existing callers without colors work unchanged
- Binary backward compatible with v1.0

---

## Tier 3 — If Demand Exists

### Package 6: ElBruno.QRCodeGenerator.Pdf

**Scope (Future):**
- QR codes embedded in PDF documents
- Pure text PDF or PDF with embedded images
- Requires: PdfSharp or similar
- Output: byte[] representing PDF with QR code

### Package 7: ElBruno.QRCodeGenerator.Ascii

**Scope (Future):**
- ASCII art QR codes (for legacy terminal output)
- Uses ASCII characters (·, ■, □, etc.) or pure block drawing
- No external dependencies, pure string output
- Output: Console or text file

---

## Shared Patterns Across All Packages

### How Packages Reuse the QRCoder Matrix

**Single Source of Truth:**
All packages depend on `QRCoder.QRCodeData`, which encodes text → bool[,] matrix.

```csharp
// In CLI, Svg, Image, Tool (all similar pattern)
public static class QRCode
{
    private static QRCodeData GenerateMatrix(string text, QRCodeOptions options)
    {
        var qrCoder = new QRCoder.QRCoder();
        var errorLevel = MapErrorCorrectionLevel(options.ErrorCorrectionLevel);
        var qrCodeData = qrCoder.GetQRCode(text, errorLevel);
        return qrCodeData;
    }
}
```

**Renderer contract:**
Each package takes the bool[,] matrix and converts to its output:
- CLI: `bool[,] → Console.Out with Unicode`
- SVG: `bool[,] → SVG XML string`
- Image: `bool[,] → byte[] (PNG/JPEG)`
- Payloads: No matrix needed; pure string formatting

### Core Extraction Trigger

**When to Extract ElBruno.QRCodeGenerator.Core:**
1. **After Image ships** and we see real duplication
2. **Clear patterns emerge** in:
   - QRCoder wrapper (~15-20 lines)
   - ErrorCorrectionLevel mapping
   - QRCodeOptions processing
3. **IF duplication > 30% across 3+ packages**, extract to Core
4. **OTHERWISE**, keep duplication as-is (YAGNI, simpler dependency graph)

**Extraction checklist (if/when needed):**
- Create `src/ElBruno.QRCodeGenerator.Core/`
- Move shared QRCoder wrapper to Core
- Move shared enums/options to Core
- Update CLI, Svg, Image to depend on Core
- Keep Payloads, Tool independent (no Core dependency)

---

## Build and Publishing Order

### Phase 1: Tier 1 (Parallel builds possible)
1. **ElBruno.QRCodeGenerator.Payloads** — no dependencies, ships immediately after code review
2. **ElBruno.QRCodeGenerator.Svg** — no external rendering deps, ships immediately after review
3. Both can be built/published in parallel; they're independent

### Phase 2: Tier 2 (Sequential; Image unblocks Tool)
4. **Dependency decision** for Image (ImageSharp vs SkiaSharp)
5. **ElBruno.QRCodeGenerator.Image** — implement, test, publish
6. **ANSI enhancement** to CLI (v1.1 patch release)
7. **ElBruno.QRCodeGenerator.Tool** — implement with Payloads + Svg + Image, test, publish

### Phase 3: Tier 3 (As-needed)
8. **PDF** — if demand warrants
9. **ASCII** — if demand warrants

### Publishing Checklist
- All tests passing (xUnit, 80%+ coverage)
- README updated with package-specific examples
- Sample projects created for each new package
- NuGet metadata complete (description, tags, icon)
- GitHub release notes (what's new, breaking changes)
- Solution file (.slnx) includes all projects

---

## Solution File Management

### Current State
```
ElBruno.QRCodeGenerator.slnx
├── src/ElBruno.QRCodeGenerator.CLI/
├── src/tests/ElBruno.QRCodeGenerator.CLI.Tests/
└── src/samples/BasicQRCode/
```

### After Tier 1
```
ElBruno.QRCodeGenerator.slnx
├── src/
│   ├── ElBruno.QRCodeGenerator.CLI/
│   ├── ElBruno.QRCodeGenerator.Payloads/
│   ├── ElBruno.QRCodeGenerator.Svg/
│   ├── tests/
│   │   ├── ElBruno.QRCodeGenerator.CLI.Tests/
│   │   ├── ElBruno.QRCodeGenerator.Payloads.Tests/
│   │   └── ElBruno.QRCodeGenerator.Svg.Tests/
│   └── samples/
│       ├── BasicQRCode/
│       ├── PayloadsDemo/
│       └── SvgQRCodeDemo/
```

### After Tier 2
```
ElBruno.QRCodeGenerator.slnx
├── src/
│   ├── ElBruno.QRCodeGenerator.CLI/
│   ├── ElBruno.QRCodeGenerator.Payloads/
│   ├── ElBruno.QRCodeGenerator.Svg/
│   ├── ElBruno.QRCodeGenerator.Image/
│   ├── ElBruno.QRCodeGenerator.Tool/
│   ├── tests/
│   │   ├── (4 test projects)
│   └── samples/
│       └── (5 sample projects)
```

---

## Risk Mitigation

| Risk | Mitigation |
|------|-----------|
| **ImageSharp licensing** | Evaluate early; have SkiaSharp fallback plan ready |
| **SVG not rendering in all contexts** | Test in major email clients early (Gmail, Outlook, Apple Mail) |
| **Tool command name conflict** | `qrgen` is sufficiently unique; verify against existing tools |
| **Performance regression** | Benchmark matrix generation across all packages; set <200ms target |
| **API surface bloat** | Each package has clear responsibility; keep options minimal |
| **Dependency version conflicts** | Pin QRCoder version explicitly; test with matrix of .NET versions |

---

## Success Metrics

### Tier 1 Completion
- ✅ Both Payloads and Svg packages published to NuGet
- ✅ Documentation and samples for both packages
- ✅ 80%+ test coverage on both packages
- ✅ Packages usable independently (no hidden dependencies)

### Tier 2 Completion
- ✅ Image package published (ImageSharp or SkiaSharp)
- ✅ Tool published and installable via `dotnet tool install`
- ✅ ANSI colors working in Windows Terminal, iTerm2, VS Code terminal
- ✅ All packages work together seamlessly

### Tier 3 (If executed)
- ✅ PDF and/or ASCII packages available and documented

---

## Next Steps for Team Leads

1. **Neo (Architect):** Finalize dependency decision for Image package (ImageSharp vs SkiaSharp). Document in decisions.md.
2. **Implementation team:**
   - Trinity: Implement Payloads (WiFi, vCard, Email, SMS, Geo, Url builders)
   - Tank: Implement Svg package (SvgRenderer, extension methods, tests)
   - Test coverage: Aim for 80%+ on both Tier 1 packages
3. **Review cycle:** Code review, validate public API, ensure no breaking changes to existing CLI
4. **Before Tier 2:** Publish Tier 1, gather user feedback on Payloads and Svg APIs

---

**Document version:** 1.0  
**Last updated:** 2026-03-26  
**Status:** Planning phase — ready for implementation
