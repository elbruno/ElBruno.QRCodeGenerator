# Project Context

- **Owner:** Bruno Capuano
- **Project:** ElBruno.QRCodeGenerator.CLI — A .NET library that creates QR codes displayed in the console using console characters. Published to NuGet.
- **Stack:** C#, .NET 8/10, xUnit, NuGet, GitHub Actions
- **Created:** 2026-03-26

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

### 2026-03-26: Library Expansion Plan Complete

**Implementation Plan Document Created:**
- File: `docs/implementation-plan.md` — comprehensive, actionable roadmap for 7 packages across 3 tiers
- **Tier 1 (Payloads, SVG):** Zero external rendering dependencies, pure string output, maximum reusability, ships first
- **Tier 2 (Image, Tool, ANSI):** Image requires dependency decision (ImageSharp vs SkiaSharp), Tool integrates all renderers, ANSI enhances CLI v1.1 with color support
- **Tier 3 (PDF, ASCII):** Future if demand exists

**Key Architectural Principles Documented:**
- All packages reuse QRCoder bool[,] matrix — single source of truth
- Payloads layer is pure string formatting, never needs rendering
- SVG uses pure XML generation, zero binary dependencies
- Image package deferred dependency decision to implementation time
- Tool unifies CLI, SVG, Image into batch processor with `qrgen` command
- Core extraction deferred until after Image ships and duplication patterns clear (YAGNI)

**Plan includes per-package:**
- Exact project structure (file layout, namespaces)
- Complete public API signatures with examples
- Dependency specifications
- Test strategy and coverage targets
- NuGet metadata templates
- Build order and integration points
- Risk mitigation and success metrics

**Impact:** Team can now pick any Tier 1 package and execute independently. Plan is reference-quality for future sessions.

**Next decision:** ImageSharp vs SkiaSharp for Image renderer (deferred to implementation phase).

### 2026-03-26: Repository Renamed and Core Architecture Decision

**Repository Rename:**
- Repository renamed from `ElBruno.QRCodeGenerator.CLI` to `ElBruno.QRCodeGenerator` to support multiple future packages
- Created new `ElBruno.QRCodeGenerator.slnx` solution file (old `ElBruno.QRCodeGenerator.CLI.slnx` to be deleted)
- Updated `Directory.Build.props` RepositoryUrl from `/ElBruno.QRCodeGenerator.CLI` to `/ElBruno.QRCodeGenerator`
- Updated `.squad/team.md` and all agent charters to reflect new project scope
- **IMPORTANT**: The CLI NuGet package name remains `ElBruno.QRCodeGenerator.CLI` — only the repo/solution was renamed

**Architecture Decision: No Core Project (Yet)**
- Analyzed current CLI codebase: QRCode.cs, QRCodeOptions.cs, ConsoleRenderer.cs
- Current structure: QRCoder library → bool[,] matrix → Unicode block character renderer
- CLI-specific logic: ConsoleRenderer (▀ ▄ █ characters), Print() method, InvertColors option
- Potentially shared logic: QRCoder integration (~20 lines), ErrorCorrectionLevel enum
- **Decision: Do NOT create ElBruno.QRCodeGenerator.Core now** — wait until Image library exists
- **Rationale**: YAGNI principle, minimal duplication (<20 lines), options will likely diverge, easier to extract after seeing real patterns
- **When to revisit**: After Image library is implemented and duplication patterns are clear

**Vision for Project:**
- `ElBruno.QRCodeGenerator.CLI` → Console/terminal QR codes (exists)
- `ElBruno.QRCodeGenerator.Image` → PNG/SVG QR images (future)
- `ElBruno.QRCodeGenerator.Core` → Shared logic (only if needed after Image exists)

**Key Files:**
- Solution: `ElBruno.QRCodeGenerator.slnx` (new)
- Decision: `.squad/decisions/inbox/neo-core-architecture.md`

### 2026-03-26: Project Architecture Scaffolded

**Architecture Decisions Made:**
- **QR Encoding Engine:** Using QRCoder NuGet package (v1.6.0) - battle-tested, MIT licensed, 50M+ downloads. No need to implement QR spec from scratch.
- **Public API Design:** Simple fluent API with `QRCode.Print()` and `QRCode.Generate()` static methods. Options pattern for configuration.
- **Console Rendering:** Unicode block characters (█ ▀ ▄) for 2:1 aspect ratio correction - makes QR codes scannable in terminals.
- **Multi-Targeting:** net8.0 (LTS) and net10.0 (latest) for maximum compatibility and modern support.

**Project Structure:**
```
ElBruno.QRCodeGenerator.CLI/
├── src/ElBruno.QRCodeGenerator.CLI/  # Main library
├── tests/ElBruno.QRCodeGenerator.CLI.Tests/  # xUnit tests
├── samples/BasicQRCode/  # Sample console app
├── docs/  # Documentation
├── Directory.Build.props  # Shared MSBuild properties
├── global.json  # SDK 10.0.0 with rollForward
└── ElBruno.QRCodeGenerator.CLI.sln
```

**Key Files Created:**
- Library csproj: Multi-target (net8.0;net10.0), NuGet metadata, XML docs enabled, QRCoder dependency
- Test csproj: xUnit, coverlet, net8.0 target
- Sample csproj: Console app, net8.0, references library
- Directory.Build.props: Nullable enabled, latest C#, code analysis, repo metadata
- global.json: Locks SDK to 10.0.0 with latestMinor rollForward

**NuGet Configuration:**
- PackageId: ElBruno.QRCodeGenerator.CLI
- Author: Bruno Capuano (ElBruno)
- Tags: qrcode, qr, console, cli, terminal, unicode, barcode
- Symbol packages enabled (snupkg) for debugging
- README.md included in package

**Success Criteria Met:**
✅ `dotnet restore` completed successfully
✅ All 3 projects added to solution (library, tests, sample)
✅ Solution builds (verified via restore)
✅ Directory structure matches ElBruno.LocalLLMs pattern

**Next Steps for Team:**
- Trinity: Implement core QR rendering logic (QRCode.cs, ConsoleRenderer.cs)
- Tank: Write unit tests for rendering, integration tests
- Dozer: Expand README with examples, create sample Program.cs
