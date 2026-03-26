namespace ElBruno.QRCodeGenerator.Image;

/// <summary>
/// High-level API for generating QR codes as bitmap images (PNG, JPEG, WebP).
/// </summary>
public static class ImageQRCode
{
    /// <summary>
    /// Generates a QR code as image bytes in the specified format.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="qrOptions">Optional QR generation options (error correction).</param>
    /// <param name="imageOptions">Optional image rendering options (format, size, colors).</param>
    /// <returns>A byte array containing the encoded image data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static byte[] Generate(string text, ImageQRCodeOptions? qrOptions = null, ImageOptions? imageOptions = null)
    {
        var matrix = CreateMatrix(text, qrOptions);
        return ImageRenderer.Render(matrix, imageOptions);
    }

    /// <summary>
    /// Generates a QR code as a PNG image with default settings.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="qrOptions">Optional QR generation options.</param>
    /// <returns>A byte array containing PNG image data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static byte[] ToPng(string text, ImageQRCodeOptions? qrOptions = null)
    {
        return Generate(text, qrOptions, new ImageOptions { Format = ImageFormat.Png });
    }

    /// <summary>
    /// Generates a QR code as a JPEG image with configurable quality.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="quality">JPEG quality (0–100). Default is 85.</param>
    /// <param name="qrOptions">Optional QR generation options.</param>
    /// <returns>A byte array containing JPEG image data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static byte[] ToJpeg(string text, int quality = 85, ImageQRCodeOptions? qrOptions = null)
    {
        return Generate(text, qrOptions, new ImageOptions
        {
            Format = ImageFormat.Jpeg,
            JpegQuality = quality
        });
    }

    private static bool[,] CreateMatrix(string text, ImageQRCodeOptions? qrOptions)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be empty or whitespace.", nameof(text));

        qrOptions ??= new ImageQRCodeOptions();

        try
        {
            using var qrGenerator = new QRCoder.QRCodeGenerator();
            var eccLevel = qrOptions.ErrorCorrection switch
            {
                ImageErrorCorrectionLevel.L => QRCoder.QRCodeGenerator.ECCLevel.L,
                ImageErrorCorrectionLevel.M => QRCoder.QRCodeGenerator.ECCLevel.M,
                ImageErrorCorrectionLevel.Q => QRCoder.QRCodeGenerator.ECCLevel.Q,
                ImageErrorCorrectionLevel.H => QRCoder.QRCodeGenerator.ECCLevel.H,
                _ => QRCoder.QRCodeGenerator.ECCLevel.M
            };

            using var qrCodeData = qrGenerator.CreateQrCode(text, eccLevel);

            var moduleMatrix = qrCodeData.ModuleMatrix;
            int size = moduleMatrix.Count;

            var matrix = new bool[size, size];
            for (int row = 0; row < size; row++)
            {
                var bitArray = moduleMatrix[row];
                for (int col = 0; col < size; col++)
                {
                    matrix[row, col] = bitArray[col];
                }
            }

            return matrix;
        }
        catch (QRCoder.Exceptions.DataTooLongException ex)
        {
            throw new ArgumentException(
                $"Text is too long to encode as QR code with error correction level {qrOptions.ErrorCorrection}. " +
                $"Try reducing text length or lowering error correction level.",
                nameof(text),
                ex);
        }
    }
}
