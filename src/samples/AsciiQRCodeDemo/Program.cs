using ElBruno.QRCodeGenerator.Ascii;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== ASCII QR Code Generation ===");
Console.WriteLine();

// Default Block style
Console.WriteLine("--- Block Style (default) ---");
AsciiQRCode.Print("https://github.com/elbruno");
Console.WriteLine();

// Hash style
Console.WriteLine("--- Hash Style ---");
AsciiQRCode.Print("https://github.com/elbruno",
    asciiOptions: new AsciiOptions { Style = AsciiStyle.Hash });
Console.WriteLine();

// Dot style
Console.WriteLine("--- Dot Style ---");
AsciiQRCode.Print("https://github.com/elbruno",
    asciiOptions: new AsciiOptions { Style = AsciiStyle.Dot });
Console.WriteLine();

// Custom characters
Console.WriteLine("--- Custom Characters ---");
AsciiQRCode.Print("Hello, World!",
    asciiOptions: new AsciiOptions
    {
        Style = AsciiStyle.Custom,
        DarkCharacter = "@@",
        LightCharacter = ".."
    });
Console.WriteLine();

// Save to text file
Console.WriteLine("--- Save to File ---");
var asciiArt = AsciiQRCode.Generate("https://github.com/elbruno",
    asciiOptions: new AsciiOptions
    {
        Style = AsciiStyle.Block,
        Border = true,
        QuietZone = 2
    });
var filePath = Path.Combine(Directory.GetCurrentDirectory(), "qrcode.txt");
File.WriteAllText(filePath, asciiArt);
Console.WriteLine($"ASCII QR code saved to: {filePath}");

Console.WriteLine();
Console.WriteLine("Sample complete!");
