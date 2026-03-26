namespace ElBruno.QRCodeGenerator.Pdf;

/// <summary>
/// High-level API for generating QR codes as PDF documents.
/// </summary>
public static class PdfQRCode
{
    /// <summary>
    /// Generates a PDF document containing a QR code.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="options">Optional QR generation options (error correction).</param>
    /// <param name="pdfOptions">Optional PDF rendering options (colors, page size, position).</param>
    /// <returns>A byte array containing the PDF document.</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static byte[] Generate(string text, PdfQRCodeOptions? options = null, PdfOptions? pdfOptions = null)
    {
        var matrix = CreateMatrix(text, options);
        return PdfRenderer.Render(matrix, pdfOptions);
    }

    /// <summary>
    /// Generates a PDF document containing a QR code and saves it to a file.
    /// </summary>
    /// <param name="text">The text to encode in the QR code. Cannot be null or empty.</param>
    /// <param name="filePath">The file path to save the PDF to.</param>
    /// <param name="options">Optional QR generation options (error correction).</param>
    /// <param name="pdfOptions">Optional PDF rendering options (colors, page size, position).</param>
    /// <exception cref="ArgumentNullException">Thrown when text or filePath is null.</exception>
    /// <exception cref="ArgumentException">Thrown when text is empty or exceeds QR code capacity.</exception>
    public static void Save(string text, string filePath, PdfQRCodeOptions? options = null, PdfOptions? pdfOptions = null)
    {
        if (filePath == null)
            throw new ArgumentNullException(nameof(filePath));

        var pdfBytes = Generate(text, options, pdfOptions);
        File.WriteAllBytes(filePath, pdfBytes);
    }

    private static bool[,] CreateMatrix(string text, PdfQRCodeOptions? options)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be empty or whitespace.", nameof(text));

        options ??= new PdfQRCodeOptions();

        try
        {
            using var qrGenerator = new QRCoder.QRCodeGenerator();
            var eccLevel = options.ErrorCorrection switch
            {
                PdfErrorCorrectionLevel.Low => QRCoder.QRCodeGenerator.ECCLevel.L,
                PdfErrorCorrectionLevel.Medium => QRCoder.QRCodeGenerator.ECCLevel.M,
                PdfErrorCorrectionLevel.Quartile => QRCoder.QRCodeGenerator.ECCLevel.Q,
                PdfErrorCorrectionLevel.High => QRCoder.QRCodeGenerator.ECCLevel.H,
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
