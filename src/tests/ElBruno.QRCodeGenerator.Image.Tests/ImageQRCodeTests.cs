using Xunit;
using ElBruno.QRCodeGenerator.Image;

namespace ElBruno.QRCodeGenerator.Image.Tests;

public class ImageQRCodeTests
{
    [Fact]
    public void Generate_WithText_ReturnsNonEmptyBytes()
    {
        var bytes = ImageQRCode.Generate("https://github.com/elbruno");

        Assert.NotNull(bytes);
        Assert.True(bytes.Length > 0);
    }

    [Fact]
    public void Generate_DefaultFormat_ReturnsPng()
    {
        var bytes = ImageQRCode.Generate("Hello, World!");

        // PNG magic bytes
        Assert.Equal(0x89, bytes[0]);
        Assert.Equal((byte)'P', bytes[1]);
        Assert.Equal((byte)'N', bytes[2]);
        Assert.Equal((byte)'G', bytes[3]);
    }

    [Fact]
    public void ToPng_ReturnsValidPng()
    {
        var bytes = ImageQRCode.ToPng("PNG test");

        Assert.Equal(0x89, bytes[0]);
        Assert.Equal((byte)'P', bytes[1]);
        Assert.Equal((byte)'N', bytes[2]);
        Assert.Equal((byte)'G', bytes[3]);
    }

    [Fact]
    public void ToJpeg_ReturnsValidJpeg()
    {
        var bytes = ImageQRCode.ToJpeg("JPEG test");

        Assert.Equal(0xFF, bytes[0]);
        Assert.Equal(0xD8, bytes[1]);
        Assert.Equal(0xFF, bytes[2]);
    }

    [Fact]
    public void ToJpeg_CustomQuality_ReturnsValidJpeg()
    {
        var bytes = ImageQRCode.ToJpeg("Quality test", quality: 50);

        Assert.Equal(0xFF, bytes[0]);
        Assert.Equal(0xD8, bytes[1]);
    }

    [Fact]
    public void Generate_NullText_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ImageQRCode.Generate(null!));
    }

    [Fact]
    public void Generate_EmptyText_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ImageQRCode.Generate(""));
    }

    [Fact]
    public void Generate_WhitespaceText_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ImageQRCode.Generate("   "));
    }

    [Fact]
    public void Generate_WithCustomErrorCorrection_Succeeds()
    {
        var qrOptions = new ImageQRCodeOptions
        {
            ErrorCorrection = ImageErrorCorrectionLevel.H
        };

        var bytes = ImageQRCode.Generate("High ECC test", qrOptions);

        Assert.NotNull(bytes);
        Assert.True(bytes.Length > 0);
    }

    [Fact]
    public void Generate_WithCustomImageOptions_Succeeds()
    {
        var imageOptions = new ImageOptions
        {
            ForegroundColor = "#003366",
            BackgroundColor = "#f0f0f0",
            ModuleSize = 5
        };

        var bytes = ImageQRCode.Generate("Custom options", imageOptions: imageOptions);

        Assert.NotNull(bytes);
        Assert.True(bytes.Length > 0);
    }

    [Fact]
    public void Generate_VeryLongText_ThrowsArgumentException()
    {
        var longText = new string('A', 5000);

        Assert.Throws<ArgumentException>(() => ImageQRCode.Generate(longText));
    }
}
