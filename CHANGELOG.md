# Changelog

All notable changes to the ElBruno.QRCodeGenerator project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
- .NET 8.0 and .NET 10.0 support
- Symbol package (.snupkg) with embedded sources
- Comprehensive XML documentation comments
