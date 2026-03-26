namespace ElBruno.QRCodeGenerator.Pdf;

/// <summary>
/// Configuration options for QR code generation in the PDF package.
/// </summary>
public class PdfQRCodeOptions
{
    /// <summary>
    /// Gets or sets the error correction level for the QR code.
    /// Higher levels allow more data recovery if the code is damaged, but reduce capacity.
    /// Default is Medium (~15% recovery).
    /// </summary>
    public PdfErrorCorrectionLevel ErrorCorrection { get; set; } = PdfErrorCorrectionLevel.Medium;
}
