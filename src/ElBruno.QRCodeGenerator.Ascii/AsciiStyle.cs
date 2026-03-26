namespace ElBruno.QRCodeGenerator.Ascii;

/// <summary>
/// Defines the character style used for ASCII art QR code rendering.
/// </summary>
public enum AsciiStyle
{
    /// <summary>
    /// Block characters: ██ for dark, spaces for light. Most compact representation.
    /// </summary>
    Block,

    /// <summary>
    /// Hash characters: ## for dark, spaces for light.
    /// </summary>
    Hash,

    /// <summary>
    /// Dot characters: ●● for dark, ·· for light. Provides visual distinction between modules.
    /// </summary>
    Dot,

    /// <summary>
    /// Shade characters: uses █▓▒░ gradient effect for borders.
    /// </summary>
    Shade,

    /// <summary>
    /// Custom characters: user provides dark and light character strings via <see cref="AsciiOptions"/>.
    /// </summary>
    Custom
}
