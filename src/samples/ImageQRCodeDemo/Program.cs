using ElBruno.QRCodeGenerator.Image;

Console.WriteLine("=== Image QR Code Generation ===");
Console.WriteLine();

// Generate a PNG QR code
Console.WriteLine("--- PNG ---");
var pngBytes = ImageQRCode.ToPng("https://github.com/elbruno");
var pngPath = Path.Combine(Directory.GetCurrentDirectory(), "qrcode.png");
File.WriteAllBytes(pngPath, pngBytes);
Console.WriteLine($"PNG QR code saved to: {pngPath} ({pngBytes.Length:N0} bytes)");
Console.WriteLine();

// Generate a JPEG QR code with custom quality
Console.WriteLine("--- JPEG (quality 75) ---");
var jpegBytes = ImageQRCode.ToJpeg("https://github.com/elbruno", quality: 75);
var jpegPath = Path.Combine(Directory.GetCurrentDirectory(), "qrcode.jpg");
File.WriteAllBytes(jpegPath, jpegBytes);
Console.WriteLine($"JPEG QR code saved to: {jpegPath} ({jpegBytes.Length:N0} bytes)");
Console.WriteLine();

// Generate with custom colors
Console.WriteLine("--- Custom Colors ---");
var customBytes = ImageQRCode.Generate("Hello, World!",
    qrOptions: new ImageQRCodeOptions
    {
        ErrorCorrection = ImageErrorCorrectionLevel.H
    },
    imageOptions: new ImageOptions
    {
        ForegroundColor = "#003366",
        BackgroundColor = "#f0f0f0",
        ModuleSize = 15,
        QuietZone = 2
    });
var customPath = Path.Combine(Directory.GetCurrentDirectory(), "qrcode_custom.png");
File.WriteAllBytes(customPath, customBytes);
Console.WriteLine($"Custom QR code saved to: {customPath} ({customBytes.Length:N0} bytes)");

Console.WriteLine();
Console.WriteLine("Sample complete!");
