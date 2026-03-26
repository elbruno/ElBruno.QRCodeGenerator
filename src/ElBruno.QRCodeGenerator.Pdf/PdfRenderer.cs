using System.Globalization;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace ElBruno.QRCodeGenerator.Pdf;

/// <summary>
/// Renders a QR code boolean matrix as a PDF document.
/// </summary>
public static class PdfRenderer
{
    /// <summary>
    /// Renders a QR code matrix to PDF bytes.
    /// </summary>
    /// <param name="matrix">A 2D boolean array where true represents a dark module.</param>
    /// <param name="options">Optional PDF rendering options.</param>
    /// <returns>A byte array containing the PDF document.</returns>
    public static byte[] Render(bool[,] matrix, PdfOptions? options = null)
    {
        options ??= new PdfOptions();

        int matrixRows = matrix.GetLength(0);
        int matrixCols = matrix.GetLength(1);
        int quietZone = options.QuietZone;
        double moduleSize = options.ModuleSize;

        double qrWidth = (matrixCols + 2 * quietZone) * moduleSize;
        double qrHeight = (matrixRows + 2 * quietZone) * moduleSize;

        var fgColor = ParseHexColor(options.ForegroundColor);
        var bgColor = ParseHexColor(options.BackgroundColor);

        var document = new PdfDocument();
        var page = document.AddPage();
        page.Width = XUnit.FromPoint(options.PageWidth);
        page.Height = XUnit.FromPoint(options.PageHeight);

        var gfx = XGraphics.FromPdfPage(page);

        // Calculate title offset
        double titleOffset = 0;
        if (!string.IsNullOrEmpty(options.Title))
        {
            titleOffset = options.TitleFontSize + 10;
        }

        // Calculate QR code position
        double totalContentHeight = qrHeight + titleOffset;
        double qrX = options.X ?? (options.PageWidth - qrWidth) / 2.0;
        double qrY = options.Y ?? (options.PageHeight - totalContentHeight) / 2.0 + titleOffset;

        // Draw title if specified
        if (!string.IsNullOrEmpty(options.Title))
        {
            var font = new XFont("Arial", options.TitleFontSize);
            double titleY = qrY - titleOffset;
            gfx.DrawString(
                options.Title,
                font,
                new XSolidBrush(fgColor),
                new XRect(0, titleY, options.PageWidth, options.TitleFontSize + 4),
                XStringFormats.TopCenter);
        }

        // Draw background rectangle for the QR code area
        gfx.DrawRectangle(new XSolidBrush(bgColor), qrX, qrY, qrWidth, qrHeight);

        // Draw dark modules
        var fgBrush = new XSolidBrush(fgColor);
        for (int row = 0; row < matrixRows; row++)
        {
            for (int col = 0; col < matrixCols; col++)
            {
                if (matrix[row, col])
                {
                    double x = qrX + (col + quietZone) * moduleSize;
                    double y = qrY + (row + quietZone) * moduleSize;
                    gfx.DrawRectangle(fgBrush, x, y, moduleSize, moduleSize);
                }
            }
        }

        using var stream = new MemoryStream();
        document.Save(stream, false);
        return stream.ToArray();
    }

    private static XColor ParseHexColor(string hex)
    {
        hex = hex.TrimStart('#');
        if (hex.Length == 6)
        {
            int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return XColor.FromArgb(r, g, b);
        }

        return XColors.Black;
    }
}
