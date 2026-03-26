# NuGet Logo Generation Prompts

This document contains AI image generator prompts for creating the ElBruno.QRCodeGenerator NuGet package icon.

## Icon Requirements

- **Size:** 128x128 pixels PNG
- **Format:** PNG with transparency (if applicable)
- **Style:** Modern, clean, developer-focused
- **Theme:** QR code motif

## Prompt Variations

### 1. Minimal / Professional

```
A clean, modern icon for a .NET library. Features a minimalist QR code made of square pixels in dark blue (#1E3A8A). The QR code is centered on a white or transparent background. Flat design, no gradients. Professional developer tool aesthetic. 128x128 pixels, square format, PNG.
```

**Best for:** Official packages, corporate environments, professional documentation

---

### 2. Detailed / Technical

```
A technical icon for a QR code generator library. A stylized QR code pattern in gradient blue-to-purple (#1E3A8A to #7C3AED) with subtle depth effect. The pixels should have rounded corners for a modern look. Include a small console/terminal cursor or bracket symbol in one corner to represent CLI output. Clean background, modern flat design with subtle shadows. 128x128 pixels, square format.
```

**Best for:** Feature-rich packages, showcasing both QR and console aspects

---

### 3. Playful / Creative

```
A fun, friendly icon for a developer tool. A QR code made of colorful square blocks (blues, purples, teals) arranged in a recognizable QR pattern. The blocks have a slight 3D effect like building blocks or console pixels. Modern, approachable style that appeals to developers. Transparent or light background. 128x128 pixels, square format, PNG.
```

**Best for:** Community projects, sample code, approachable developer tools

---

## Color Palette Suggestions

- **Primary Blue:** `#1E3A8A` (NuGet brand-aligned)
- **Accent Purple:** `#7C3AED` (modern, tech-forward)
- **Console Green:** `#10B981` (terminal aesthetic)
- **Teal:** `#06B6D4` (fresh, modern)

## Design Tips

1. **Keep it simple** - The icon appears at 32x32 in NuGet search results
2. **High contrast** - Ensure the icon is recognizable when small
3. **Avoid text** - Text becomes illegible at small sizes
4. **Test at multiple sizes** - View at 128px, 64px, and 32px
5. **Use transparency wisely** - Transparent backgrounds work well for NuGet

## Alternative Icon Ideas

If a QR code motif doesn't work:

- **Console + QR hybrid:** Terminal window with a QR code displayed inside
- **Abstract blocks:** Square pixel pattern suggesting both QR codes and terminal characters
- **Unicode blocks:** Use the actual Unicode block characters (█▀▄) from the library in an artistic arrangement
- **Bracket + QR:** Angle brackets `< >` surrounding a simplified QR pattern

## Testing Your Icon

After generating:

1. View at 32x32 pixels (NuGet search size)
2. View on both light and dark backgrounds
3. Check that it's recognizable as QR-code-related
4. Ensure it stands out in NuGet search results

## Image Generators

Recommended AI tools:
- **DALL-E 3** (via ChatGPT Plus or API)
- **Midjourney** (Discord bot)
- **Adobe Firefly** (Integrated with Creative Cloud)
- **Stable Diffusion** (Open source, local generation)

## Usage

Once generated:

1. Save as `icon.png` at 128x128 pixels
2. Add to the project root or `assets/` folder
3. Reference in `.csproj` with `<PackageIcon>icon.png</PackageIcon>`
4. Include in the NuGet package with `<None Include="icon.png" Pack="true" PackagePath="" />`
