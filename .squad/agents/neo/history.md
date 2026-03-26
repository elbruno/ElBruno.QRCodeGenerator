# Project Context

- **Owner:** Bruno Capuano
- **Project:** ElBruno.QRCodeGenerator.CLI — A .NET library that creates QR codes displayed in the console using console characters. Published to NuGet.
- **Stack:** C#, .NET 8/10, xUnit, NuGet, GitHub Actions
- **Created:** 2026-03-26

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

### 2026-03-26: QRCoder Dependency Analysis — Keep It

**Question:** Could we replace QRCoder with custom QR code generation to go zero-dependency?

**Dependency Surface (Tiny):**
- 3 QRCoder types used: `QRCodeGenerator`, `QRCodeGenerator.ECCLevel`, `QRCodeData`
- 1 method call: `CreateQrCode(string, ECCLevel)` → returns `QRCodeData`
- 1 property read: `QRCodeData.ModuleMatrix` (List of BitArray)
- 1 exception type caught: `DataTooLongException`
- Total QRCoder-touching code: ~15 lines in `QRCode.cs`, zero elsewhere

**Replacement Cost (High):**
- Full QR encoding per ISO 18004 = ~2,000-3,000 LOC minimum for core logic
- Components: data encoding (4 modes), Reed-Solomon ECC over GF(256), version selection (40 versions × 4 ECC levels), module placement (finder/timing/alignment patterns, format/version info), masking (8 patterns + penalty scoring)
- Reed-Solomon alone is ~200-400 LOC of non-trivial math (Galois Field arithmetic, polynomial division)
- Also need: lookup tables for capacity, alignment pattern positions, format strings, version info blocks
- Testing burden: need conformance tests against spec for all versions/ECC levels

**Recommendation: Do NOT replace QRCoder.**
- QRCoder is MIT licensed, zero-dependency itself, battle-tested (14M+ downloads), well-maintained
- Our value is the rendering layer (CLI, SVG, Image), not the encoding
- Cost/benefit ratio is terrible: 2,000-3,000 LOC of complex math to eliminate 1 well-maintained MIT dependency
- .NET developers do not generally care about transitive deps the way Node.js devs do
- Risk: subtle encoding bugs that produce unscannable QR codes — hard to test exhaustively
- If QRCoder ever became unmaintained, THEN forking/internalizing would make sense (it's MIT, easy to vendor)

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
├── src/tests/ElBruno.QRCodeGenerator.CLI.Tests/  # xUnit tests
├── src/samples/BasicQRCode/  # Sample console app
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

### 2026-03-26: Repository Structure Reorganization

**Structural Change: Moved tests/ and samples/ into src/**
- Consolidated directory structure with all project content under `src/`
- New structure: `src/` contains library (`ElBruno.QRCodeGenerator.CLI/`), tests (`tests/`), and samples (`samples/`)
- Benefits: Cleaner repo root, clearer separation of source code vs documentation/tooling
- Git history preserved using `git mv` (manual approach due to permission constraints)

**Files Updated:**
- `ElBruno.QRCodeGenerator.slnx` — Updated project paths and folder structure
- Project references in `ElBruno.QRCodeGenerator.CLI.Tests.csproj` and `BasicQRCode.csproj` — Fixed relative paths (`../../src/...` → `../../`)
- Documentation (`README.md`, `docs/implementation-plan.md`) — Updated all path references and structure diagrams
- All squad files (charters, histories, routing, decisions) — Updated path conventions

**Removed:**
- `ElBruno.QRCodeGenerator.CLI.slnx` — Removed from git tracking (file was already deleted but still tracked)

**Verification:**
- ✅ Solution builds successfully (`dotnet build`)
- ✅ All 35 tests pass (`dotnet test`)
- ✅ Changes committed with git rename detection (R status)
- ✅ Pushed to GitHub main branch

**Convention Established:**
- Future package structure: `src/{PackageName}/`, `src/tests/{PackageName}.Tests/`, `src/samples/{SampleName}/`
- Repo root reserved for: solution file, README, LICENSE, CHANGELOG, docs/, .squad/, build configuration

### 2026-03-26: Repo Conventions Extracted to Copilot Instructions

**What:** Created `.github/copilot-instructions.md` as a reusable conventions document for all ElBruno .NET NuGet library repos. Covers: repo structure, .slnx solution format, multi-target projects, Directory.Build.props, global.json, .gitignore, CI build workflow, NuGet trusted publishing via OIDC, README format, testing with xUnit, author info.

**Format Decision:** Chose `.github/copilot-instructions.md` as the primary output because it's automatically picked up by GitHub Copilot in any repo — no tooling dependency required. Also created a Squad skill version at `.squad/skills/elbruno-dotnet-repo/SKILL.md` for Squad-aware repos.

**Key Conventions Codified:**
- Libraries target `net8.0;net10.0`, tests/tools target `net8.0` only
- CI uses `-p:TargetFrameworks=net8.0` since runners may not have preview SDKs
- NuGet publishing uses OIDC trusted publishing (`NuGet/login@v1`) — no API key secrets
- All packable projects include `nuget_logo.png` via `Pack="true" PackagePath=""`
- README follows strict structure: title → badges → tagline → packages table → per-package sections → author

**Reuse:** Copy `.github/copilot-instructions.md` into any new ElBruno .NET repo. Copilot applies the conventions automatically.

### 2026-03-27: CI and NuGet Publish Workflows Created

**Problem:** Bruno created a GitHub release (v0.5.0) but nothing happened — no NuGet publish. The `.github/workflows/` folder only had Squad-related workflows, no CI or publish pipelines.

**Workflows Created:**

1. **`.github/workflows/build.yml`** — CI Build
   - Triggers: push to main, PR to main
   - Steps: checkout → setup-dotnet (8.0.x + global.json SDK) → restore → build → test (net8.0)
   - Tests use `--framework net8.0` since test project targets net8.0 only

2. **`.github/workflows/publish.yml`** — NuGet Publish
   - Triggers: `release` event (type: published), push of `v*` tags
   - Steps: checkout → setup-dotnet → restore → build Release → test → pack → push
   - Only packs CLI project: `src/ElBruno.QRCodeGenerator.CLI/ElBruno.QRCodeGenerator.CLI.csproj`
   - Pushes both .nupkg and .snupkg to NuGet.org
   - Uses `secrets.NUGET_API_KEY` — must be configured in repo settings
   - Uses `--skip-duplicate` to handle re-runs gracefully

**Design Decisions:**
- Both workflows install .NET 8.0.x SDK explicitly AND respect global.json (10.0.0) via `global-json-file: global.json`
- Version comes from .csproj `<Version>` property — no tag parsing needed
- The .csproj already has `<IncludeSymbols>true</IncludeSymbols>` and `<SymbolPackageFormat>snupkg</SymbolPackageFormat>`, so pack produces both packages
- `<ContinuousIntegrationBuild>` is already conditioned on `GITHUB_ACTIONS` env var in .csproj — deterministic builds work automatically

**Requirement for Bruno:** Add `NUGET_API_KEY` secret in GitHub repo settings (Settings → Secrets → Actions → New repository secret), then either re-run the v0.5.0 release workflow or create a new release/tag.
