namespace ElBruno.QRCodeGenerator.Ascii;

/// <summary>
/// Configuration options for QR code generation in the ASCII package.
/// </summary>
public class AsciiQRCodeOptions
{
    /// <summary>
    /// Gets or sets the error correction level for the QR code.
    /// Higher levels allow more data recovery if the code is damaged, but reduce capacity.
    /// Default is Medium (~15% recovery).
    /// </summary>
    public AsciiErrorCorrectionLevel ErrorCorrection { get; set; } = AsciiErrorCorrectionLevel.Medium;
}
