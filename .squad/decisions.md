# Squad Decisions

## Active Decisions

### 2026-03-26: Image Renderer Dependency — ImageSharp vs SkiaSharp

**Owner:** Neo (Architect)  
**Date:** 2026-03-26  
**Status:** Pending Implementation Phase  
**Impact:** ElBruno.QRCodeGenerator.Image package dependency

**Decision:** ImageSharp (Primary) with SkiaSharp as Fallback
- Pure .NET, cross-platform, modern API
- Licensing: Evaluate SixLabors terms early; fallback to SkiaSharp if needed
- Target timeline: Before Tier 2 implementation starts
- Performance target: <200ms per QR code generation
- Test cross-platform builds (net8.0, net10.0 on Windows/macOS/Linux)

**Next Steps:**
1. Review current SixLabors licensing terms before Tier 2
2. Clarify commercial license acceptability with Bruno/team
3. Implement chosen renderer with full PNG + JPEG support
4. Update README with setup requirements

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
