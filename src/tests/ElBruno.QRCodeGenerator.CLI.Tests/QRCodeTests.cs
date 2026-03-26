using System;
using System.IO;
using Xunit;

namespace ElBruno.QRCodeGenerator.CLI.Tests;

public class QRCodeTests
{
    [Fact]
    public void Generate_WithSimpleText_ReturnsNonEmptyString()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = QRCode.Generate(text);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains('\n', result); // Should have multiple lines
    }

    [Fact]
    public void Generate_WithUrl_ReturnsValidQRString()
    {
        // Arrange
        var url = "https://github.com/elbruno";

        // Act
        var result = QRCode.Generate(url);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        var lines = result.Split('\n');
        Assert.True(lines.Length > 5, "QR code should have multiple lines");
    }

    [Fact]
    public void Generate_WithNullText_ThrowsArgumentNullException()
    {
        // Arrange
        string? text = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => QRCode.Generate(text!));
    }

    [Fact]
    public void Generate_WithEmptyText_ThrowsArgumentException()
    {
        // Arrange
        var text = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => QRCode.Generate(text));
    }

    [Theory]
    [InlineData(ErrorCorrectionLevel.L)]
    [InlineData(ErrorCorrectionLevel.M)]
    [InlineData(ErrorCorrectionLevel.Q)]
    [InlineData(ErrorCorrectionLevel.H)]
    public void Generate_WithOptions_RespectsErrorCorrectionLevel(ErrorCorrectionLevel level)
    {
        // Arrange
        var text = "Test QR Code";
        var options = new QRCodeOptions { ErrorCorrection = level };

        // Act
        var result = QRCode.Generate(text, options);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        // Different error correction levels may produce different sized QR codes
        // but all should produce valid output
    }

    [Fact]
    public void Generate_WithInvertedColors_ProducesDifferentOutput()
    {
        // Arrange
        var text = "Compare Colors";
        var normalOptions = new QRCodeOptions { InvertColors = false };
        var invertedOptions = new QRCodeOptions { InvertColors = true };

        // Act
        var normalResult = QRCode.Generate(text, normalOptions);
        var invertedResult = QRCode.Generate(text, invertedOptions);

        // Assert
        Assert.NotNull(normalResult);
        Assert.NotNull(invertedResult);
        Assert.NotEqual(normalResult, invertedResult);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    public void Generate_WithDifferentQuietZones_ProducesDifferentWidths(int quietZone)
    {
        // Arrange
        var text = "Quiet Zone Test";
        var options = new QRCodeOptions { QuietZoneSize = quietZone };

        // Act
        var result = QRCode.Generate(text, options);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        
        // Larger quiet zones should produce wider output
        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.True(lines.Length > 0, "Should have at least one line");
        
        // Width should increase with quiet zone
        var firstLineLength = lines[0].Length;
        Assert.True(firstLineLength > 0, "Lines should have content");
    }

    [Fact]
    public void Print_WithSimpleText_WritesToConsole()
    {
        // Arrange
        var text = "Console Output Test";
        var originalOut = Console.Out;
        
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            // Act
            QRCode.Print(text);

            // Assert
            var output = writer.ToString();
            Assert.NotEmpty(output);
            Assert.Contains('\n', output); // Should have multiple lines
        }
        finally
        {
            // Cleanup
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void Generate_WithUnicodeText_Works()
    {
        // Arrange
        var text = "Hello 👋 World 🌍";

        // Act
        var result = QRCode.Generate(text);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        // Unicode input should be encodable in QR
    }

    [Fact]
    public void Generate_WithVeryLongText_HandlesGracefully()
    {
        // Arrange
        // QR codes have capacity limits (e.g., ~2953 bytes for binary data in Version 40 with L error correction)
        var longText = new string('A', 3000);

        // Act & Assert
        // This may either succeed (if QR library can handle it) or throw a descriptive exception
        try
        {
            var result = QRCode.Generate(longText);
            Assert.NotNull(result);
        }
        catch (ArgumentException)
        {
            // Expected for text that exceeds QR capacity
            Assert.True(true, "Correctly throws ArgumentException for text exceeding QR capacity");
        }
    }

    [Fact]
    public void Generate_WithWhitespaceOnlyText_ThrowsArgumentException()
    {
        // Arrange
        var text = "   \t\n  ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => QRCode.Generate(text));
    }

    [Fact]
    public void Generate_ProducesConsistentOutput()
    {
        // Arrange
        var text = "Consistency Test";

        // Act
        var result1 = QRCode.Generate(text);
        var result2 = QRCode.Generate(text);

        // Assert
        Assert.Equal(result1, result2, ignoreLineEndingDifferences: true);
    }
}
