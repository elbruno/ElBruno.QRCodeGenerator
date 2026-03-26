namespace ElBruno.QRCodeGenerator.Svg;

/// <summary>
/// Configuration options for SVG rendering of QR codes.
/// </summary>
public class SvgOptions
{
    /// <summary>
    /// Gets or sets the foreground (dark module) color as a CSS color string.
    /// Default is "#000000" (black).
    /// </summary>
    public string ForegroundColor { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the background (light module) color as a CSS color string.
    /// Default is "#ffffff" (white).
    /// </summary>
    public string BackgroundColor { get; set; } = "#ffffff";

    /// <summary>
    /// Gets or sets the size of each module (cell) in SVG units.
    /// Default is 10.
    /// </summary>
    public int ModuleSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the quiet zone (border) size in modules around the QR code.
    /// Default is 1.
    /// </summary>
    public int QuietZone { get; set; } = 1;

    /// <summary>
    /// Gets or sets whether to include the XML declaration at the start of the SVG output.
    /// Default is true.
    /// </summary>
    public bool IncludeXmlDeclaration { get; set; } = true;
}
