using ElBruno.QRCodeGenerator.Pdf;

Console.WriteLine("=== PDF QR Code Generation ===");
Console.WriteLine();

// 1. Generate a simple PDF with QR code
Console.WriteLine("--- Simple QR Code PDF ---");
var simplePdf = PdfQRCode.Generate("https://github.com/elbruno");
var simplePath = Path.Combine(Directory.GetCurrentDirectory(), "simple_qrcode.pdf");
File.WriteAllBytes(simplePath, simplePdf);
Console.WriteLine($"Simple QR code PDF saved to: {simplePath} ({simplePdf.Length} bytes)");
Console.WriteLine();

// 2. Custom page size (A4) and positioning
Console.WriteLine("--- Custom Page Size & Position ---");
PdfQRCode.Save("https://github.com/elbruno/ElBruno.QRCodeGenerator",
    Path.Combine(Directory.GetCurrentDirectory(), "custom_qrcode.pdf"),
    pdfOptions: new PdfOptions
    {
        PageWidth = 595,    // A4 width in points
        PageHeight = 842,   // A4 height in points
        ModuleSize = 3.0,
        X = 50,
        Y = 50
    });
Console.WriteLine("Custom A4 PDF with positioned QR code saved to: custom_qrcode.pdf");
Console.WriteLine();

// 3. QR code with title text
Console.WriteLine("--- QR Code with Title ---");
PdfQRCode.Save("https://github.com/elbruno",
    Path.Combine(Directory.GetCurrentDirectory(), "titled_qrcode.pdf"),
    options: new PdfQRCodeOptions
    {
        ErrorCorrection = PdfErrorCorrectionLevel.High
    },
    pdfOptions: new PdfOptions
    {
        Title = "Scan to visit ElBruno on GitHub",
        TitleFontSize = 16,
        ModuleSize = 3.0,
        ForegroundColor = "#003366"
    });
Console.WriteLine("Titled QR code PDF saved to: titled_qrcode.pdf");
Console.WriteLine();

// 4. Generate and inspect bytes
Console.WriteLine("--- Verify PDF Magic Bytes ---");
var pdfBytes = PdfQRCode.Generate("Hello, PDF QR Codes!");
var header = System.Text.Encoding.ASCII.GetString(pdfBytes, 0, Math.Min(8, pdfBytes.Length));
Console.WriteLine($"PDF header: {header}");
Console.WriteLine($"PDF size: {pdfBytes.Length} bytes");

Console.WriteLine();
Console.WriteLine("Sample complete!");
