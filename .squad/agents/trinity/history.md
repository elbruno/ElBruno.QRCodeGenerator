# Project Context

- **Owner:** Bruno Capuano
- **Project:** ElBruno.QRCodeGenerator.CLI — A .NET library that creates QR codes displayed in the console using console characters. Published to NuGet.
- **Stack:** C#, .NET 8/10, xUnit, NuGet, GitHub Actions
- **Created:** 2026-03-26

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

### 2026-03-26: Project Restructuring — Folder Reorganization

**Structural Change — All team members aware:**
- Tests moved from `tests/` → `src/tests/`
- Samples moved from `samples/` → `src/samples/`
- Stale ElBruno.QRCodeGenerator.CLI.slnx deleted
- All references updated (solutions, projects, CI/CD workflows)
- Build passes, 35 tests green, changes committed

**Impact on Trinity's work:**
- Library source still in `src/ElBruno.QRCodeGenerator.CLI/`
- Tests adjacent in `src/tests/` for easy reference
- Sample apps in `src/samples/` demonstrate usage patterns
- All project file references updated

---

### 2026-03-26: Core Library Implementation

**Architecture:**
- Main API in `QRCode.cs`: static class with `Generate()` and `Print()` methods
- Configuration in `QRCodeOptions.cs`: error correction levels, color inversion, quiet zone size
- Rendering engine in `Internal/ConsoleRenderer.cs`: Unicode half-block character rendering

**Key Technical Decisions:**
- **QRCoder Integration**: Used `QRCoder.QRCodeGenerator` to generate QR data, extracted `ModuleMatrix` (List<BitArray>) and converted to bool[,] array
- **Namespace Handling**: Fully qualified `QRCoder.QRCodeGenerator` references to avoid conflicts with our namespace `ElBruno.QRCodeGenerator.CLI`
- **Unicode Rendering Strategy**: 
  - Process 2 QR module rows → 1 console line (2:1 aspect ratio correction)
  - Characters: █ (both dark), ▀ (top dark), ▄ (bottom dark), space (both light)
  - Quiet zone added by expanding matrix before rendering
  - Color inversion supported by flipping module booleans

**File Paths:**
- `src/ElBruno.QRCodeGenerator.CLI/QRCode.cs` — Public API
- `src/ElBruno.QRCodeGenerator.CLI/QRCodeOptions.cs` — Configuration
- `src/ElBruno.QRCodeGenerator.CLI/Internal/ConsoleRenderer.cs` — Rendering engine
- `src/samples/BasicQRCode/Program.cs` — Working sample demonstrating usage

**Build Targets:** net8.0 and net10.0
**Dependencies:** QRCoder 1.6.0 (battle-tested QR encoding library)

**Validation:** Build succeeded, sample app runs and displays scannable QR codes in console with various options (error correction, inverted colors, quiet zone sizing).

---

### 2026-03-27: Ascii Package Implementation (Tier 3)

**Architecture:**
- Follows exact same patterns as Svg package — own namespace, own error correction enum, own options classes
- `AsciiRenderer.Render(bool[,], AsciiOptions?)` — low-level renderer, pure string output
- `AsciiQRCode.Generate()` / `AsciiQRCode.Print()` — high-level API with QRCoder integration
- Five built-in styles: Block (██), Hash (##), Dot (●●/··), Shade (██/░░), Custom (user-defined)
- `AsciiOptions` supports quiet zone, border (+-| frame), and custom character pairs

**File Paths:**
- `src/ElBruno.QRCodeGenerator.Ascii/AsciiQRCode.cs` — Public API
- `src/ElBruno.QRCodeGenerator.Ascii/AsciiRenderer.cs` — Rendering engine
- `src/ElBruno.QRCodeGenerator.Ascii/AsciiOptions.cs` — Rendering configuration
- `src/ElBruno.QRCodeGenerator.Ascii/AsciiStyle.cs` — Style enum
- `src/ElBruno.QRCodeGenerator.Ascii/AsciiQRCodeOptions.cs` — QR generation options
- `src/ElBruno.QRCodeGenerator.Ascii/AsciiErrorCorrectionLevel.cs` — Error correction enum
- `src/tests/ElBruno.QRCodeGenerator.Ascii.Tests/` — 30 tests (renderer, QR code, options)
- `src/samples/AsciiQRCodeDemo/Program.cs` — Demo app with all styles + file save

**Build Targets:** net8.0 and net10.0
**Dependencies:** QRCoder 1.6.0 (same as all other packages)
**Validation:** Build succeeded (0 warnings, 0 errors), 30 tests green, solution file updated.

---

### 2026-03-27: Image Package — SkiaSharp-Based Bitmap QR Code Generation

**New Package:** `ElBruno.QRCodeGenerator.Image`

**Architecture (mirrors Svg package pattern):**
- `ImageQRCode.cs` — Static high-level API: `Generate()`, `ToPng()`, `ToJpeg()`
- `ImageRenderer.cs` — Static renderer: `bool[,]` matrix → PNG/JPEG/WebP bytes via SkiaSharp
- `ImageOptions.cs` — Format, ModuleSize, QuietZone, ForegroundColor, BackgroundColor, JpegQuality
- `ImageQRCodeOptions.cs` — ErrorCorrection level
- `ImageErrorCorrectionLevel.cs` — Own enum (L/M/Q/H), no cross-package dependency
- `ImageFormat.cs` — Png, Jpeg, Webp enum

**Key Technical Decisions:**
- **SkiaSharp 3.116.1** chosen over ImageSharp (BSD-3 license, no commercial concerns)
- Uses `SKBitmap` + `SKCanvas` + `SKPaint` for rectangle-based module rendering
- Output via `SKImage.Encode()` with `SKEncodedImageFormat` for PNG/JPEG/WebP
- Color parsing: hex string → `SKColor` via manual parse (no external dependency)
- QRCoder usage identical to Svg package (fully qualified references)

**File Paths:**
- `src/ElBruno.QRCodeGenerator.Image/` — 6 source files
- `src/tests/ElBruno.QRCodeGenerator.Image.Tests/` — 3 test files, 24 tests
- `src/samples/ImageQRCodeDemo/` — Console app demo (PNG, JPEG, custom colors)

**Build Targets:** net8.0 and net10.0
**Dependencies:** QRCoder 1.6.0, SkiaSharp 3.116.1
**Tests:** 24 passing (renderer: 10, QRCode API: 11, options: 4 — includes magic byte validation, dimension checks, color verification, edge cases)

---

### 2026-03-27: PDF Package Implementation (Tier 3)

**Architecture:**
- New package `ElBruno.QRCodeGenerator.Pdf` using PdfSharpCore 1.3.65 (MIT, cross-platform)
- Follows exact same pattern as the Svg package: separate renderer, high-level API, own error correction enum
- PdfRenderer draws QR modules as filled XGraphics rectangles, supports centering, custom position, and title text
- PdfQRCode provides Generate (returns byte[]) and Save (writes to file) static methods

**Key Technical Details:**
- PdfSharpCore's XGraphics API: XFont for text, XSolidBrush + DrawRectangle for modules, DrawString for titles
- Hex color parsing via custom ParseHexColor → XColor.FromArgb
- PDF centering accounts for title offset when both title and centering are used
- PdfSharpCore transitively pulls SixLabors.ImageSharp 1.0.4 which has known vulnerabilities (NuGet warnings) — this is a PdfSharpCore issue, not ours

**File Paths:**
- `src/ElBruno.QRCodeGenerator.Pdf/PdfQRCode.cs` — High-level API
- `src/ElBruno.QRCodeGenerator.Pdf/PdfRenderer.cs` — PDF rendering engine
- `src/ElBruno.QRCodeGenerator.Pdf/PdfOptions.cs` — PDF rendering configuration
- `src/ElBruno.QRCodeGenerator.Pdf/PdfQRCodeOptions.cs` — QR generation options
- `src/ElBruno.QRCodeGenerator.Pdf/PdfErrorCorrectionLevel.cs` — Error correction enum
- `src/tests/ElBruno.QRCodeGenerator.Pdf.Tests/` — 20 tests, all passing
- `src/samples/PdfQRCodeDemo/Program.cs` — Working sample with 4 demos

**Build Targets:** net8.0 and net10.0
**Dependencies:** QRCoder 1.6.0, PdfSharpCore 1.3.65
**Validation:** Build succeeded (net8.0), 20 tests green, solution file updated.

---

### ANSI Color Support — v1.1 Enhancement

**Date:** Added as part of v1.1 enhancement request by Bruno Capuano.

**What was done:**
- Added 5 new properties to `QRCodeOptions`: `ForegroundColor`, `BackgroundColor`, `UseTrueColor`, `TrueColorForeground`, `TrueColorBackground`
- Created `AnsiColorHelper.cs`: internal static helper mapping ConsoleColor to ANSI escape codes (16-color) and generating 24-bit truecolor sequences
- Updated `Internal/ConsoleRenderer.cs`: builds ANSI color prefix per line, appends reset suffix; no-op when no colors configured (backward compatible)
- Added `InternalsVisibleTo` in csproj for test project access to internal types
- Created `AnsiColorHelperTests.cs`: 20+ tests covering all 16 ConsoleColor mappings (fg/bg), truecolor generation, boundary values, reset code, colored QR output verification, precedence, default backward compatibility, edge cases (fg-only, bg-only, both)

**Key Design Decisions:**
- TrueColor takes precedence over ConsoleColor when `UseTrueColor = true`
- Color prefix applied per line (not per character) for performance
- All new properties default to null/false — zero behavior change for existing callers
- ANSI reset appended at end of each line to prevent terminal color bleed

**Validation:** Build succeeded (net8.0), 81 tests pass (all existing + 20 new ANSI tests).

---

### Global Dotnet Tool — ElBruno.QRCodeGenerator.Tool

**Date:** Added as part of batch QR generation tool request by Bruno Capuano.

**What was built:**
- New `src/ElBruno.QRCodeGenerator.Tool/` — global dotnet tool (`qrgen` command)
- Uses System.CommandLine (2.0.0-beta4.22272.1) for CLI parsing
- Routes to all 5 rendering packages: CLI (console), Svg, Image (PNG/JPEG), Ascii, Pdf
- Supports 11 options: `--format`, `--output`, `--module-size`, `--fg`, `--bg`, `--error-correction`, `--invert`, `--quality`, `--ascii-style`, `--title`
- PackAsTool=true, ToolCommandName=qrgen, single-target net8.0

**Architecture:**
- `Program.cs` — Minimal entry point, delegates to GenerateCommand
- `GenerateCommand.cs` — RootCommand builder + Execute handler routing to per-format handlers
- Each format handler maps CLI options to the target package's options classes and enums
- Error correction mapping handles both L/M/Q/H (CLI/SVG/Image) and Low/Medium/Quartile/High (Ascii/Pdf) enum styles
- File formats (png/jpeg/pdf) require `--output`; text formats (console/svg/ascii) default to stdout

**File Paths:**
- `src/ElBruno.QRCodeGenerator.Tool/ElBruno.QRCodeGenerator.Tool.csproj`
- `src/ElBruno.QRCodeGenerator.Tool/Program.cs`
- `src/ElBruno.QRCodeGenerator.Tool/GenerateCommand.cs`
- `src/tests/ElBruno.QRCodeGenerator.Tool.Tests/GenerateCommandTests.cs`

**Dependencies:** System.CommandLine + ProjectReferences to CLI, Svg, Image, Ascii, Pdf packages
**Tests:** 18 passing — format routing, file output validation (magic bytes), error cases, ECC mapping, ascii style mapping
**Validation:** Build succeeded (net8.0), all 18 tests green, manual `dotnet run` renders scannable QR in console.

---

### vCard N: (Structured Name) Field — Issue #1

**Date:** Added for issue #1, requested by Bruno Capuano.

**What was done:**
- Added `WithName(lastName, firstName, middleName?, prefix?, suffix?)` builder method to `VCardPayload`
- Emits `N:lastName;firstName;middleName;prefix;suffix` line before `FN:` in vCard output
- Null/missing optional components default to empty string per vCard 4.0 spec (RFC 6350)
- Bumped package version from 1.0.0 → 1.1.0

**Key Design Decisions:**
- `N:` line placed immediately before `FN:` — standard vCard field ordering convention
- Used nullable tuple `_structuredName` field — `N:` only emitted when `WithName()` is explicitly called, preserving backward compatibility
- All 5 components always emitted (even if empty) to produce fully compliant `N:` property

**File Paths:**
- `src/ElBruno.QRCodeGenerator.Payloads/VCardPayload.cs` — WithName() method + GetPayloadString() N: emission
- `src/ElBruno.QRCodeGenerator.Payloads/ElBruno.QRCodeGenerator.Payloads.csproj` — Version bump to 1.1.0

**Branch:** `squad/1-vcard-name-field`
**Validation:** Build succeeded (0 errors), commit pushed to origin.

---