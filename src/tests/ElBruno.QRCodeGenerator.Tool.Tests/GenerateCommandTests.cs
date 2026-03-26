using ElBruno.QRCodeGenerator.Ascii;
using ElBruno.QRCodeGenerator.CLI;
using ElBruno.QRCodeGenerator.Image;
using ElBruno.QRCodeGenerator.Pdf;
using ElBruno.QRCodeGenerator.Svg;
using ElBruno.QRCodeGenerator.Tool;
using Xunit;

namespace ElBruno.QRCodeGenerator.Tool.Tests;

public class GenerateCommandTests
{
    [Fact]
    public void DefaultFormat_IsConsole()
    {
        // Console format should succeed without --output
        var result = GenerateCommand.Execute(
            "Hello", "console", null, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
        Assert.Equal(0, result);
    }

    [Fact]
    public void SvgFormat_ProducesOutput()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.svg");
        try
        {
            var result = GenerateCommand.Execute(
                "Hello", "svg", outputPath, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
            Assert.Equal(0, result);
            var content = File.ReadAllText(outputPath);
            Assert.Contains("<svg", content);
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void PngFormat_RequiresOutput()
    {
        var result = GenerateCommand.Execute(
            "Hello", "png", null, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
        Assert.Equal(1, result);
    }

    [Fact]
    public void JpegFormat_RequiresOutput()
    {
        var result = GenerateCommand.Execute(
            "Hello", "jpeg", null, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
        Assert.Equal(1, result);
    }

    [Fact]
    public void PdfFormat_RequiresOutput()
    {
        var result = GenerateCommand.Execute(
            "Hello", "pdf", null, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
        Assert.Equal(1, result);
    }

    [Fact]
    public void PngFormat_WritesFile()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.png");
        try
        {
            var result = GenerateCommand.Execute(
                "Hello", "png", outputPath, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
            Assert.Equal(0, result);
            var bytes = File.ReadAllBytes(outputPath);
            // PNG magic bytes
            Assert.Equal(0x89, bytes[0]);
            Assert.Equal(0x50, bytes[1]);
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void JpegFormat_WritesFile()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.jpg");
        try
        {
            var result = GenerateCommand.Execute(
                "Hello", "jpeg", outputPath, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
            Assert.Equal(0, result);
            var bytes = File.ReadAllBytes(outputPath);
            // JPEG magic bytes
            Assert.Equal(0xFF, bytes[0]);
            Assert.Equal(0xD8, bytes[1]);
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void PdfFormat_WritesFile()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
        try
        {
            var result = GenerateCommand.Execute(
                "Hello", "pdf", outputPath, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
            Assert.Equal(0, result);
            var content = File.ReadAllText(outputPath);
            Assert.StartsWith("%PDF", content);
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void InvalidFormat_ReturnsError()
    {
        var result = GenerateCommand.Execute(
            "Hello", "bmp", null, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
        Assert.Equal(1, result);
    }

    [Fact]
    public void InvertFlag_WorksForConsole()
    {
        // Should succeed with invert enabled
        var result = GenerateCommand.Execute(
            "Hello", "console", null, 10, "#000000", "#ffffff", "medium", true, 85, "block", null);
        Assert.Equal(0, result);
    }

    [Fact]
    public void AsciiFormat_ProducesOutput()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.txt");
        try
        {
            var result = GenerateCommand.Execute(
                "Hello", "ascii", outputPath, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
            Assert.Equal(0, result);
            var content = File.ReadAllText(outputPath);
            Assert.NotEmpty(content);
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void AsciiStyleOption_MapsCorrectly()
    {
        Assert.Equal(AsciiStyle.Block, GenerateCommand.MapAsciiStyle("block"));
        Assert.Equal(AsciiStyle.Hash, GenerateCommand.MapAsciiStyle("hash"));
        Assert.Equal(AsciiStyle.Dot, GenerateCommand.MapAsciiStyle("dot"));
        Assert.Equal(AsciiStyle.Shade, GenerateCommand.MapAsciiStyle("shade"));
        // Unknown falls back to Block
        Assert.Equal(AsciiStyle.Block, GenerateCommand.MapAsciiStyle("unknown"));
    }

    [Theory]
    [InlineData("low", ErrorCorrectionLevel.L)]
    [InlineData("medium", ErrorCorrectionLevel.M)]
    [InlineData("quartile", ErrorCorrectionLevel.Q)]
    [InlineData("high", ErrorCorrectionLevel.H)]
    public void ErrorCorrection_MapsCorrectly(string input, ErrorCorrectionLevel expected)
    {
        Assert.Equal(expected, GenerateCommand.MapCliEcc(input));
    }

    [Fact]
    public void SvgFormat_ToStdout_Succeeds()
    {
        // SVG without --output should succeed (stdout)
        var result = GenerateCommand.Execute(
            "Hello", "svg", null, 10, "#000000", "#ffffff", "medium", false, 85, "block", null);
        Assert.Equal(0, result);
    }

    [Fact]
    public void PdfFormat_WithTitle_WritesFile()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
        try
        {
            var result = GenerateCommand.Execute(
                "Hello", "pdf", outputPath, 10, "#000000", "#ffffff", "medium", false, 85, "block", "My QR Code");
            Assert.Equal(0, result);
            Assert.True(File.Exists(outputPath));
            Assert.True(new FileInfo(outputPath).Length > 0);
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }
}
