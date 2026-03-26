using Xunit;
using ElBruno.QRCodeGenerator.Svg;

namespace ElBruno.QRCodeGenerator.Svg.Tests;

public class SvgRendererTests
{
    private static bool[,] CreateSampleMatrix()
    {
        // A small 3x3 matrix with known dark modules
        return new bool[,]
        {
            { true, false, true },
            { false, true, false },
            { true, false, true }
        };
    }

    [Fact]
    public void GenerateSvg_WithDefaultOptions_ReturnsValidSvg()
    {
        var matrix = CreateSampleMatrix();

        var svg = SvgRenderer.GenerateSvg(matrix);

        Assert.Contains("<svg", svg);
        Assert.Contains("</svg>", svg);
        Assert.Contains("xmlns=\"http://www.w3.org/2000/svg\"", svg);
    }

    [Fact]
    public void GenerateSvg_WithDefaultOptions_IncludesXmlDeclaration()
    {
        var matrix = CreateSampleMatrix();

        var svg = SvgRenderer.GenerateSvg(matrix);

        Assert.StartsWith("<?xml", svg);
    }

    [Fact]
    public void GenerateSvgInline_OmitsXmlDeclaration()
    {
        var matrix = CreateSampleMatrix();

        var svg = SvgRenderer.GenerateSvgInline(matrix);

        Assert.DoesNotContain("<?xml", svg);
        Assert.StartsWith("<svg", svg);
    }

    [Fact]
    public void GenerateSvg_ContainsBackgroundRect()
    {
        var matrix = CreateSampleMatrix();
        var options = new SvgOptions { BackgroundColor = "#ff0000" };

        var svg = SvgRenderer.GenerateSvg(matrix, options);

        Assert.Contains("fill=\"#ff0000\"", svg);
    }

    [Fact]
    public void GenerateSvg_DarkModulesCreateRects()
    {
        var matrix = CreateSampleMatrix();
        var options = new SvgOptions { ForegroundColor = "#000000" };

        var svg = SvgRenderer.GenerateSvg(matrix, options);

        // The sample matrix has 5 dark modules (true values)
        // Background uses #ffffff by default, foreground uses #000000
        int foregroundRectCount = CountOccurrences(svg, "fill=\"#000000\"");
        Assert.Equal(5, foregroundRectCount);
    }

    [Fact]
    public void GenerateSvg_CustomColors_AppliedCorrectly()
    {
        var matrix = CreateSampleMatrix();
        var options = new SvgOptions
        {
            ForegroundColor = "#123456",
            BackgroundColor = "#abcdef"
        };

        var svg = SvgRenderer.GenerateSvg(matrix, options);

        Assert.Contains("fill=\"#123456\"", svg);
        Assert.Contains("fill=\"#abcdef\"", svg);
    }

    [Fact]
    public void GenerateSvg_CustomModuleSize_AffectsDimensions()
    {
        var matrix = CreateSampleMatrix(); // 3x3
        var options = new SvgOptions
        {
            ModuleSize = 20,
            QuietZone = 1
        };

        // totalSize = (3 + 2*1) * 20 = 100
        var svg = SvgRenderer.GenerateSvg(matrix, options);

        Assert.Contains("viewBox=\"0 0 100 100\"", svg);
        Assert.Contains("width=\"100\"", svg);
        Assert.Contains("height=\"100\"", svg);
    }

    [Fact]
    public void GenerateSvg_EmptyMatrix_ReturnsOnlyBackground()
    {
        // All false = no dark modules
        var matrix = new bool[3, 3];
        var options = new SvgOptions
        {
            ForegroundColor = "#000000",
            BackgroundColor = "#ffffff"
        };

        var svg = SvgRenderer.GenerateSvg(matrix, options);

        // Only 1 rect total (the background)
        int totalRects = CountOccurrences(svg, "<rect");
        Assert.Equal(1, totalRects);
    }

    private static int CountOccurrences(string source, string substring)
    {
        int count = 0;
        int index = 0;
        while ((index = source.IndexOf(substring, index, StringComparison.Ordinal)) != -1)
        {
            count++;
            index += substring.Length;
        }
        return count;
    }
}
