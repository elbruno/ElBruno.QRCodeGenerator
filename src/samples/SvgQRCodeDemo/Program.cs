using ElBruno.QRCodeGenerator.Svg;

Console.WriteLine("=== SVG QR Code Generation ===");
Console.WriteLine();

// Generate a simple SVG QR code
Console.WriteLine("Simple URL (first 200 chars):");
var svg = SvgQRCode.Generate("https://github.com/elbruno");
Console.WriteLine(svg[..Math.Min(200, svg.Length)] + "...");
Console.WriteLine();

// Generate with custom colors
Console.WriteLine("=== Custom Colors ===");
Console.WriteLine();
var customSvg = SvgQRCode.Generate("Hello, World!", svgOptions: new SvgOptions
{
    ForegroundColor = "#003366",
    BackgroundColor = "#f0f0f0",
    ModuleSize = 8
});
Console.WriteLine($"Generated SVG with custom colors ({customSvg.Length} chars)");
Console.WriteLine();

// Save SVG to file
Console.WriteLine("=== Save to File ===");
var fileSvg = SvgQRCode.Generate("https://github.com/elbruno", svgOptions: new SvgOptions
{
    ModuleSize = 10,
    QuietZone = 2
});
var filePath = Path.Combine(Directory.GetCurrentDirectory(), "qrcode.svg");
File.WriteAllText(filePath, fileSvg);
Console.WriteLine($"SVG QR code saved to: {filePath}");

Console.WriteLine();
Console.WriteLine("Sample complete!");
