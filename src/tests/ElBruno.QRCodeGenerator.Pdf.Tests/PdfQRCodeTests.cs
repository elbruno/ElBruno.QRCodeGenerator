using Xunit;
using ElBruno.QRCodeGenerator.Pdf;

namespace ElBruno.QRCodeGenerator.Pdf.Tests;

public class PdfQRCodeTests
{
    [Fact]
    public void Generate_WithText_ReturnsValidPdfBytes()
    {
        var pdfBytes = PdfQRCode.Generate("https://github.com/elbruno");

        Assert.True(pdfBytes.Length > 4);
        Assert.Equal((byte)'%', pdfBytes[0]);
        Assert.Equal((byte)'P', pdfBytes[1]);
        Assert.Equal((byte)'D', pdfBytes[2]);
        Assert.Equal((byte)'F', pdfBytes[3]);
    }

    [Fact]
    public void Generate_NullText_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PdfQRCode.Generate(null!));
    }

    [Fact]
    public void Generate_EmptyText_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => PdfQRCode.Generate(""));
    }

    [Fact]
    public void Generate_WithCustomOptions_ProducesOutput()
    {
        var qrOptions = new PdfQRCodeOptions
        {
            ErrorCorrection = PdfErrorCorrectionLevel.High
        };
        var pdfOptions = new PdfOptions
        {
            ForegroundColor = "#003366",
            BackgroundColor = "#f0f0f0",
            ModuleSize = 3.0
        };

        var pdfBytes = PdfQRCode.Generate("Hello, World!", qrOptions, pdfOptions);

        Assert.True(pdfBytes.Length > 100);
        Assert.Equal((byte)'%', pdfBytes[0]);
    }

    [Fact]
    public void Generate_WithTitle_ProducesValidPdf()
    {
        var pdfOptions = new PdfOptions
        {
            Title = "My QR Code",
            TitleFontSize = 18
        };

        var pdfBytes = PdfQRCode.Generate("https://github.com", pdfOptions: pdfOptions);

        Assert.True(pdfBytes.Length > 100);
        Assert.Equal((byte)'%', pdfBytes[0]);
    }

    [Fact]
    public void Save_CreatesFile()
    {
        var filePath = Path.Combine(Path.GetTempPath(), $"test_qr_{Guid.NewGuid():N}.pdf");

        try
        {
            PdfQRCode.Save("https://github.com/elbruno", filePath);

            Assert.True(File.Exists(filePath));
            var bytes = File.ReadAllBytes(filePath);
            Assert.True(bytes.Length > 4);
            Assert.Equal((byte)'%', bytes[0]);
        }
        finally
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }

    [Fact]
    public void Save_NullFilePath_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PdfQRCode.Save("test", null!));
    }

    [Fact]
    public void Generate_VeryLongText_ProducesValidPdf()
    {
        // Long URL that should still fit in a QR code with low error correction
        var longText = "https://example.com/" + new string('a', 200);

        var pdfBytes = PdfQRCode.Generate(longText, new PdfQRCodeOptions
        {
            ErrorCorrection = PdfErrorCorrectionLevel.Low
        });

        Assert.True(pdfBytes.Length > 100);
        Assert.Equal((byte)'%', pdfBytes[0]);
    }

    [Fact]
    public void Generate_MinimumModuleSize_ProducesValidPdf()
    {
        var pdfOptions = new PdfOptions { ModuleSize = 0.5 };

        var pdfBytes = PdfQRCode.Generate("test", pdfOptions: pdfOptions);

        Assert.True(pdfBytes.Length > 100);
        Assert.Equal((byte)'%', pdfBytes[0]);
    }
}
