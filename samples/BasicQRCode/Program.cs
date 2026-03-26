using ElBruno.QRCodeGenerator.CLI;

Console.WriteLine("=== Basic QR Code Generation ===");
Console.WriteLine();
Console.WriteLine("Simple URL:");
QRCode.Print("https://github.com/elbruno");

Console.WriteLine();
Console.WriteLine("=== Custom Options ===");
Console.WriteLine();
Console.WriteLine("High error correction with larger quiet zone:");
QRCode.Print("Hello, World!", new QRCodeOptions
{
    ErrorCorrection = ErrorCorrectionLevel.H,
    QuietZoneSize = 2
});

Console.WriteLine();
Console.WriteLine("=== Light Terminal Theme ===");
Console.WriteLine();
Console.WriteLine("Inverted colors for light backgrounds:");
QRCode.Print("https://inthelabs.dev", new QRCodeOptions
{
    InvertColors = true,
    ErrorCorrection = ErrorCorrectionLevel.M
});

Console.WriteLine();
Console.WriteLine("Sample complete!");
