namespace ElBruno.QRCodeGenerator.CLI;

/// <summary>
/// Main entry point for generating and displaying QR codes in the console.
/// </summary>
public static class QRCode
{
    /// <summary>
    /// Generates a QR code as a string of Unicode block characters.
    /// The returned string can be printed to console or saved to a file.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="options">Optional configuration for QR generation and rendering.</param>
    /// <returns>A string containing the QR code rendered with Unicode block characters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static string Generate(string text, QRCodeOptions? options = null)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be empty or whitespace.", nameof(text));

        options ??= new QRCodeOptions();

        try
        {
            // Generate QR code data using QRCoder
            using var qrGenerator = new QRCoder.QRCodeGenerator();
            var eccLevel = options.ErrorCorrection switch
            {
                ErrorCorrectionLevel.L => QRCoder.QRCodeGenerator.ECCLevel.L,
                ErrorCorrectionLevel.M => QRCoder.QRCodeGenerator.ECCLevel.M,
                ErrorCorrectionLevel.Q => QRCoder.QRCodeGenerator.ECCLevel.Q,
                ErrorCorrectionLevel.H => QRCoder.QRCodeGenerator.ECCLevel.H,
                _ => QRCoder.QRCodeGenerator.ECCLevel.M
            };

            using var qrCodeData = qrGenerator.CreateQrCode(text, eccLevel);
            
            // Extract module matrix from QRCodeData
            var moduleMatrix = qrCodeData.ModuleMatrix;
            int size = moduleMatrix.Count;
            
            // Convert to bool[,] array (true = dark module, false = light module)
            var matrix = new bool[size, size];
            for (int row = 0; row < size; row++)
            {
                var bitArray = moduleMatrix[row];
                for (int col = 0; col < size; col++)
                {
                    matrix[row, col] = bitArray[col];
                }
            }

            // Render using Unicode block characters
            return Internal.ConsoleRenderer.Render(matrix, options);
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

    /// <summary>
    /// Generates and prints a QR code directly to Console.Out.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="options">Optional configuration for QR generation and rendering.</param>
    /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static void Print(string text, QRCodeOptions? options = null)
    {
        var qrCode = Generate(text, options);
        Console.WriteLine(qrCode);
    }
}
