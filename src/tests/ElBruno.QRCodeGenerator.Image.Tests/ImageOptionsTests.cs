using Xunit;
using ElBruno.QRCodeGenerator.Image;

namespace ElBruno.QRCodeGenerator.Image.Tests;

public class ImageOptionsTests
{
    [Fact]
    public void ImageOptions_DefaultValues_AreCorrect()
    {
        var options = new ImageOptions();

        Assert.Equal(ImageFormat.Png, options.Format);
        Assert.Equal(10, options.ModuleSize);
        Assert.Equal(1, options.QuietZone);
        Assert.Equal("#000000", options.ForegroundColor);
        Assert.Equal("#ffffff", options.BackgroundColor);
        Assert.Equal(85, options.JpegQuality);
    }

    [Fact]
    public void ImageQRCodeOptions_DefaultValues_AreCorrect()
    {
        var options = new ImageQRCodeOptions();

        Assert.Equal(ImageErrorCorrectionLevel.M, options.ErrorCorrection);
        Assert.Equal(1, options.QuietZoneSize);
    }

    [Fact]
    public void ImageOptions_CustomModuleSize_AffectsOutputSize()
    {
        var smallBytes = ImageQRCode.ToPng("size test");
        var largeBytes = ImageQRCode.Generate("size test", imageOptions: new ImageOptions
        {
            ModuleSize = 20
        });

        // Larger module size → more pixels → larger byte output
        Assert.True(largeBytes.Length > smallBytes.Length,
            $"Expected larger output for ModuleSize=20 ({largeBytes.Length}) vs default ({smallBytes.Length})");
    }

    [Fact]
    public void ImageOptions_MinimumModuleSize_Renders()
    {
        var options = new ImageOptions { ModuleSize = 1, QuietZone = 0 };

        var bytes = ImageQRCode.Generate("tiny", imageOptions: options);

        Assert.NotNull(bytes);
        Assert.True(bytes.Length > 0);
    }
}
