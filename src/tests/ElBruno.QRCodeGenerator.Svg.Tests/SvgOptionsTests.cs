using Xunit;
using ElBruno.QRCodeGenerator.Svg;

namespace ElBruno.QRCodeGenerator.Svg.Tests;

public class SvgOptionsTests
{
    [Fact]
    public void SvgOptions_DefaultValues_AreCorrect()
    {
        var options = new SvgOptions();

        Assert.Equal("#000000", options.ForegroundColor);
        Assert.Equal("#ffffff", options.BackgroundColor);
        Assert.Equal(10, options.ModuleSize);
        Assert.Equal(1, options.QuietZone);
        Assert.True(options.IncludeXmlDeclaration);
    }

    [Fact]
    public void SvgQRCodeOptions_DefaultValues_AreCorrect()
    {
        var options = new SvgQRCodeOptions();

        Assert.Equal(SvgErrorCorrectionLevel.M, options.ErrorCorrection);
        Assert.Equal(1, options.QuietZoneSize);
    }
}
