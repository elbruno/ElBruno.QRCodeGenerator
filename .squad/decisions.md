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

### 2026-03-26: User Directive — Keep QRCoder (Confirmed)

**Owner:** Bruno Capuano  
**Date:** 2026-03-26  
**Status:** Confirmed  
**Impact:** All ElBruno.QRCodeGenerator packages

**Directive:** Keep QRCoder as the QR encoding dependency. Do not replace with custom implementation.

**Confirmation:** User request validated after Neo's analysis showing QRCoder is MIT licensed, zero-dependency, 14M+ downloads. Team's value proposition is rendering, not encoding.

---

### 2026-03-27: Slim README for NuGet Release

**Owner:** Dozer (DevRel)  
**Date:** 2026-03-27  
**Status:** Implemented  
**Impact:** README.md, docs/api-reference.md, NuGet presentation  
**Requested by:** Bruno Capuano

**Decision:** Reduce README.md from 288 lines to ~120 lines for cleaner NuGet package landing page.

**Changes Implemented:**
1. **Removed from README:** Full API reference, QRCodeOptions/ErrorCorrectionLevel definitions, config examples, error correction table, project tree, "Coming soon" package placeholders
2. **Created:** docs/api-reference.md (159 lines) with complete API documentation
3. **Kept in README:** Badges, description, packages table (CLI only), features, quick start, samples, building guide, docs links with api-reference.md

**Rationale:** NuGet landing pages benefit from brevity; detailed docs belong in separate files. 59% reduction maintains all core info while improving scannability.

**Result:** Commit 0836845 to main; repository ready for NuGet publication.

**Next Steps:** None — ready for release.

---

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
