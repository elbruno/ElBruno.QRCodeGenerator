using System.Globalization;
using SkiaSharp;

namespace ElBruno.QRCodeGenerator.Image;

/// <summary>
/// Renders a QR code boolean matrix as a bitmap image using SkiaSharp.
/// </summary>
public static class ImageRenderer
{
    /// <summary>
    /// Renders a QR code matrix to image bytes in the specified format.
    /// </summary>
    /// <param name="matrix">A 2D boolean array where true represents a dark module.</param>
    /// <param name="options">Optional image rendering options.</param>
    /// <returns>A byte array containing the encoded image data.</returns>
    public static byte[] Render(bool[,] matrix, ImageOptions? options = null)
    {
        options ??= new ImageOptions();

        int matrixRows = matrix.GetLength(0);
        int matrixCols = matrix.GetLength(1);
        int quietZone = options.QuietZone;
        int moduleSize = options.ModuleSize;

        int totalWidth = (matrixCols + 2 * quietZone) * moduleSize;
        int totalHeight = (matrixRows + 2 * quietZone) * moduleSize;

        var fgColor = ParseColor(options.ForegroundColor);
        var bgColor = ParseColor(options.BackgroundColor);

        using var bitmap = new SKBitmap(totalWidth, totalHeight);
        using var canvas = new SKCanvas(bitmap);

        // Fill background
        using (var bgPaint = new SKPaint { Color = bgColor, Style = SKPaintStyle.Fill })
        {
            canvas.DrawRect(0, 0, totalWidth, totalHeight, bgPaint);
        }

        // Draw dark modules
        using (var fgPaint = new SKPaint { Color = fgColor, Style = SKPaintStyle.Fill })
        {
            for (int row = 0; row < matrixRows; row++)
            {
                for (int col = 0; col < matrixCols; col++)
                {
                    if (matrix[row, col])
                    {
                        int x = (col + quietZone) * moduleSize;
                        int y = (row + quietZone) * moduleSize;
                        canvas.DrawRect(x, y, moduleSize, moduleSize, fgPaint);
                    }
                }
            }
        }

        canvas.Flush();

        var skFormat = options.Format switch
        {
            ImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
            ImageFormat.Webp => SKEncodedImageFormat.Webp,
            _ => SKEncodedImageFormat.Png
        };

        int quality = options.Format == ImageFormat.Jpeg
            ? Math.Clamp(options.JpegQuality, 0, 100)
            : 100;

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(skFormat, quality);
        return data.ToArray();
    }

    private static SKColor ParseColor(string hex)
    {
        hex = hex.TrimStart('#');
        if (hex.Length == 6 &&
            byte.TryParse(hex.AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte r) &&
            byte.TryParse(hex.AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte g) &&
            byte.TryParse(hex.AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte b))
        {
            return new SKColor(r, g, b);
        }

        return SKColors.Black;
    }
}
