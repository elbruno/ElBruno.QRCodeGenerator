# Squad Decisions

## Active Decisions

### 2026-03-26: Keep QRCoder Dependency

**Owner:** Neo (Architect)  
**Date:** 2026-03-26  
**Status:** Decided  
**Impact:** All ElBruno.QRCodeGenerator packages  
**Requested by:** Bruno Capuano

**Decision:** Keep QRCoder. Do not replace with custom QR encoder.

**Rationale:**
1. **Cost/benefit is terrible.** 2,000-3,000 LOC of complex mathematical code to eliminate 1 dependency that is itself zero-dependency and MIT licensed.
2. **Our value proposition is rendering, not encoding.** We add value through CLI/SVG/Image output. The QR encoding is commodity infrastructure.
3. **Risk of subtle bugs.** A bad Reed-Solomon implementation produces QR codes that look right but don't scan.
4. **QRCoder is healthy.** MIT license, 14M+ NuGet downloads, actively maintained, zero transitive dependencies.
5. **.NET devs don't care.** Zero-dep is a non-feature for our target audience; well-maintained transitive dependencies are acceptable.
6. **Escape hatch exists.** If QRCoder ever becomes unmaintained, we can vendor (copy) its source since it's MIT licensed.

**Current Dependency Surface:** ~15 lines
- `QRCodeGenerator` instantiation
- `CreateQrCode(string, ECCLevel)`
- `QRCodeData.ModuleMatrix` read
- `DataTooLongException` handling

**Replacement Effort (if ever needed):** 10-14 dev-days + high risk

**Next Steps:** None — document this decision and move forward. Revisit only if QRCoder becomes unmaintained or licensing changes.

---

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
