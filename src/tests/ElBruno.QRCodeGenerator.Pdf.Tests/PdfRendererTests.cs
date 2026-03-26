using Xunit;
using ElBruno.QRCodeGenerator.Pdf;

namespace ElBruno.QRCodeGenerator.Pdf.Tests;

public class PdfRendererTests
{
    private static bool[,] CreateSampleMatrix()
    {
        return new bool[,]
        {
            { true, false, true },
            { false, true, false },
            { true, false, true }
        };
    }

    [Fact]
    public void Render_WithDefaultOptions_ReturnsPdfMagicBytes()
    {
        var matrix = CreateSampleMatrix();

        var pdfBytes = PdfRenderer.Render(matrix);

        Assert.True(pdfBytes.Length > 4);
        // PDF files start with %PDF
        Assert.Equal((byte)'%', pdfBytes[0]);
        Assert.Equal((byte)'P', pdfBytes[1]);
        Assert.Equal((byte)'D', pdfBytes[2]);
        Assert.Equal((byte)'F', pdfBytes[3]);
    }

    [Fact]
    public void Render_WithDefaultOptions_ReturnsNonEmptyOutput()
    {
        var matrix = CreateSampleMatrix();

        var pdfBytes = PdfRenderer.Render(matrix);

        Assert.True(pdfBytes.Length > 100, "PDF output should be substantial");
    }

    [Fact]
    public void Render_CustomModuleSize_AffectsOutput()
    {
        var matrix = CreateSampleMatrix();
        var smallOptions = new PdfOptions { ModuleSize = 1.0 };
        var largeOptions = new PdfOptions { ModuleSize = 5.0 };

        var smallPdf = PdfRenderer.Render(matrix, smallOptions);
        var largePdf = PdfRenderer.Render(matrix, largeOptions);

        // Different module sizes should produce different PDF content
        Assert.NotEqual(smallPdf.Length, largePdf.Length);
    }

    [Fact]
    public void Render_CustomPageSize_ProducesValidPdf()
    {
        var matrix = CreateSampleMatrix();
        var options = new PdfOptions
        {
            PageWidth = 595,   // A4
            PageHeight = 842
        };

        var pdfBytes = PdfRenderer.Render(matrix, options);

        Assert.True(pdfBytes.Length > 4);
        Assert.Equal((byte)'%', pdfBytes[0]);
    }

    [Fact]
    public void Render_WithTitle_ProducesLargerOutput()
    {
        var matrix = CreateSampleMatrix();
        var withoutTitle = new PdfOptions();
        var withTitle = new PdfOptions { Title = "Test QR Code" };

        var pdfNoTitle = PdfRenderer.Render(matrix, withoutTitle);
        var pdfWithTitle = PdfRenderer.Render(matrix, withTitle);

        // PDF with title should contain more data
        Assert.True(pdfWithTitle.Length > pdfNoTitle.Length,
            "PDF with title should be larger than PDF without title");
    }

    [Fact]
    public void Render_EmptyMatrix_ReturnsValidPdf()
    {
        var matrix = new bool[3, 3]; // all false

        var pdfBytes = PdfRenderer.Render(matrix);

        Assert.True(pdfBytes.Length > 4);
        Assert.Equal((byte)'%', pdfBytes[0]);
    }

    [Fact]
    public void Render_CustomPosition_ProducesValidPdf()
    {
        var matrix = CreateSampleMatrix();
        var options = new PdfOptions
        {
            X = 50.0,
            Y = 100.0
        };

        var pdfBytes = PdfRenderer.Render(matrix, options);

        Assert.True(pdfBytes.Length > 4);
        Assert.Equal((byte)'%', pdfBytes[0]);
    }
}
