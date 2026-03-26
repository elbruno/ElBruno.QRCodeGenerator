using Xunit;
using ElBruno.QRCodeGenerator.Pdf;

namespace ElBruno.QRCodeGenerator.Pdf.Tests;

public class PdfOptionsTests
{
    [Fact]
    public void PdfOptions_DefaultValues_AreCorrect()
    {
        var options = new PdfOptions();

        Assert.Equal(2.0, options.ModuleSize);
        Assert.Equal("#000000", options.ForegroundColor);
        Assert.Equal("#ffffff", options.BackgroundColor);
        Assert.Equal(1, options.QuietZone);
        Assert.Equal(612, options.PageWidth);
        Assert.Equal(792, options.PageHeight);
        Assert.Null(options.X);
        Assert.Null(options.Y);
        Assert.Null(options.Title);
        Assert.Equal(14, options.TitleFontSize);
    }

    [Fact]
    public void PdfOptions_CustomPageSize_CanBeSet()
    {
        var options = new PdfOptions
        {
            PageWidth = 595,   // A4 width
            PageHeight = 842   // A4 height
        };

        Assert.Equal(595, options.PageWidth);
        Assert.Equal(842, options.PageHeight);
    }

    [Fact]
    public void PdfOptions_CustomPosition_CanBeSet()
    {
        var options = new PdfOptions
        {
            X = 100.0,
            Y = 200.0
        };

        Assert.Equal(100.0, options.X);
        Assert.Equal(200.0, options.Y);
    }

    [Fact]
    public void PdfQRCodeOptions_DefaultValues_AreCorrect()
    {
        var options = new PdfQRCodeOptions();

        Assert.Equal(PdfErrorCorrectionLevel.Medium, options.ErrorCorrection);
    }
}
