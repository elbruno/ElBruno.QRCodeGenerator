namespace ElBruno.QRCodeGenerator.CLI;

/// <summary>
/// Configuration options for QR code generation and rendering.
/// </summary>
public class QRCodeOptions
{
    /// <summary>
    /// Gets or sets the error correction level for the QR code.
    /// Higher levels allow more data recovery if the code is damaged, but reduce capacity.
    /// Default is M (medium, ~15% recovery).
    /// </summary>
    public ErrorCorrectionLevel ErrorCorrection { get; set; } = ErrorCorrectionLevel.M;

    /// <summary>
    /// Gets or sets whether to invert the colors (light background, dark foreground).
    /// Useful for light-themed terminals. Default is false.
    /// </summary>
    public bool InvertColors { get; set; } = false;

    /// <summary>
    /// Gets or sets the size of the quiet zone (border) around the QR code in modules.
    /// QR spec recommends at least 4, but 1 works for most scanners. Default is 1.
    /// </summary>
    public int QuietZoneSize { get; set; } = 1;
}

/// <summary>
/// Error correction level for QR code encoding.
/// Higher levels provide more error recovery but reduce data capacity.
/// </summary>
public enum ErrorCorrectionLevel
{
    /// <summary>
    /// Low - recovers approximately 7% of data
    /// </summary>
    L = 0,

    /// <summary>
    /// Medium - recovers approximately 15% of data (recommended default)
    /// </summary>
    M = 1,

    /// <summary>
    /// Quartile - recovers approximately 25% of data
    /// </summary>
    Q = 2,

    /// <summary>
    /// High - recovers approximately 30% of data
    /// </summary>
    H = 3
}
