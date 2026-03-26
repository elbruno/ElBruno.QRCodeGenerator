# ElBruno.QRCodeGenerator — 5-Minute Video Overview Script

## Goal

A short, high-energy walkthrough of the repo: hook on camera, quick tour of the project, code + demos, and a closing CTA for feedback.

## Total Duration

~5:00

---

## 0:00 – 0:25 | Camera-Only Hook (Face Cam)

**Visual:** Full camera, fast pace, excited tone.

**Script:**
"What if you could generate QR codes in .NET in literally any format you need—ASCII for terminals, images for apps, payload builders for real-world scenarios, and even a CLI and tool ready to go? 🚀

In the next 5 minutes, I’ll show you one repo that does all of that: `ElBruno.QRCodeGenerator`. Let’s go!"

**On-screen text (big captions):**

- "One repo"
- "Multiple QR formats"
- "CLI + Tool + Payloads"

---

## 0:25 – 1:10 | Repo Overview (Screen Capture)

**Visual:** Open repo root in VS Code.

**Script:**
"This repo is organized by focused projects under `src/`.
You have:

- `ElBruno.QRCodeGenerator.Ascii` for terminal-friendly QR output,
- `ElBruno.QRCodeGenerator.Image` for image generation,
- `ElBruno.QRCodeGenerator.Payloads` for smart QR payload builders,
- plus CLI and tool projects to run it from command line.

And yes—there are dedicated test projects for each area, which is exactly what you want in a reusable library."

**Show quickly:**

- `src/`
- `tests/`
- `samples/`

---

## 1:10 – 2:00 | Code Peek #1: ASCII QR (Screen Capture)

**Visual:** Open `src/ElBruno.QRCodeGenerator.Ascii/AsciiQRCode.cs` and related options file.

**Script:**
"First up: ASCII QR generation. This is super useful for logs, terminal tools, SSH sessions, or quick debugging.
The API is clean: configure options, pass text, render. Done.

What I like here is the separation between options, renderer, and QR generation classes—it keeps the API simple for consumers and maintainable for contributors."

**On-screen text:**

- "ASCII output"
- "Great for terminal apps"
- "Simple options-driven API"

---

## 2:00 – 2:50 | Code Peek #2: Image QR (Screen Capture)

**Visual:** Open `src/ElBruno.QRCodeGenerator.Image/ImageQRCode.cs` and options.

**Script:**
"Need PNG-style output for web or mobile? That’s covered in the Image project.
Same concept: structured options, error correction level support, and rendering through dedicated classes.

This consistency across modules is gold—once you learn one project, the others feel familiar."

**On-screen text:**

- "Image output"
- "Consistent API design"

---

## 2:50 – 3:40 | Payloads = Real-World Use Cases (Screen Capture)

**Visual:** Open `src/ElBruno.QRCodeGenerator.Payloads/` and highlight payload classes.

**Script:**
"This is where it gets practical. The `Payloads` project includes builders for common QR scenarios:

- URL
- Email
- SMS
- Geo location
- WiFi
- vCard

So instead of hand-crafting QR payload strings every time, you use typed payload models and generate valid content faster—with fewer mistakes."

**On-screen text:**

- "Typed payload builders"
- "Less string-hacking"
- "Faster + safer"

---

## 3:40 – 4:20 | Demos & Samples (Screen Capture)

**Visual:** Open `src/samples/` and quickly run/show sample folders.

**Script:**
"There are multiple sample apps included, like:

- `AsciiQRCodeDemo`
- `ImageQRCodeDemo`
- `PayloadsDemo`
- and even `PdfQRCodeDemo` and `SvgQRCodeDemo`

This makes onboarding super easy: clone, run a sample, and adapt it to your app in minutes."

**On-screen text:**

- "Samples included"
- "Copy → Adapt → Ship"

---

## 4:20 – 5:00 | Camera-Only Outro + CTA

**Visual:** Back to face cam, friendly energy.

**Script:**
"And that’s `ElBruno.QRCodeGenerator` in 5 minutes: modular, practical, and ready for real projects.

If you want, I can do a follow-up deep dive on one module—ASCII, Image, or Payloads—and build a mini app live.

Drop a comment with what you want next, star the repo if this helped, and share this with one .NET dev who still hardcodes QR payloads. 😄 See you in the next one!"

**On-screen text:**

- "Which deep dive next?"
- "Comment below"
- "⭐ Star the repo"

---

## Quick Recording Tips (Optional)

- Keep cuts fast (3–8 seconds per visual beat).
- Use captions for key phrases.
- Add zoom-ins on file names and class names.
- Keep background music low and upbeat.
- Pin one top comment with repo link + timestamps.
