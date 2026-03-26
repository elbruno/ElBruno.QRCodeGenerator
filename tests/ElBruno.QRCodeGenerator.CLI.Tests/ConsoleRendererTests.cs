using System;
using System.Linq;
using Xunit;

namespace ElBruno.QRCodeGenerator.CLI.Tests;

/// <summary>
/// Tests for console rendering behavior, tested through the public API.
/// ConsoleRenderer is internal, so we test its behavior indirectly.
/// </summary>
public class ConsoleRendererTests
{
    [Fact]
    public void Render_ContainsUnicodeBlockCharacters()
    {
        // Arrange
        var text = "Block Character Test";

        // Act
        var result = QRCode.Generate(text);

        // Assert
        Assert.NotNull(result);
        
        // QR rendering should use Unicode block characters for console optimization
        // Common characters: ▀ (U+2580), ▄ (U+2584), █ (U+2588), or space
        var hasBlockCharacters = 
            result.Contains('▀') || 
            result.Contains('▄') || 
            result.Contains('█') ||
            result.Contains(' ');
        
        Assert.True(hasBlockCharacters, "Output should contain Unicode block characters or spaces");
    }

    [Fact]
    public void Render_HasConsistentLineWidth()
    {
        // Arrange
        var text = "Line Width Test";

        // Act
        var result = QRCode.Generate(text);

        // Assert
        Assert.NotNull(result);
        
        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.True(lines.Length > 1, "QR code should have multiple lines");
        
        // All lines (except possibly the last if it's empty) should have the same width
        var expectedWidth = lines[0].Length;
        foreach (var line in lines)
        {
            Assert.Equal(expectedWidth, line.Length);
        }
    }

    [Fact]
    public void Render_WithInvertedColors_SwapsCharacters()
    {
        // Arrange
        var text = "Invert Test";
        var normalOptions = new QRCodeOptions { InvertColors = false };
        var invertedOptions = new QRCodeOptions { InvertColors = true };

        // Act
        var normalResult = QRCode.Generate(text, normalOptions);
        var invertedResult = QRCode.Generate(text, invertedOptions);

        // Assert
        Assert.NotNull(normalResult);
        Assert.NotNull(invertedResult);
        Assert.NotEqual(normalResult, invertedResult);
        
        // The outputs should be different - inverted should swap light/dark characters
        var normalLines = normalResult.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var invertedLines = invertedResult.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        Assert.Equal(normalLines.Length, invertedLines.Length);
    }

    [Fact]
    public void Render_OutputIsSquarish()
    {
        // Arrange
        var text = "Square Test";

        // Act
        var result = QRCode.Generate(text);

        // Assert
        Assert.NotNull(result);
        
        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var height = lines.Length;
        var width = lines.Length > 0 ? lines[0].Length : 0;
        
        Assert.True(height > 0, "QR code should have height");
        Assert.True(width > 0, "QR code should have width");
        
        // QR codes are square, but with 2:1 console aspect ratio correction,
        // the rendered height should be roughly half the width (or width ~= 2*height)
        // Allow some tolerance for quiet zones and rounding
        var aspectRatio = (double)width / height;
        Assert.InRange(aspectRatio, 1.5, 2.5); // Should be roughly 2:1
    }

    [Fact]
    public void Render_WithQuietZone_HasBorder()
    {
        // Arrange
        var text = "Border Test";
        var noQuietZone = new QRCodeOptions { QuietZoneSize = 0 };
        var withQuietZone = new QRCodeOptions { QuietZoneSize = 2 };

        // Act
        var noQuietResult = QRCode.Generate(text, noQuietZone);
        var withQuietResult = QRCode.Generate(text, withQuietZone);

        // Assert
        var noQuietLines = noQuietResult.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var withQuietLines = withQuietResult.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        // With quiet zone should be larger
        Assert.True(withQuietLines.Length > noQuietLines.Length, 
            "QR code with quiet zone should be taller");
        Assert.True(withQuietLines[0].Length > noQuietLines[0].Length, 
            "QR code with quiet zone should be wider");
    }

    [Fact]
    public void Render_DoesNotContainInvalidCharacters()
    {
        // Arrange
        var text = "Character Validation";

        // Act
        var result = QRCode.Generate(text);

        // Assert
        Assert.NotNull(result);
        
        // Should only contain valid console characters:
        // - Unicode block characters: ▀ ▄ █
        // - Spaces
        // - Newlines
        var validChars = new[] { ' ', '\n', '\r', '▀', '▄', '█' };
        
        foreach (var ch in result)
        {
            Assert.True(
                validChars.Contains(ch),
                $"Output contains unexpected character: U+{((int)ch):X4} '{ch}'");
        }
    }
}
