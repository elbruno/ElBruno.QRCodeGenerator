using System.Text;

namespace ElBruno.QRCodeGenerator.CLI.Internal;

/// <summary>
/// Renders QR code module matrix as Unicode block characters for console display.
/// Uses half-block characters to achieve 2:1 aspect ratio correction.
/// </summary>
internal static class ConsoleRenderer
{
    // Unicode block characters for rendering
    private const char UpperHalfBlock = '▀'; // U+2580: top dark, bottom light
    private const char LowerHalfBlock = '▄'; // U+2584: top light, bottom dark
    private const char FullBlock = '█';      // U+2588: both dark
    private const char Space = ' ';          // Both light

    /// <summary>
    /// Renders a QR code module matrix as a string using Unicode block characters.
    /// Processes 2 rows at a time to create 1 console line for aspect ratio correction.
    /// </summary>
    /// <param name="matrix">The QR code module matrix where true = dark, false = light.</param>
    /// <param name="options">Rendering options including quiet zone and color inversion.</param>
    /// <returns>A string containing the rendered QR code.</returns>
    internal static string Render(bool[,] matrix, QRCodeOptions options)
    {
        int originalSize = matrix.GetLength(0);
        int quietZone = Math.Max(0, options.QuietZoneSize);
        
        // Add quiet zone to the matrix
        int expandedSize = originalSize + (quietZone * 2);
        var expandedMatrix = new bool[expandedSize, expandedSize];
        
        // Copy original matrix to center of expanded matrix
        // Quiet zone remains false (light) by default
        for (int row = 0; row < originalSize; row++)
        {
            for (int col = 0; col < originalSize; col++)
            {
                expandedMatrix[row + quietZone, col + quietZone] = matrix[row, col];
            }
        }

        var sb = new StringBuilder();
        
        // Process 2 rows at a time (combining into 1 console line)
        for (int row = 0; row < expandedSize; row += 2)
        {
            for (int col = 0; col < expandedSize; col++)
            {
                bool topModule = expandedMatrix[row, col];
                bool bottomModule = row + 1 < expandedSize ? expandedMatrix[row + 1, col] : false;

                // Apply color inversion if requested
                if (options.InvertColors)
                {
                    topModule = !topModule;
                    bottomModule = !bottomModule;
                }

                // Select character based on top and bottom modules
                char ch = (topModule, bottomModule) switch
                {
                    (true, true) => FullBlock,      // Both dark: █
                    (true, false) => UpperHalfBlock, // Top dark, bottom light: ▀
                    (false, true) => LowerHalfBlock, // Top light, bottom dark: ▄
                    (false, false) => Space          // Both light: space
                };

                sb.Append(ch);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
