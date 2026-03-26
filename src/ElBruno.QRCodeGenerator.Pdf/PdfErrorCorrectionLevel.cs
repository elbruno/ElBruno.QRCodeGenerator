namespace ElBruno.QRCodeGenerator.Pdf;

/// <summary>
/// Error correction level for QR code encoding.
/// Higher levels provide more error recovery but reduce data capacity.
/// </summary>
public enum PdfErrorCorrectionLevel
{
    /// <summary>
    /// Low - recovers approximately 7% of data
    /// </summary>
    Low = 0,

    /// <summary>
    /// Medium - recovers approximately 15% of data (recommended default)
    /// </summary>
    Medium = 1,

    /// <summary>
    /// Quartile - recovers approximately 25% of data
    /// </summary>
    Quartile = 2,

    /// <summary>
    /// High - recovers approximately 30% of data
    /// </summary>
    High = 3
}
