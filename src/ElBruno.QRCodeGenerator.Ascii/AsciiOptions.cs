namespace ElBruno.QRCodeGenerator.Ascii;

/// <summary>
/// Configuration options for ASCII art rendering of QR codes.
/// </summary>
public class AsciiOptions
{
    /// <summary>
    /// Gets or sets the character style for rendering.
    /// Default is <see cref="AsciiStyle.Block"/>.
    /// </summary>
    public AsciiStyle Style { get; set; } = AsciiStyle.Block;

    /// <summary>
    /// Gets or sets the string used for dark modules when <see cref="Style"/> is <see cref="AsciiStyle.Custom"/>.
    /// Default is "██".
    /// </summary>
    public string DarkCharacter { get; set; } = "██";

    /// <summary>
    /// Gets or sets the string used for light modules when <see cref="Style"/> is <see cref="AsciiStyle.Custom"/>.
    /// Default is "  " (two spaces).
    /// </summary>
    public string LightCharacter { get; set; } = "  ";

    /// <summary>
    /// Gets or sets the quiet zone (border) size in modules around the QR code.
    /// Default is 1.
    /// </summary>
    public int QuietZone { get; set; } = 1;

    /// <summary>
    /// Gets or sets whether to add a simple border around the output.
    /// Default is false.
    /// </summary>
    public bool Border { get; set; }
}
