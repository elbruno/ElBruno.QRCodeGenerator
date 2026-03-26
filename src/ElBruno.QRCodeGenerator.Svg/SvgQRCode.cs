namespace ElBruno.QRCodeGenerator.Svg;

/// <summary>
/// High-level API for generating QR codes as SVG markup.
/// </summary>
public static class SvgQRCode
{
    /// <summary>
    /// Generates a QR code as a complete SVG document string.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="qrOptions">Optional QR generation options (error correction, quiet zone).</param>
    /// <param name="svgOptions">Optional SVG rendering options (colors, module size).</param>
    /// <returns>A string containing the SVG markup for the QR code.</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static string Generate(string text, SvgQRCodeOptions? qrOptions = null, SvgOptions? svgOptions = null)
    {
        var matrix = CreateMatrix(text, qrOptions);
        return SvgRenderer.GenerateSvg(matrix, svgOptions);
    }

    /// <summary>
    /// Generates a QR code as inline SVG markup (no XML declaration), suitable for embedding in HTML.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="qrOptions">Optional QR generation options (error correction, quiet zone).</param>
    /// <param name="svgOptions">Optional SVG rendering options (colors, module size).</param>
    /// <returns>A string containing the inline SVG markup for the QR code.</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static string GenerateInline(string text, SvgQRCodeOptions? qrOptions = null, SvgOptions? svgOptions = null)
    {
        var matrix = CreateMatrix(text, qrOptions);
        return SvgRenderer.GenerateSvgInline(matrix, svgOptions);
    }

    private static bool[,] CreateMatrix(string text, SvgQRCodeOptions? qrOptions)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be empty or whitespace.", nameof(text));

        qrOptions ??= new SvgQRCodeOptions();

        try
        {
            using var qrGenerator = new QRCoder.QRCodeGenerator();
            var eccLevel = qrOptions.ErrorCorrection switch
            {
                SvgErrorCorrectionLevel.L => QRCoder.QRCodeGenerator.ECCLevel.L,
                SvgErrorCorrectionLevel.M => QRCoder.QRCodeGenerator.ECCLevel.M,
                SvgErrorCorrectionLevel.Q => QRCoder.QRCodeGenerator.ECCLevel.Q,
                SvgErrorCorrectionLevel.H => QRCoder.QRCodeGenerator.ECCLevel.H,
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
