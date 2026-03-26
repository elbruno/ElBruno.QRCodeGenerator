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
