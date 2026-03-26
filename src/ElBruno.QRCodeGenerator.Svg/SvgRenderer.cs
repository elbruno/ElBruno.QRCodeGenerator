using System.Globalization;
using System.Text;

namespace ElBruno.QRCodeGenerator.Svg;

/// <summary>
/// Renders a QR code boolean matrix as SVG markup.
/// </summary>
public static class SvgRenderer
{
    /// <summary>
    /// Generates a complete SVG document from a QR code matrix.
    /// </summary>
    /// <param name="qrCodeMatrix">A 2D boolean array where true represents a dark module.</param>
    /// <param name="options">Optional SVG rendering options.</param>
    /// <returns>A string containing the SVG markup.</returns>
    public static string GenerateSvg(bool[,] qrCodeMatrix, SvgOptions? options = null)
    {
        options ??= new SvgOptions();
        return RenderSvg(qrCodeMatrix, options);
    }

    /// <summary>
    /// Generates SVG markup without the XML declaration, suitable for inline embedding in HTML.
    /// </summary>
    /// <param name="qrCodeMatrix">A 2D boolean array where true represents a dark module.</param>
    /// <param name="options">Optional SVG rendering options.</param>
    /// <returns>A string containing the SVG markup without an XML declaration.</returns>
    public static string GenerateSvgInline(bool[,] qrCodeMatrix, SvgOptions? options = null)
    {
        options ??= new SvgOptions();
        var inlineOptions = new SvgOptions
        {
            ForegroundColor = options.ForegroundColor,
            BackgroundColor = options.BackgroundColor,
            ModuleSize = options.ModuleSize,
            QuietZone = options.QuietZone,
            IncludeXmlDeclaration = false
        };
        return RenderSvg(qrCodeMatrix, inlineOptions);
    }

    private static string RenderSvg(bool[,] qrCodeMatrix, SvgOptions options)
    {
        int matrixRows = qrCodeMatrix.GetLength(0);
        int matrixCols = qrCodeMatrix.GetLength(1);
        int quietZone = options.QuietZone;
        int moduleSize = options.ModuleSize;

        int totalWidth = (matrixCols + 2 * quietZone) * moduleSize;
        int totalHeight = (matrixRows + 2 * quietZone) * moduleSize;

        var sb = new StringBuilder();

        if (options.IncludeXmlDeclaration)
        {
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        }

        sb.AppendFormat(
            CultureInfo.InvariantCulture,
            "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {0} {1}\" width=\"{0}\" height=\"{1}\">",
            totalWidth,
            totalHeight);
        sb.AppendLine();

        // Background rect
        sb.AppendFormat(
            CultureInfo.InvariantCulture,
            "  <rect width=\"{0}\" height=\"{1}\" fill=\"{2}\" />",
            totalWidth,
            totalHeight,
            options.BackgroundColor);
        sb.AppendLine();

        // Dark module rects
        for (int row = 0; row < matrixRows; row++)
        {
            for (int col = 0; col < matrixCols; col++)
            {
                if (qrCodeMatrix[row, col])
                {
                    int x = (col + quietZone) * moduleSize;
                    int y = (row + quietZone) * moduleSize;

                    sb.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "  <rect x=\"{0}\" y=\"{1}\" width=\"{2}\" height=\"{2}\" fill=\"{3}\" />",
                        x,
                        y,
                        moduleSize,
                        options.ForegroundColor);
                    sb.AppendLine();
                }
            }
        }

        sb.Append("</svg>");

        return sb.ToString();
    }
}
