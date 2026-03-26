# Project Context

- **Owner:** Bruno Capuano
- **Project:** ElBruno.QRCodeGenerator.CLI — A .NET library that creates QR codes displayed in the console using console characters. Published to NuGet.
- **Stack:** C#, .NET 8/10, xUnit, NuGet, GitHub Actions
- **Created:** 2026-03-26

## Learnings

### Documentation Architecture
- README structure follows ElBruno standard: badges → tagline → features → install → quick start → API reference → samples → docs → author
- NuGet badge URLs use the package ID as-is: `ElBruno.QRCodeGenerator.CLI`
- Package metadata in .csproj drives NuGet display (Version, Description, Tags, ReadmeFile)

### API Surface
- Core API: `QRCode.Generate(text, options)` returns string; `QRCode.Print(text, options)` outputs directly
- Configuration class: `QRCodeOptions` with three properties: `ErrorCorrection`, `InvertColors`, `QuietZoneSize`
- Error levels as enum: `L` (7%), `M` (15%), `Q` (25%), `H` (30%)
- Exceptions thrown: `ArgumentNullException`, `ArgumentException` (text validation), wraps `DataTooLongException` from QRCoder

### Sample Code Strategy
- Samples are self-contained console apps in `samples/` folder
- Each sample demonstrates one distinct capability (e.g., BasicQRCode shows simple usage + options)
- Samples should be runnable with `dotnet run` after `cd`

### Publishing Documentation
- Keep NuGet publishing guide in `docs/publishing.md`
- Include version management, API key security, git tagging workflow
- Reference GitHub Actions for CI/CD automation

### File Organization
- README.md and LICENSE at repo root (not in docs/)
- CHANGELOG.md at repo root with dates and version sections
- docs/ folder contains guides (publishing.md, etc.)
- samples/ contains example applications

