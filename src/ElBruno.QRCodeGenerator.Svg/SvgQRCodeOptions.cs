namespace ElBruno.QRCodeGenerator.Svg;

/// <summary>
/// Configuration options for QR code generation in the SVG package.
/// </summary>
public class SvgQRCodeOptions
{
    /// <summary>
    /// Gets or sets the error correction level for the QR code.
    /// Higher levels allow more data recovery if the code is damaged, but reduce capacity.
    /// Default is M (medium, ~15% recovery).
    /// </summary>
    public SvgErrorCorrectionLevel ErrorCorrection { get; set; } = SvgErrorCorrectionLevel.M;

    /// <summary>
    /// Gets or sets the size of the quiet zone (border) around the QR code in modules.
    /// QR spec recommends at least 4, but 1 works for most scanners. Default is 1.
    /// </summary>
    public int QuietZoneSize { get; set; } = 1;
}
