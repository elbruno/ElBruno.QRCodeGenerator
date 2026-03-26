namespace ElBruno.QRCodeGenerator.Image;

/// <summary>
/// Error correction level for QR code encoding.
/// Higher levels provide more error recovery but reduce data capacity.
/// </summary>
public enum ImageErrorCorrectionLevel
{
    /// <summary>
    /// Low — recovers approximately 7% of data.
    /// </summary>
    L = 0,

    /// <summary>
    /// Medium — recovers approximately 15% of data (recommended default).
    /// </summary>
    M = 1,

    /// <summary>
    /// Quartile — recovers approximately 25% of data.
    /// </summary>
    Q = 2,

    /// <summary>
    /// High — recovers approximately 30% of data.
    /// </summary>
    H = 3
}
