namespace ElBruno.QRCodeGenerator.Pdf;

/// <summary>
/// Configuration options for PDF rendering of QR codes.
/// </summary>
public class PdfOptions
{
    /// <summary>
    /// Gets or sets the size of each QR module in PDF points.
    /// Default is 2.0.
    /// </summary>
    public double ModuleSize { get; set; } = 2.0;

    /// <summary>
    /// Gets or sets the foreground (dark module) color as a hex string.
    /// Default is "#000000" (black).
    /// </summary>
    public string ForegroundColor { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the background (light module) color as a hex string.
    /// Default is "#ffffff" (white).
    /// </summary>
    public string BackgroundColor { get; set; } = "#ffffff";

    /// <summary>
    /// Gets or sets the quiet zone (border) size in modules around the QR code.
    /// Default is 1.
    /// </summary>
    public int QuietZone { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page width in PDF points. Default is 612 (US Letter).
    /// </summary>
    public double PageWidth { get; set; } = 612;

    /// <summary>
    /// Gets or sets the page height in PDF points. Default is 792 (US Letter).
    /// </summary>
    public double PageHeight { get; set; } = 792;

    /// <summary>
    /// Gets or sets the X position of the QR code in PDF points.
    /// Null means centered horizontally on the page.
    /// </summary>
    public double? X { get; set; }

    /// <summary>
    /// Gets or sets the Y position of the QR code in PDF points.
    /// Null means centered vertically on the page.
    /// </summary>
    public double? Y { get; set; }

    /// <summary>
    /// Gets or sets an optional title to display above the QR code.
    /// Null means no title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the font size for the title text in PDF points.
    /// Default is 14.
    /// </summary>
    public double TitleFontSize { get; set; } = 14;
}
