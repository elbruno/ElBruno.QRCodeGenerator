using Xunit;
using ElBruno.QRCodeGenerator.Ascii;

namespace ElBruno.QRCodeGenerator.Ascii.Tests;

public class AsciiQRCodeTests
{
    [Fact]
    public void Generate_WithText_ReturnsNonEmptyString()
    {
        var result = AsciiQRCode.Generate("https://github.com/elbruno");

        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void Generate_NullText_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => AsciiQRCode.Generate(null!));
    }

    [Fact]
    public void Generate_EmptyText_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => AsciiQRCode.Generate(""));
    }

    [Fact]
    public void Generate_WhitespaceText_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => AsciiQRCode.Generate("   "));
    }

    [Fact]
    public void Generate_BlockStyle_ContainsBlockCharacters()
    {
        var result = AsciiQRCode.Generate("test", asciiOptions: new AsciiOptions { Style = AsciiStyle.Block });

        Assert.Contains("██", result);
    }

    [Fact]
    public void Generate_HashStyle_ContainsHashCharacters()
    {
        var result = AsciiQRCode.Generate("test", asciiOptions: new AsciiOptions { Style = AsciiStyle.Hash });

        Assert.Contains("##", result);
    }

    [Fact]
    public void Generate_DotStyle_ContainsDotCharacters()
    {
        var result = AsciiQRCode.Generate("test", asciiOptions: new AsciiOptions { Style = AsciiStyle.Dot });

        Assert.Contains("●●", result);
        Assert.Contains("··", result);
    }

    [Fact]
    public void Generate_ShadeStyle_ContainsShadeCharacters()
    {
        var result = AsciiQRCode.Generate("test", asciiOptions: new AsciiOptions { Style = AsciiStyle.Shade });

        Assert.Contains("░░", result);
    }

    [Fact]
    public void Generate_CustomStyle_UsesCustomCharacters()
    {
        var result = AsciiQRCode.Generate("test", asciiOptions: new AsciiOptions
        {
            Style = AsciiStyle.Custom,
            DarkCharacter = "@@",
            LightCharacter = "--"
        });

        Assert.Contains("@@", result);
        Assert.Contains("--", result);
    }

    [Fact]
    public void Generate_WithHighErrorCorrection_ProducesLargerOutput()
    {
        var lowResult = AsciiQRCode.Generate("test",
            options: new AsciiQRCodeOptions { ErrorCorrection = AsciiErrorCorrectionLevel.Low });
        var highResult = AsciiQRCode.Generate("test",
            options: new AsciiQRCodeOptions { ErrorCorrection = AsciiErrorCorrectionLevel.High });

        Assert.True(highResult.Length >= lowResult.Length);
    }

    [Fact]
    public void Generate_VeryLongText_ThrowsArgumentException()
    {
        var longText = new string('A', 10000);

        Assert.Throws<ArgumentException>(() => AsciiQRCode.Generate(longText));
    }

    [Fact]
    public void Print_WithText_WritesToConsoleOut()
    {
        var originalOut = Console.Out;
        try
        {
            using var writer = new StringWriter();
            Console.SetOut(writer);

            AsciiQRCode.Print("test");

            var output = writer.ToString();
            Assert.False(string.IsNullOrEmpty(output));
            Assert.Contains("██", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void Generate_MultipleLines_HasCorrectStructure()
    {
        var result = AsciiQRCode.Generate("test");
        var lines = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.True(lines.Length > 1);
        // All lines should have the same length (uniform QR code)
        var firstLineLength = lines[0].Length;
        foreach (var line in lines)
        {
            Assert.Equal(firstLineLength, line.Length);
        }
    }
}
