namespace ElBruno.QRCodeGenerator.Image;

/// <summary>
/// Specifies the output image format for QR code rendering.
/// </summary>
public enum ImageFormat
{
    /// <summary>
    /// PNG format — lossless compression, ideal for QR codes.
    /// </summary>
    Png = 0,

    /// <summary>
    /// JPEG format — lossy compression with configurable quality.
    /// </summary>
    Jpeg = 1,

    /// <summary>
    /// WebP format — modern format with good compression.
    /// </summary>
    Webp = 2
}
