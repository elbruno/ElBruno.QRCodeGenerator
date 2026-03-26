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

    /// <summary>
    /// Foreground (module) color using System.ConsoleColor. Default: null (uses terminal default).
    /// </summary>
    public ConsoleColor? ForegroundColor { get; set; }

    /// <summary>
    /// Background (quiet zone) color using System.ConsoleColor. Default: null (uses terminal default).
    /// </summary>
    public ConsoleColor? BackgroundColor { get; set; }

    /// <summary>
    /// Use truecolor (24-bit) ANSI. Requires modern terminal (Windows Terminal, iTerm2, etc.).
    /// </summary>
    public bool UseTrueColor { get; set; } = false;

    /// <summary>
    /// Custom RGB color for foreground (when UseTrueColor = true). Format: (R, G, B) where each is 0-255.
    /// </summary>
    public (byte R, byte G, byte B)? TrueColorForeground { get; set; }

    /// <summary>
    /// Custom RGB color for background (when UseTrueColor = true). Format: (R, G, B) where each is 0-255.
    /// </summary>
    public (byte R, byte G, byte B)? TrueColorBackground { get; set; }
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
