# Publishing to NuGet

Guide for publishing ElBruno.QRCodeGenerator packages to NuGet.

This guide focuses on the ElBruno.QRCodeGenerator.CLI package. Future packages (Image, Core) will follow similar workflows.

## Prerequisites

- .NET SDK (8.0 or later)
- NuGet account with push permissions
- API key from https://www.nuget.org/account/api-keys

## Building the Package

### Step 1: Update Version

Edit `src/ElBruno.QRCodeGenerator.CLI/ElBruno.QRCodeGenerator.CLI.csproj`:

```xml
<Version>1.0.1</Version>
```

Update the version following [Semantic Versioning](https://semver.org):
- `MAJOR.MINOR.PATCH`
- Increment PATCH for bug fixes
- Increment MINOR for new backwards-compatible features
- Increment MAJOR for breaking changes

### Step 2: Update CHANGELOG.md

Add a new section at the top of the CHANGELOG.md at the repository root with the version and date:

```markdown
## [1.0.1] - 2026-03-27

### Fixed
- Brief description of the fix
```

### Step 3: Build the Package

```bash
dotnet pack -c Release
```

This creates:
- `src/ElBruno.QRCodeGenerator.CLI/bin/Release/ElBruno.QRCodeGenerator.CLI.1.0.1.nupkg`
- `src/ElBruno.QRCodeGenerator.CLI/bin/Release/ElBruno.QRCodeGenerator.CLI.1.0.1.snupkg` (symbol package)

## Publishing to NuGet

### Step 1: Get API Key

1. Go to https://www.nuget.org/account/api-keys
2. Create or copy your API key
3. Keep this secure — don't commit it to the repository

### Step 2: Push Package

```bash
dotnet nuget push src/ElBruno.QRCodeGenerator.CLI/bin/Release/ElBruno.QRCodeGenerator.CLI.1.0.1.nupkg \
  --api-key <YOUR_API_KEY> \
  --source https://api.nuget.org/v3/index.json
```

The symbol package is automatically pushed along with the main package.

### Step 3: Verify on NuGet

1. Go to https://www.nuget.org/packages/ElBruno.QRCodeGenerator.CLI
2. Verify the new version appears (may take 5-10 minutes to index)
3. Check that package size, dependencies, and metadata are correct

## GitHub Actions CI/CD

The ElBruno.QRCodeGenerator repository includes GitHub Actions workflows that:
- Build and test on every push
- Publish to NuGet on version tags

### Using GitHub Actions to Publish

1. Create a git tag with the version:
   ```bash
   git tag v1.0.1
   git push origin v1.0.1
   ```

2. Push will trigger the publish workflow (if configured)

3. Monitor the workflow in `.github/workflows/`

**Note:** GitHub Actions requires `NUGET_API_KEY` secret in repository settings.

## Troubleshooting

### Package Already Exists

If you get an error that the package version already exists:
- The version has already been published
- Increment the version number and rebuild
- NuGet does not allow re-pushing the same version

### Authentication Failed

```
Response status code does not indicate success: 401 (Unauthorized).
```

- Verify your API key is correct
- Ensure your NuGet account has push permissions
- API keys can expire — get a new one from your account

### Package Validation Warnings

NuGet may warn about:
- Missing icon URL (optional)
- Missing repository URL (add to `.csproj`)
- Unsigned package (use code signing for production)

## Best Practices

1. **Test before publishing**: Run `dotnet test` locally first
2. **Update documentation**: Update README and CHANGELOG before publishing
3. **Tag releases**: Use git tags matching the version (e.g., `v1.0.1`)
4. **Semantic versioning**: Follow semver for version numbers
5. **Security**: Never commit API keys; use GitHub Secrets for CI/CD
6. **Changelog**: Document all changes for each release

## Further Reading

- [NuGet Publish Documentation](https://learn.microsoft.com/en-us/nuget/nuget-org/publish-a-package)
- [Semantic Versioning](https://semver.org)
- [Keep a Changelog](https://keepachangelog.com)
- [GitHub Actions Secrets](https://docs.github.com/en/actions/security-guides/encrypted-secrets)
