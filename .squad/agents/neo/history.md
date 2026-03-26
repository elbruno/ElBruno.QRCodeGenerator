# Project Context

- **Owner:** Bruno Capuano
- **Project:** ElBruno.QRCodeGenerator.CLI — A .NET library that creates QR codes displayed in the console using console characters. Published to NuGet.
- **Stack:** C#, .NET 8/10, xUnit, NuGet, GitHub Actions
- **Created:** 2026-03-26

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

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
