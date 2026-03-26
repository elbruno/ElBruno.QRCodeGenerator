namespace ElBruno.QRCodeGenerator.Image;

/// <summary>
/// Configuration options for bitmap rendering of QR codes.
/// </summary>
public class ImageOptions
{
    /// <summary>
    /// Gets or sets the output image format.
    /// Default is <see cref="ImageFormat.Png"/>.
    /// </summary>
    public ImageFormat Format { get; set; } = ImageFormat.Png;

    /// <summary>
    /// Gets or sets the size of each module (cell) in pixels.
    /// Default is 10.
    /// </summary>
    public int ModuleSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the quiet zone (border) size in modules around the QR code.
    /// Default is 1.
    /// </summary>
    public int QuietZone { get; set; } = 1;

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
    /// Gets or sets the JPEG quality (0–100). Only applies when <see cref="Format"/> is <see cref="ImageFormat.Jpeg"/>.
    /// Default is 85.
    /// </summary>
    public int JpegQuality { get; set; } = 85;
}
