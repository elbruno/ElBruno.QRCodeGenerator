# Decisions — ElBruno.QRCodeGenerator

**Last Updated:** 2026-03-26

## 2026-03-26T12:07:38Z: User Directive

**By:** Bruno Capuano (via Copilot)

Project renamed from ElBruno.QRCodeGenerator.CLI to ElBruno.QRCodeGenerator as an umbrella repository. Main goals:
- CLI implementation (current)
- Image generation implementation (planned)
- Shared Core project (planned)

**Rationale:** User request to support multiple QR code generation formats.

---

## Umbrella Project Structure — Dozer (DevRel)

**Date:** 2026-03-26  
**Status:** Implemented

Repository renamed to **ElBruno.QRCodeGenerator** containing:
- **ElBruno.QRCodeGenerator.CLI** (current) — Console QR codes with Unicode blocks
- **ElBruno.QRCodeGenerator.Image** (planned) — Generate PNG/SVG images
- **ElBruno.QRCodeGenerator.Core** (planned) — Shared core functionality

**Documentation Changes:**
- README.md — Added packages table with current/planned packages
- CHANGELOG.md — Organized by package with umbrella header
- docs/publishing.md — Updated repo name references
- docs/README.md — Added umbrella structure context
- docs/nuget-logo-prompt.md — Created AI image generator prompts

**Key Principles:**
1. NuGet package name unchanged (ElBruno.QRCodeGenerator.CLI)
2. Repo name reflects umbrella (github.com/elbruno/ElBruno.QRCodeGenerator)
3. Keep CLI content intact
4. Future packages as "coming soon"

**Rationale:** Supports future growth without breaking existing users. Clearer branding, easier discoverability, maintains backward compatibility.

---

## ElBruno.QRCodeGenerator.Core Architecture — Neo (Lead)

**Date:** 2026-03-26  
**Status:** Recommended

**Question:** Should we create a Core project NOW or wait until Image library is needed?

**Analysis:** Examined current CLI architecture (QRCode.cs, QRCodeOptions.cs, ConsoleRenderer.cs). What's CLI-specific: ConsoleRenderer, Print(), InvertColors, QuietZoneSize (rendering-specific). What could be shared: QRCoder integration, ErrorCorrectionLevel, bool[,] matrix generation.

**Decision: Do NOT create Core now. Wait for the Image library.**

**Rationale:**
1. **YAGNI**: Premature abstraction risks wrong patterns
2. **Minimal Duplication**: Shared logic is <20 lines (QRCoder + matrix extraction)
3. **Options Diverge**: CLI needs (InvertColors, Unicode) vs Image needs (PixelsPerModule, Color, Format) are different
4. **API Clarity**: Consumers select library they need; Core adds overhead
5. **Easier Later**: When Image exists, real duplication patterns are visible

**When to Revisit:**
Create Core when Image library exists AND clear duplication patterns emerge (≥50+ lines identical code).

**Implementation:** Keep repository as ElBruno.QRCodeGenerator, keep package as ElBruno.QRCodeGenerator.CLI, update structural files. When Image is added, create alongside CLI. After both work, evaluate if Core makes sense.

---

## Project Rename — Coordinator Decision

**Date:** 2026-03-26T11:52:00Z  
**By:** Bruno Capuano (via Copilot)

Renamed project from `ElBruno.CLI.QRCodeGenerator` to `ElBruno.QRCodeGenerator.CLI`. All `.squad/` references updated. The NuGet package, namespaces, and all project files use `ElBruno.QRCodeGenerator.CLI` as the canonical name.

**Rationale:** User requested rename to match the new folder name convention.

---

## Architecture Decisions — Neo (Lead)

**Date:** 2026-03-26  
**Status:** Implemented

### 1. QR Encoding Library: Use QRCoder NuGet Package

**Decision:** Use QRCoder (https://github.com/codebude/QRCoder) as the QR encoding engine.

**Rationale:**
- Battle-tested, most popular .NET QR library (>50M downloads)
- MIT license - compatible with our project
- Handles all QR encoding complexity (error correction, encoding modes, version selection)
- Let's us focus on the unique value: console rendering with Unicode block characters
- Reduces maintenance burden and testing scope

**Alternative Considered:** Implement QR encoding from scratch  
**Rejected:** QR spec is complex (40+ versions, 4 error correction levels, multiple encoding modes). Not our core value proposition.

### 2. Public API Surface

**Namespace:** `ElBruno.QRCodeGenerator.CLI`

**Primary API:**
```csharp
namespace ElBruno.QRCodeGenerator.CLI;

// Main entry point - fluent API
public static class QRCode
{
    public static string Generate(string text, QRCodeOptions? options = null);
    public static void Print(string text, QRCodeOptions? options = null);
}

// Configuration
public class QRCodeOptions
{
    public ErrorCorrectionLevel ErrorCorrection { get; set; } = ErrorCorrectionLevel.M;
    public bool InvertColors { get; set; } = false;
    public int QuietZoneSize { get; set; } = 1; // Border size in modules
}

public enum ErrorCorrectionLevel
{
    L = 0,  // ~7% recovery
    M = 1,  // ~15% recovery
    Q = 2,  // ~25% recovery
    H = 3   // ~30% recovery
}
```

**Design Principles:**
- **Simple by default:** `QRCode.Print("https://example.com")` should "just work"
- **Fluent and discoverable:** Static methods for common cases
- **Console-optimized:** Use Unicode block characters (█ ▀ ▄ ▌ ▐) for 2:1 aspect ratio correction
- **String output:** `Generate()` returns string for flexibility
- **Direct print:** `Print()` writes directly to Console for convenience

### 3. Multi-Targeting Strategy

**Targets:** `net8.0;net10.0`

**Rationale:**
- .NET 8 is LTS (long-term support) - widest compatibility
- .NET 10 is latest - future-proof, show modern practices
- No special API needs - both targets can share 100% of code
- Console and QRCoder work on both

### 4. Project Structure

Following ElBruno.LocalLLMs pattern:

```
ElBruno.QRCodeGenerator.CLI/
├── .squad/                    # Squad metadata
├── src/
│   ├── ElBruno.QRCodeGenerator.CLI/
│   │   ├── ElBruno.QRCodeGenerator.CLI.csproj
│   │   ├── QRCode.cs          # Main public API
│   │   ├── QRCodeOptions.cs   # Configuration
│   │   └── Internal/          # Implementation details
│   │       └── ConsoleRenderer.cs
│   ├── tests/
│   │   └── ElBruno.QRCodeGenerator.CLI.Tests/
│   │       └── ElBruno.QRCodeGenerator.CLI.Tests.csproj
│   └── samples/
│       └── BasicQRCode/
│           ├── BasicQRCode.csproj
│           └── Program.cs
├── docs/
│   └── README.md
├── Directory.Build.props      # Shared MSBuild properties
├── global.json                # SDK version lock
├── ElBruno.QRCodeGenerator.CLI.sln
└── README.md
```

### 5. NuGet Package Configuration

**Package ID:** `ElBruno.QRCodeGenerator.CLI`  
**Author:** Bruno Capuano (ElBruno)  
**License:** MIT  
**Tags:** qrcode, qr, console, cli, terminal, unicode, barcode  
**Description:** Generate and display QR codes in the console using Unicode block characters. Optimized for terminal output with 2:1 aspect ratio correction.

**Metadata:**
- Include README.md as PackageReadmeFile
- Generate XML docs for IntelliSense
- Enable NuGet symbol packages for debugging

## Console Rendering Strategy — Trinity (Core Dev)

**Date:** 2026-03-26  
**Status:** Implemented

Implemented Unicode half-block character rendering for QR codes in the console using a 2:1 vertical compression strategy.

### Character Mapping

The renderer processes 2 rows of QR modules into 1 console line using these Unicode block characters:

- `█` (U+2588) Full Block: Both top and bottom modules dark
- `▀` (U+2580) Upper Half Block: Top module dark, bottom light
- `▄` (U+2584) Lower Half Block: Top module light, bottom dark
- ` ` (Space): Both modules light

### Technical Approach

1. **Matrix Extraction:** Convert QRCoder's `List<BitArray>` ModuleMatrix to `bool[,]` array
2. **Quiet Zone:** Expand matrix by adding border of light modules based on `QuietZoneSize`
3. **Row Pair Processing:** Iterate through matrix 2 rows at a time
4. **Character Selection:** Map (top, bottom) module pair to appropriate Unicode character
5. **Color Inversion:** Support light terminal themes by flipping module booleans

### Rationale

- **Aspect Ratio Correction:** Console characters are typically ~2x taller than wide. Processing 2 vertical modules per character produces square-appearing QR codes.
- **Density:** Achieves 2x vertical compression compared to using full block characters per module.
- **Compatibility:** Unicode block characters work in all modern terminals (Windows Terminal, iTerm2, Terminal.app, GNOME Terminal, etc.).
- **Scannability:** Verified that generated codes are scannable by standard QR readers.

## Testing Strategy — Tank (Tester)

**Date:** 2026-03-26  
**Status:** Implemented

Comprehensive test coverage strategy with 35 tests across 3 test classes.

### Test Structure

1. **QRCodeTests.cs** (14 tests)
   - Core functionality testing
   - Error handling validation
   - Options and configuration testing
   - Edge case coverage

2. **QRCodeOptionsTests.cs** (6 tests)
   - Configuration class validation
   - Default values verification
   - Property setter/getter testing

3. **ConsoleRendererTests.cs** (7 tests)
   - Rendering behavior validation (tested through public API)
   - Unicode character verification
   - Aspect ratio validation
   - Output consistency checks

### Key Testing Patterns

**Arrange/Act/Assert:** All tests follow AAA pattern for clarity and consistency.

**Theory-based Testing:** Using `[Theory]` with `[InlineData]` for testing multiple values:
- All error correction levels (L, M, Q, H)
- Different quiet zone sizes
- Various input scenarios

**Edge Cases Covered:**
- Null and empty inputs
- Whitespace-only text
- Unicode/emoji input
- Very long text (exceeds QR capacity)
- Consistency (same input = same output)

**Console Output Testing:** Using `StringWriter` to capture and validate `Console.Out` for the `Print()` method.

### Testing Philosophy

1. **Test behavior, not implementation:** Tests validate observable behavior through the public API
2. **Internal components tested indirectly:** `ConsoleRenderer` is internal, so we test its behavior through `QRCode.Generate()`
3. **Spec-driven development:** Tests written from Neo's architecture spec before implementation complete
4. **No brittle tests:** Tests validate correctness without hardcoding exact output strings

### Test Coverage Goals

- ✅ All public methods (`Generate`, `Print`)
- ✅ All configuration options
- ✅ All error conditions
- ✅ Console rendering characteristics
- ✅ Unicode/special character handling
- ✅ Edge cases and boundary conditions
