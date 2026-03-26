using Xunit;
using ElBruno.QRCodeGenerator.Image;

namespace ElBruno.QRCodeGenerator.Image.Tests;

public class ImageRendererTests
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
    public void Render_WithDefaultOptions_ReturnsPngBytes()
    {
        var matrix = CreateSampleMatrix();

        var bytes = ImageRenderer.Render(matrix);

        Assert.NotNull(bytes);
        Assert.True(bytes.Length > 0);
        // PNG magic bytes: 0x89 P N G
        Assert.Equal(0x89, bytes[0]);
        Assert.Equal((byte)'P', bytes[1]);
        Assert.Equal((byte)'N', bytes[2]);
        Assert.Equal((byte)'G', bytes[3]);
    }

    [Fact]
    public void Render_JpegFormat_ReturnsJpegBytes()
    {
        var matrix = CreateSampleMatrix();
        var options = new ImageOptions { Format = ImageFormat.Jpeg };

        var bytes = ImageRenderer.Render(matrix, options);

        Assert.NotNull(bytes);
        Assert.True(bytes.Length > 0);
        // JPEG magic bytes: 0xFF 0xD8 0xFF
        Assert.Equal(0xFF, bytes[0]);
        Assert.Equal(0xD8, bytes[1]);
        Assert.Equal(0xFF, bytes[2]);
    }

    [Fact]
    public void Render_WebpFormat_ReturnsWebpBytes()
    {
        var matrix = CreateSampleMatrix();
        var options = new ImageOptions { Format = ImageFormat.Webp };

        var bytes = ImageRenderer.Render(matrix, options);

        Assert.NotNull(bytes);
        Assert.True(bytes.Length > 0);
        // WebP starts with RIFF....WEBP
        Assert.Equal((byte)'R', bytes[0]);
        Assert.Equal((byte)'I', bytes[1]);
        Assert.Equal((byte)'F', bytes[2]);
        Assert.Equal((byte)'F', bytes[3]);
    }

    [Fact]
    public void Render_CorrectDimensions_DefaultOptions()
    {
        // 3x3 matrix, moduleSize=10, quietZone=1 → (3+2)*10 = 50x50
        var matrix = CreateSampleMatrix();

        var bytes = ImageRenderer.Render(matrix);

        // Decode and verify dimensions
        using var skBitmap = SkiaSharp.SKBitmap.Decode(bytes);
        Assert.Equal(50, skBitmap.Width);
        Assert.Equal(50, skBitmap.Height);
    }

    [Fact]
    public void Render_CustomModuleSize_AffectsDimensions()
    {
        // 3x3 matrix, moduleSize=20, quietZone=1 → (3+2)*20 = 100x100
        var matrix = CreateSampleMatrix();
        var options = new ImageOptions { ModuleSize = 20, QuietZone = 1 };

        var bytes = ImageRenderer.Render(matrix, options);

        using var skBitmap = SkiaSharp.SKBitmap.Decode(bytes);
        Assert.Equal(100, skBitmap.Width);
        Assert.Equal(100, skBitmap.Height);
    }

    [Fact]
    public void Render_CustomQuietZone_AffectsDimensions()
    {
        // 3x3 matrix, moduleSize=10, quietZone=4 → (3+8)*10 = 110x110
        var matrix = CreateSampleMatrix();
        var options = new ImageOptions { ModuleSize = 10, QuietZone = 4 };

        var bytes = ImageRenderer.Render(matrix, options);

        using var skBitmap = SkiaSharp.SKBitmap.Decode(bytes);
        Assert.Equal(110, skBitmap.Width);
        Assert.Equal(110, skBitmap.Height);
    }

    [Fact]
    public void Render_CustomColors_AppliedCorrectly()
    {
        // All-dark 2x2 matrix with red foreground on white background
        var matrix = new bool[,] { { true, true }, { true, true } };
        var options = new ImageOptions
        {
            ForegroundColor = "#ff0000",
            BackgroundColor = "#ffffff",
            ModuleSize = 1,
            QuietZone = 0
        };

        var bytes = ImageRenderer.Render(matrix, options);

        using var skBitmap = SkiaSharp.SKBitmap.Decode(bytes);
        var pixel = skBitmap.GetPixel(0, 0);
        Assert.Equal(255, pixel.Red);
        Assert.Equal(0, pixel.Green);
        Assert.Equal(0, pixel.Blue);
    }

    [Fact]
    public void Render_BackgroundColor_InQuietZone()
    {
        // 1x1 dark module with quietZone=1, blue background
        var matrix = new bool[,] { { true } };
        var options = new ImageOptions
        {
            ForegroundColor = "#000000",
            BackgroundColor = "#0000ff",
            ModuleSize = 10,
            QuietZone = 1
        };

        var bytes = ImageRenderer.Render(matrix, options);

        using var skBitmap = SkiaSharp.SKBitmap.Decode(bytes);
        // Top-left corner (0,0) is in the quiet zone → should be background
        var pixel = skBitmap.GetPixel(0, 0);
        Assert.Equal(0, pixel.Red);
        Assert.Equal(0, pixel.Green);
        Assert.Equal(255, pixel.Blue);
    }

    [Fact]
    public void Render_EmptyMatrix_ReturnsOnlyBackground()
    {
        var matrix = new bool[3, 3]; // all false
        var options = new ImageOptions
        {
            ForegroundColor = "#000000",
            BackgroundColor = "#ffffff",
            ModuleSize = 1,
            QuietZone = 0
        };

        var bytes = ImageRenderer.Render(matrix, options);

        using var skBitmap = SkiaSharp.SKBitmap.Decode(bytes);
        // All pixels should be white
        for (int y = 0; y < skBitmap.Height; y++)
        {
            for (int x = 0; x < skBitmap.Width; x++)
            {
                var p = skBitmap.GetPixel(x, y);
                Assert.Equal(255, p.Red);
                Assert.Equal(255, p.Green);
                Assert.Equal(255, p.Blue);
            }
        }
    }
}
