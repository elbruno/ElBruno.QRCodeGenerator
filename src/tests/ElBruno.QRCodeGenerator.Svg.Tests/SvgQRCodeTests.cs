using System.Xml.Linq;
using Xunit;
using ElBruno.QRCodeGenerator.Svg;

namespace ElBruno.QRCodeGenerator.Svg.Tests;

public class SvgQRCodeTests
{
    [Fact]
    public void Generate_WithText_ReturnsValidSvg()
    {
        var svg = SvgQRCode.Generate("https://github.com/elbruno");

        Assert.Contains("<svg", svg);
        Assert.Contains("</svg>", svg);
        Assert.Contains("xmlns", svg);
    }

    [Fact]
    public void Generate_NullText_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => SvgQRCode.Generate(null!));
    }

    [Fact]
    public void Generate_EmptyText_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => SvgQRCode.Generate(""));
    }

    [Fact]
    public void Generate_WithCustomOptions_ProducesOutput()
    {
        var qrOptions = new SvgQRCodeOptions
        {
            ErrorCorrection = SvgErrorCorrectionLevel.H,
            QuietZoneSize = 2
        };
        var svgOptions = new SvgOptions
        {
            ForegroundColor = "#003366",
            BackgroundColor = "#f0f0f0",
            ModuleSize = 5
        };

        var svg = SvgQRCode.Generate("Hello, World!", qrOptions, svgOptions);

        Assert.Contains("<svg", svg);
        Assert.Contains("fill=\"#003366\"", svg);
        Assert.Contains("fill=\"#f0f0f0\"", svg);
    }

    [Fact]
    public void GenerateInline_OmitsXmlDeclaration()
    {
        var svg = SvgQRCode.GenerateInline("test");

        Assert.DoesNotContain("<?xml", svg);
        Assert.StartsWith("<svg", svg);
    }

    [Fact]
    public void Generate_SvgIsWellFormedXml()
    {
        var svg = SvgQRCode.Generate("Well-formed XML test");

        // Should parse without throwing
        var doc = XDocument.Parse(svg);
        Assert.NotNull(doc);
        Assert.NotNull(doc.Root);
        Assert.Equal("svg", doc.Root.Name.LocalName);
    }
}
