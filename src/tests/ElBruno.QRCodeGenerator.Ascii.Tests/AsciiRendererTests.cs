using Xunit;
using ElBruno.QRCodeGenerator.Ascii;

namespace ElBruno.QRCodeGenerator.Ascii.Tests;

public class AsciiRendererTests
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
    public void Render_WithDefaultOptions_ReturnsNonEmptyString()
    {
        var matrix = CreateSampleMatrix();

        var result = AsciiRenderer.Render(matrix);

        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void Render_BlockStyle_ContainsBlockCharacters()
    {
        var matrix = CreateSampleMatrix();
        var options = new AsciiOptions { Style = AsciiStyle.Block };

        var result = AsciiRenderer.Render(matrix, options);

        Assert.Contains("██", result);
    }

    [Fact]
    public void Render_HashStyle_ContainsHashCharacters()
    {
        var matrix = CreateSampleMatrix();
        var options = new AsciiOptions { Style = AsciiStyle.Hash };

        var result = AsciiRenderer.Render(matrix, options);

        Assert.Contains("##", result);
    }

    [Fact]
    public void Render_DotStyle_ContainsDotCharacters()
    {
        var matrix = CreateSampleMatrix();
        var options = new AsciiOptions { Style = AsciiStyle.Dot };

        var result = AsciiRenderer.Render(matrix, options);

        Assert.Contains("●●", result);
        Assert.Contains("··", result);
    }

    [Fact]
    public void Render_ShadeStyle_ContainsShadeCharacters()
    {
        var matrix = CreateSampleMatrix();
        var options = new AsciiOptions { Style = AsciiStyle.Shade };

        var result = AsciiRenderer.Render(matrix, options);

        Assert.Contains("██", result);
        Assert.Contains("░░", result);
    }

    [Fact]
    public void Render_CustomStyle_UsesCustomCharacters()
    {
        var matrix = CreateSampleMatrix();
        var options = new AsciiOptions
        {
            Style = AsciiStyle.Custom,
            DarkCharacter = "XX",
            LightCharacter = ".."
        };

        var result = AsciiRenderer.Render(matrix, options);

        Assert.Contains("XX", result);
        Assert.Contains("..", result);
    }

    [Fact]
    public void Render_QuietZone_AddsPadding()
    {
        // Single dark module
        var matrix = new bool[,] { { true } };
        var options = new AsciiOptions { Style = AsciiStyle.Block, QuietZone = 2 };

        var result = AsciiRenderer.Render(matrix, options);
        var lines = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        // 1 matrix row + 2*2 quiet zone rows = 5 rows
        Assert.Equal(5, lines.Length);
    }

    [Fact]
    public void Render_ZeroQuietZone_NoExtraPadding()
    {
        var matrix = CreateSampleMatrix(); // 3x3
        var options = new AsciiOptions { Style = AsciiStyle.Block, QuietZone = 0 };

        var result = AsciiRenderer.Render(matrix, options);
        var lines = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(3, lines.Length);
    }

    [Fact]
    public void Render_WithBorder_ContainsBorderCharacters()
    {
        var matrix = CreateSampleMatrix();
        var options = new AsciiOptions { Style = AsciiStyle.Block, Border = true };

        var result = AsciiRenderer.Render(matrix, options);

        Assert.Contains("+", result);
        Assert.Contains("-", result);
        Assert.Contains("|", result);
    }

    [Fact]
    public void Render_WithoutBorder_NoBorderCharacters()
    {
        var matrix = CreateSampleMatrix();
        var options = new AsciiOptions { Style = AsciiStyle.Block, Border = false };

        var result = AsciiRenderer.Render(matrix, options);

        Assert.DoesNotContain("+", result);
        Assert.DoesNotContain("|", result);
    }

    [Fact]
    public void Render_AllDarkMatrix_ContainsOnlyDarkCharacters()
    {
        var matrix = new bool[,]
        {
            { true, true },
            { true, true }
        };
        var options = new AsciiOptions { Style = AsciiStyle.Hash, QuietZone = 0 };

        var result = AsciiRenderer.Render(matrix, options);
        var lines = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            Assert.Equal("####", line);
        }
    }

    [Fact]
    public void Render_AllLightMatrix_ContainsOnlyLightCharacters()
    {
        var matrix = new bool[3, 3]; // all false
        var options = new AsciiOptions { Style = AsciiStyle.Dot, QuietZone = 0 };

        var result = AsciiRenderer.Render(matrix, options);
        var lines = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            Assert.Equal("······", line);
        }
    }
}
