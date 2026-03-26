using System.Text;

namespace ElBruno.QRCodeGenerator.Ascii;

/// <summary>
/// Renders a QR code boolean matrix as ASCII art text.
/// </summary>
public static class AsciiRenderer
{
    /// <summary>
    /// Renders a QR code matrix to an ASCII art string.
    /// </summary>
    /// <param name="matrix">A 2D boolean array where true represents a dark module.</param>
    /// <param name="options">Optional ASCII rendering options.</param>
    /// <returns>A multi-line string containing the ASCII art QR code.</returns>
    public static string Render(bool[,] matrix, AsciiOptions? options = null)
    {
        options ??= new AsciiOptions();

        var (dark, light) = GetCharacters(options);
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        int quietZone = options.QuietZone;

        var sb = new StringBuilder();

        if (options.Border)
        {
            int contentWidth = (cols + 2 * quietZone) * dark.Length;
            string borderLine = "+" + new string('-', contentWidth) + "+";
            sb.AppendLine(borderLine);
        }

        // Top quiet zone rows
        for (int q = 0; q < quietZone; q++)
        {
            AppendQuietRow(sb, cols, quietZone, light, options.Border);
        }

        // Matrix rows
        for (int row = 0; row < rows; row++)
        {
            if (options.Border)
                sb.Append('|');

            // Left quiet zone
            for (int q = 0; q < quietZone; q++)
                sb.Append(light);

            for (int col = 0; col < cols; col++)
            {
                sb.Append(matrix[row, col] ? dark : light);
            }

            // Right quiet zone
            for (int q = 0; q < quietZone; q++)
                sb.Append(light);

            if (options.Border)
                sb.Append('|');

            sb.AppendLine();
        }

        // Bottom quiet zone rows
        for (int q = 0; q < quietZone; q++)
        {
            AppendQuietRow(sb, cols, quietZone, light, options.Border);
        }

        if (options.Border)
        {
            int contentWidth = (cols + 2 * quietZone) * dark.Length;
            string borderLine = "+" + new string('-', contentWidth) + "+";
            sb.AppendLine(borderLine);
        }

        return sb.ToString();
    }

    private static void AppendQuietRow(StringBuilder sb, int cols, int quietZone, string light, bool border)
    {
        if (border)
            sb.Append('|');

        int totalCols = cols + 2 * quietZone;
        for (int i = 0; i < totalCols; i++)
            sb.Append(light);

        if (border)
            sb.Append('|');

        sb.AppendLine();
    }

    private static (string dark, string light) GetCharacters(AsciiOptions options)
    {
        return options.Style switch
        {
            AsciiStyle.Block => ("██", "  "),
            AsciiStyle.Hash => ("##", "  "),
            AsciiStyle.Dot => ("●●", "··"),
            AsciiStyle.Shade => ("██", "░░"),
            AsciiStyle.Custom => (options.DarkCharacter, options.LightCharacter),
            _ => ("██", "  ")
        };
    }
}
