namespace ElBruno.QRCodeGenerator.Ascii;

/// <summary>
/// High-level API for generating QR codes as ASCII art text.
/// </summary>
public static class AsciiQRCode
{
    /// <summary>
    /// Generates a QR code as an ASCII art string.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="options">Optional QR generation options (error correction).</param>
    /// <param name="asciiOptions">Optional ASCII rendering options (style, characters, border).</param>
    /// <returns>A multi-line string containing the ASCII art QR code.</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static string Generate(string text, AsciiQRCodeOptions? options = null, AsciiOptions? asciiOptions = null)
    {
        var matrix = CreateMatrix(text, options);
        return AsciiRenderer.Render(matrix, asciiOptions);
    }

    /// <summary>
    /// Generates a QR code as ASCII art and prints it to <see cref="Console.Out"/>.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="options">Optional QR generation options (error correction).</param>
    /// <param name="asciiOptions">Optional ASCII rendering options (style, characters, border).</param>
    /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static void Print(string text, AsciiQRCodeOptions? options = null, AsciiOptions? asciiOptions = null)
    {
        var ascii = Generate(text, options, asciiOptions);
        Console.Out.Write(ascii);
    }

    private static bool[,] CreateMatrix(string text, AsciiQRCodeOptions? options)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be empty or whitespace.", nameof(text));

        options ??= new AsciiQRCodeOptions();

        try
        {
            using var qrGenerator = new QRCoder.QRCodeGenerator();
            var eccLevel = options.ErrorCorrection switch
            {
                AsciiErrorCorrectionLevel.Low => QRCoder.QRCodeGenerator.ECCLevel.L,
                AsciiErrorCorrectionLevel.Medium => QRCoder.QRCodeGenerator.ECCLevel.M,
                AsciiErrorCorrectionLevel.Quartile => QRCoder.QRCodeGenerator.ECCLevel.Q,
                AsciiErrorCorrectionLevel.High => QRCoder.QRCodeGenerator.ECCLevel.H,
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
                $"Text is too long to encode as QR code with error correction level {options.ErrorCorrection}. " +
                $"Try reducing text length or lowering error correction level.",
                nameof(text),
                ex);
        }
    }
}
