using System;

namespace ElBruno.QRCodeGenerator.CLI;

/// <summary>
/// Internal helper for generating ANSI escape sequences for terminal color output.
/// Supports standard 16-color (ConsoleColor) and 24-bit truecolor modes.
/// </summary>
internal static class AnsiColorHelper
{
    private const char Escape = '\x1b';

    /// <summary>
    /// ANSI reset sequence that restores default terminal colors and attributes.
    /// </summary>
    internal static string Reset => $"{Escape}[0m";

    /// <summary>
    /// Returns the ANSI foreground escape sequence for a <see cref="ConsoleColor"/> value.
    /// Maps to codes \x1b[30m–\x1b[37m (standard) and \x1b[90m–\x1b[97m (bright).
    /// </summary>
    internal static string GetForegroundAnsi(ConsoleColor color)
    {
        int code = MapConsoleColorToAnsiForeground(color);
        return $"{Escape}[{code}m";
    }

    /// <summary>
    /// Returns the ANSI background escape sequence for a <see cref="ConsoleColor"/> value.
    /// Maps to codes \x1b[40m–\x1b[47m (standard) and \x1b[100m–\x1b[107m (bright).
    /// </summary>
    internal static string GetBackgroundAnsi(ConsoleColor color)
    {
        int code = MapConsoleColorToAnsiForeground(color) + 10;
        return $"{Escape}[{code}m";
    }

    /// <summary>
    /// Returns a 24-bit truecolor ANSI foreground escape sequence: \x1b[38;2;R;G;Bm
    /// </summary>
    internal static string GetTrueColorForeground(byte r, byte g, byte b)
    {
        return $"{Escape}[38;2;{r};{g};{b}m";
    }

    /// <summary>
    /// Returns a 24-bit truecolor ANSI background escape sequence: \x1b[48;2;R;G;Bm
    /// </summary>
    internal static string GetTrueColorBackground(byte r, byte g, byte b)
    {
        return $"{Escape}[48;2;{r};{g};{b}m";
    }

    private static int MapConsoleColorToAnsiForeground(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => 30,
            ConsoleColor.DarkRed => 31,
            ConsoleColor.DarkGreen => 32,
            ConsoleColor.DarkYellow => 33,
            ConsoleColor.DarkBlue => 34,
            ConsoleColor.DarkMagenta => 35,
            ConsoleColor.DarkCyan => 36,
            ConsoleColor.Gray => 37,
            ConsoleColor.DarkGray => 90,
            ConsoleColor.Red => 91,
            ConsoleColor.Green => 92,
            ConsoleColor.Yellow => 93,
            ConsoleColor.Blue => 94,
            ConsoleColor.Magenta => 95,
            ConsoleColor.Cyan => 96,
            ConsoleColor.White => 97,
            _ => 37 // Default to gray
        };
    }
}
