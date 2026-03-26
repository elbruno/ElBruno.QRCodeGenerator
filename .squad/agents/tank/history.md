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

**Impact on Tank's work:**
- Test suite now resides in `src/tests/`
- Sample apps in `src/samples/` for reference
- All test paths updated in project files
- CI/CD workflows reflect new structure

---

### 2026-03-26: Initial Test Suite Creation

**Created comprehensive xUnit test suite** for ElBruno.QRCodeGenerator.CLI library:
- **QRCodeTests.cs**: 14 tests covering main API (`QRCode.Generate()` and `QRCode.Print()`)
  - Happy path tests (simple text, URLs)
  - Error handling (null, empty, whitespace input)
  - Options validation (all error correction levels, inverted colors, quiet zones)
  - Edge cases (Unicode, very long text, consistency)
- **QRCodeOptionsTests.cs**: 6 tests for configuration class
  - Default values verification (ErrorCorrection=M, InvertColors=false, QuietZoneSize=1)
  - All enum values valid
  - Property setters work correctly
- **ConsoleRendererTests.cs**: 7 tests for console rendering behavior
  - Unicode block character usage (▀ ▄ █)
  - Consistent line width
  - Inverted color behavior
  - Aspect ratio validation (2:1 for console)
  - Quiet zone border verification
  - Character validation (only valid console chars)

**Test Philosophy**:
- Arrange/Act/Assert pattern consistently applied
- Tests written against architecture spec (Neo's API design)
- Tests ready for Trinity's implementation (may have compilation errors until source is complete)
- Focused on observable behavior through public API
- ConsoleRenderer tested indirectly (it's internal)

**Key File Paths**:
- `src/tests/ElBruno.QRCodeGenerator.CLI.Tests/QRCodeTests.cs`
- `src/tests/ElBruno.QRCodeGenerator.CLI.Tests/QRCodeOptionsTests.cs`
- `src/tests/ElBruno.QRCodeGenerator.CLI.Tests/ConsoleRendererTests.cs`
- Architecture reference: `.squad/decisions/inbox/neo-architecture.md`

**Build Status**: Tests created successfully. Source implementation in progress by Trinity (expected compilation errors until complete).
