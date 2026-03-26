# Skill: ElBruno .NET Repository Conventions

**Confidence:** high
**Domain:** .NET, NuGet, GitHub Actions, repository structure
**Applies when:** Setting up a new .NET library repository or reviewing repo structure for ElBruno projects

## Pattern

### Repository Structure
- Root: `README.md`, `LICENSE` (MIT), `Directory.Build.props`, `global.json`, `.gitignore`, `{ProjectName}.slnx`
- `src/` — libraries (`{Owner}.{ProjectName}.{Package}/`), `tests/`, `samples/`
- `docs/` — all documentation except README.md and LICENSE
- `images/` — all images including `nuget_logo.png`
- `.github/workflows/` — CI/CD

### Solution & Projects
- `.slnx` format (XML-based), not `.sln`
- Libraries: multi-target `net8.0;net10.0`
- Tests: `net8.0` only, xUnit, naming `{Lib}.Tests`, location `src/tests/`
- Tools: `net8.0` only, `<PackAsTool>true</PackAsTool>`
- Package naming: `{Owner}.{ProjectName}.{Feature}`
- All packable projects include `nuget_logo.png` via: `<None Include="$(MSBuildThisFileDirectory)..\..\images\nuget_logo.png" Pack="true" PackagePath="" />`

### Directory.Build.props (repo root)
- `LangVersion=latest`, `Nullable=enable`, `ImplicitUsings=enable`
- Code analysis: `EnforceCodeStyleInBuild`, `EnableNETAnalyzers`, `AnalysisLevel=latest`
- NuGet: Authors=Bruno Capuano (ElBruno), MIT license, dynamic copyright year, `PackageIcon=nuget_logo.png`
- Repo: `RepositoryUrl`, `RepositoryType=git`, `PublishRepositoryUrl=true`

### global.json
- SDK version with `"rollForward": "latestMajor"`

### CI Build (build.yml)
- Triggers: push/PR to main
- Steps: checkout → setup-dotnet (8.0.x) → restore → build → test
- Use `-p:TargetFrameworks=net8.0` for restore/build, `--framework net8.0` for test
- Solution-level commands

### NuGet Publishing (publish.yml)
- **Trusted publishing via OIDC** — no API keys as secrets
- `NuGet/login@v1` for OIDC token exchange
- GitHub environment `release` with `NUGET_USER` secret
- `permissions: id-token: write, contents: read`
- Triggers: release published + workflow_dispatch
- Version: from tag (strip `v`), input, or csproj fallback; validated with `^[0-9]+\.[0-9]+\.[0-9]+`
- Each packable project gets its own `dotnet pack` line
- Push with `--skip-duplicate`

### README.md
- Title → badges (CI, NuGet, MIT, stars) → tagline with emoji → description → packages table → per-package sections → building → docs → license → author → acknowledgments
- Use `dotnet add package` for installation (not `<PackageReference>` XML)
- Author links: elbruno.com, @inthelabs (YouTube, LinkedIn, Twitter), inthelabs.dev (podcast)

## Evidence

Applied in: ElBruno.QRCodeGenerator, ElBruno.LocalEmbeddings, ElBruno.LocalLLMs

## Usage

Copy `.github/copilot-instructions.md` from this repo into any new ElBruno .NET library repository. GitHub Copilot will automatically read and apply these conventions.
