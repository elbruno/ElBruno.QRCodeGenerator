using Xunit;
using ElBruno.QRCodeGenerator.Ascii;

namespace ElBruno.QRCodeGenerator.Ascii.Tests;

public class AsciiOptionsTests
{
    [Fact]
    public void AsciiOptions_DefaultValues_AreCorrect()
    {
        var options = new AsciiOptions();

        Assert.Equal(AsciiStyle.Block, options.Style);
        Assert.Equal("██", options.DarkCharacter);
        Assert.Equal("  ", options.LightCharacter);
        Assert.Equal(1, options.QuietZone);
        Assert.False(options.Border);
    }

    [Fact]
    public void AsciiQRCodeOptions_DefaultValues_AreCorrect()
    {
        var options = new AsciiQRCodeOptions();

        Assert.Equal(AsciiErrorCorrectionLevel.Medium, options.ErrorCorrection);
    }

    [Fact]
    public void AsciiOptions_CustomValues_AreSet()
    {
        var options = new AsciiOptions
        {
            Style = AsciiStyle.Custom,
            DarkCharacter = "XX",
            LightCharacter = "..",
            QuietZone = 3,
            Border = true
        };

        Assert.Equal(AsciiStyle.Custom, options.Style);
        Assert.Equal("XX", options.DarkCharacter);
        Assert.Equal("..", options.LightCharacter);
        Assert.Equal(3, options.QuietZone);
        Assert.True(options.Border);
    }

    [Fact]
    public void AsciiErrorCorrectionLevel_AllValues_Exist()
    {
        Assert.Equal(0, (int)AsciiErrorCorrectionLevel.Low);
        Assert.Equal(1, (int)AsciiErrorCorrectionLevel.Medium);
        Assert.Equal(2, (int)AsciiErrorCorrectionLevel.Quartile);
        Assert.Equal(3, (int)AsciiErrorCorrectionLevel.High);
    }

    [Fact]
    public void AsciiStyle_AllValues_Exist()
    {
        var styles = Enum.GetValues<AsciiStyle>();

        Assert.Equal(5, styles.Length);
        Assert.Contains(AsciiStyle.Block, styles);
        Assert.Contains(AsciiStyle.Hash, styles);
        Assert.Contains(AsciiStyle.Dot, styles);
        Assert.Contains(AsciiStyle.Shade, styles);
        Assert.Contains(AsciiStyle.Custom, styles);
    }
}
