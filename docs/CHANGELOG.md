# Changelog

All notable changes to the ElBruno.QRCodeGenerator project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.1.1] - 2026-11-14

### Changed
- Added .NET 11.0 support to all six library packages while retaining .NET 8.0 and .NET 10.0.
- Added .NET 11.0 support to the `qrgen` global tool while retaining its .NET 8.0 target.
- Replaced the PDF package's vulnerable PdfSharpCore/ImageSharp dependency graph with PDFsharp and the patched `System.Security.Cryptography.Pkcs` 8.0.2.
- Bundled an Arial-metric-compatible Liberation Sans font under the SIL Open Font License for portable PDF title rendering.

---

## ElBruno.QRCodeGenerator.CLI

## [1.0.0] - 2026-03-26

### Added
- Initial release of ElBruno.QRCodeGenerator.CLI
- QR code generation with QRCoder engine
- Unicode block character console rendering (2:1 aspect ratio correction)
- `QRCode.Generate()` method to get QR code as string
- `QRCode.Print()` method to print directly to console
- `QRCodeOptions` class for configuration
- Error correction levels: L (7%), M (15%), Q (25%), H (30%)
- Color inversion support for light terminal themes
- Adjustable quiet zone (border) configuration
- .NET 8.0, .NET 10.0, and .NET 11.0 support
- Symbol package (.snupkg) with embedded sources
- Comprehensive XML documentation comments
