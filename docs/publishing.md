# Publishing to NuGet

Guide for publishing ElBruno.QRCodeGenerator packages to NuGet.

This guide focuses on the ElBruno.QRCodeGenerator.CLI package. Future packages (Image, Core) will follow similar workflows.

## Prerequisites

- .NET SDK (8.0 or later)
- NuGet account with trusted publishing configured
- GitHub repository environment `release` with `NUGET_USER` secret

## How Publishing Works

This project uses **NuGet trusted publishing** via OIDC — no long-lived API keys needed. The GitHub Actions workflow exchanges an OIDC token for a temporary NuGet API key using the `NuGet/login@v1` action.

### Setup (one-time)

1. **NuGet side:** Configure trusted publishing for your package at https://www.nuget.org — link it to your GitHub repository and the `release` environment.
2. **GitHub side:** Create a repository environment named `release` in **Settings → Environments**.
3. **Add the secret:** In the `release` environment, add `NUGET_USER` with your NuGet username.

## Publishing via GitHub Actions (Recommended)

### Option 1: Create a Release

1. Update the version in `src/ElBruno.QRCodeGenerator.CLI/ElBruno.QRCodeGenerator.CLI.csproj`:
   ```xml
   <Version>1.0.1</Version>
   ```

2. Update `docs/CHANGELOG.md` with the new version.

3. Create a GitHub release with a tag like `v1.0.1`. The workflow automatically:
   - Extracts the version from the tag
   - Builds, tests, and packs
   - Authenticates via OIDC trusted publishing
   - Pushes to NuGet.org

### Option 2: Manual Dispatch

1. Go to **Actions → Publish to NuGet → Run workflow**
2. Optionally enter a version (defaults to the version in the `.csproj`)

## Building Locally

### Step 1: Build the Package

```bash
dotnet pack -c Release
```

This creates:
- `src/ElBruno.QRCodeGenerator.CLI/bin/Release/ElBruno.QRCodeGenerator.CLI.1.0.1.nupkg`
- `src/ElBruno.QRCodeGenerator.CLI/bin/Release/ElBruno.QRCodeGenerator.CLI.1.0.1.snupkg` (symbol package)

### Step 2: Verify on NuGet

1. Go to https://www.nuget.org/packages/ElBruno.QRCodeGenerator.CLI
2. Verify the new version appears (may take 5-10 minutes to index)
3. Check that package size, dependencies, and metadata are correct

## Troubleshooting

### Package Already Exists

If you get an error that the package version already exists:
- The version has already been published
- Increment the version number and rebuild
- NuGet does not allow re-pushing the same version

### OIDC Authentication Failed

- Ensure the GitHub environment is named exactly `release`
- Verify `NUGET_USER` secret is set in the `release` environment (not repo-level secrets)
- Confirm trusted publishing is configured on NuGet.org for this repo

### Invalid Version

The workflow validates the version from the release tag. Tags must follow `vMAJOR.MINOR.PATCH` format (e.g., `v1.0.1`).

## Best Practices

1. **Test before publishing**: Run `dotnet test` locally first
2. **Update documentation**: Update README and CHANGELOG before publishing
3. **Tag releases**: Use git tags matching the version (e.g., `v1.0.1`)
4. **Semantic versioning**: Follow semver for version numbers
5. **Trusted publishing**: No API keys to rotate — OIDC handles auth automatically
6. **Changelog**: Document all changes for each release

## Further Reading

- [NuGet Trusted Publishing](https://devblogs.microsoft.com/nuget/introducing-nuget-trusted-publishing/)
- [NuGet/login Action](https://github.com/NuGet/login)
- [Semantic Versioning](https://semver.org)
- [Keep a Changelog](https://keepachangelog.com)
