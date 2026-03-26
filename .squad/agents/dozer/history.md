# Project Context

- **Owner:** Bruno Capuano
- **Project:** ElBruno.QRCodeGenerator — A suite of .NET libraries for generating QR codes. Currently ships ElBruno.QRCodeGenerator.CLI for console rendering. Published to NuGet.
- **Stack:** C#, .NET 8/10, xUnit, NuGet, GitHub Actions
- **Created:** 2026-03-26

## Learnings

### 2026-03-26: Project Restructuring — Folder Reorganization

**Structural Change — All team members aware:**
- Tests moved from `tests/` → `src/tests/`
- Samples moved from `samples/` → `src/samples/`
- Stale ElBruno.QRCodeGenerator.CLI.slnx deleted
- All references updated (solutions, projects, CI/CD workflows)
- Build passes, 35 tests green, changes committed

**Impact on Dozer's work:**
- Sample apps now in `src/samples/` (may reference in documentation)
- All project paths updated
- CI/CD workflows stable and validated

---

### Project Structure Evolution (2026-03-26)
- Repository renamed from ElBruno.QRCodeGenerator.CLI to ElBruno.QRCodeGenerator (umbrella project)
- ElBruno.QRCodeGenerator.CLI is now one package within the umbrella
- Future packages planned: ElBruno.QRCodeGenerator.Image (images), ElBruno.QRCodeGenerator.Core (shared)
- NuGet package name stays ElBruno.QRCodeGenerator.CLI (only repo/solution renamed)
- README now features a "Packages" table showing current + planned packages
- CHANGELOG.md updated with umbrella project context, organized by package

### Documentation Architecture
- README structure follows ElBruno standard: badges → tagline → features → install → quick start → API reference → samples → docs → author
- NuGet badge URLs use the package ID as-is: `ElBruno.QRCodeGenerator.CLI`
- Package metadata in .csproj drives NuGet display (Version, Description, Tags, ReadmeFile)
- Created `docs/nuget-logo-prompt.md` with AI image generator prompts (3 variations: minimal, detailed, playful)

### API Surface
- Core API: `QRCode.Generate(text, options)` returns string; `QRCode.Print(text, options)` outputs directly
- Configuration class: `QRCodeOptions` with three properties: `ErrorCorrection`, `InvertColors`, `QuietZoneSize`
- Error levels as enum: `L` (7%), `M` (15%), `Q` (25%), `H` (30%)
- Exceptions thrown: `ArgumentNullException`, `ArgumentException` (text validation), wraps `DataTooLongException` from QRCoder

### Sample Code Strategy
- Samples are self-contained console apps in `src/samples/` folder
- Each sample demonstrates one distinct capability (e.g., BasicQRCode shows simple usage + options)
- Samples should be runnable with `dotnet run` after `cd`

### Publishing Documentation
- Keep NuGet publishing guide in `docs/publishing.md`
- Include version management, API key security, git tagging workflow
- Reference GitHub Actions for CI/CD automation
- Updated all references from old repo name to new (ElBruno.QRCodeGenerator)

### File Organization
- README.md and LICENSE at repo root (not in docs/)
- CHANGELOG.md at repo root with dates and version sections, organized by package
- docs/ folder contains guides (publishing.md, nuget-logo-prompt.md, etc.)
- src/samples/ contains example applications


