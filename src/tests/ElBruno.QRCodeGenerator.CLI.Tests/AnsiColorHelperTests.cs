using System;
using Xunit;

namespace ElBruno.QRCodeGenerator.CLI.Tests;

/// <summary>
/// Tests for AnsiColorHelper internal helper and ANSI color rendering through the public API.
/// </summary>
public class AnsiColorHelperTests
{
    private const string EscapePrefix = "\x1b[";

    // ── Reset ──

    [Fact]
    public void Reset_ReturnsCorrectSequence()
    {
        Assert.Equal("\x1b[0m", AnsiColorHelper.Reset);
    }

    // ── ConsoleColor → ANSI foreground mapping (all 16 colors) ──

    [Theory]
    [InlineData(ConsoleColor.Black, "\x1b[30m")]
    [InlineData(ConsoleColor.DarkRed, "\x1b[31m")]
    [InlineData(ConsoleColor.DarkGreen, "\x1b[32m")]
    [InlineData(ConsoleColor.DarkYellow, "\x1b[33m")]
    [InlineData(ConsoleColor.DarkBlue, "\x1b[34m")]
    [InlineData(ConsoleColor.DarkMagenta, "\x1b[35m")]
    [InlineData(ConsoleColor.DarkCyan, "\x1b[36m")]
    [InlineData(ConsoleColor.Gray, "\x1b[37m")]
    [InlineData(ConsoleColor.DarkGray, "\x1b[90m")]
    [InlineData(ConsoleColor.Red, "\x1b[91m")]
    [InlineData(ConsoleColor.Green, "\x1b[92m")]
    [InlineData(ConsoleColor.Yellow, "\x1b[93m")]
    [InlineData(ConsoleColor.Blue, "\x1b[94m")]
    [InlineData(ConsoleColor.Magenta, "\x1b[95m")]
    [InlineData(ConsoleColor.Cyan, "\x1b[96m")]
    [InlineData(ConsoleColor.White, "\x1b[97m")]
    public void GetForegroundAnsi_MapsAllConsoleColors(ConsoleColor color, string expected)
    {
        Assert.Equal(expected, AnsiColorHelper.GetForegroundAnsi(color));
    }

    // ── ConsoleColor → ANSI background mapping (all 16 colors) ──

    [Theory]
    [InlineData(ConsoleColor.Black, "\x1b[40m")]
    [InlineData(ConsoleColor.DarkRed, "\x1b[41m")]
    [InlineData(ConsoleColor.DarkGreen, "\x1b[42m")]
    [InlineData(ConsoleColor.DarkYellow, "\x1b[43m")]
    [InlineData(ConsoleColor.DarkBlue, "\x1b[44m")]
    [InlineData(ConsoleColor.DarkMagenta, "\x1b[45m")]
    [InlineData(ConsoleColor.DarkCyan, "\x1b[46m")]
    [InlineData(ConsoleColor.Gray, "\x1b[47m")]
    [InlineData(ConsoleColor.DarkGray, "\x1b[100m")]
    [InlineData(ConsoleColor.Red, "\x1b[101m")]
    [InlineData(ConsoleColor.Green, "\x1b[102m")]
    [InlineData(ConsoleColor.Yellow, "\x1b[103m")]
    [InlineData(ConsoleColor.Blue, "\x1b[104m")]
    [InlineData(ConsoleColor.Magenta, "\x1b[105m")]
    [InlineData(ConsoleColor.Cyan, "\x1b[106m")]
    [InlineData(ConsoleColor.White, "\x1b[107m")]
    public void GetBackgroundAnsi_MapsAllConsoleColors(ConsoleColor color, string expected)
    {
        Assert.Equal(expected, AnsiColorHelper.GetBackgroundAnsi(color));
    }

    // ── TrueColor ANSI code generation ──

    [Fact]
    public void GetTrueColorForeground_ReturnsCorrectSequence()
    {
        Assert.Equal("\x1b[38;2;255;128;0m", AnsiColorHelper.GetTrueColorForeground(255, 128, 0));
    }

    [Fact]
    public void GetTrueColorBackground_ReturnsCorrectSequence()
    {
        Assert.Equal("\x1b[48;2;0;64;255m", AnsiColorHelper.GetTrueColorBackground(0, 64, 255));
    }

    [Fact]
    public void GetTrueColorForeground_BoundaryValues()
    {
        Assert.Equal("\x1b[38;2;0;0;0m", AnsiColorHelper.GetTrueColorForeground(0, 0, 0));
        Assert.Equal("\x1b[38;2;255;255;255m", AnsiColorHelper.GetTrueColorForeground(255, 255, 255));
    }

    [Fact]
    public void GetTrueColorBackground_BoundaryValues()
    {
        Assert.Equal("\x1b[48;2;0;0;0m", AnsiColorHelper.GetTrueColorBackground(0, 0, 0));
        Assert.Equal("\x1b[48;2;255;255;255m", AnsiColorHelper.GetTrueColorBackground(255, 255, 255));
    }

    // ── Default behavior (no colors) — backward compatibility ──

    [Fact]
    public void Generate_WithDefaultOptions_ContainsNoAnsiCodes()
    {
        var result = QRCode.Generate("No Color Test");
        Assert.DoesNotContain("\x1b[", result);
    }

    // ── Colored QR output contains ANSI codes ──

    [Fact]
    public void Generate_WithForegroundColor_ContainsAnsiCodes()
    {
        var options = new QRCodeOptions { ForegroundColor = ConsoleColor.Green };
        var result = QRCode.Generate("FG Test", options);

        Assert.Contains("\x1b[92m", result);   // Green foreground
        Assert.Contains("\x1b[0m", result);    // Reset at line ends
    }

    [Fact]
    public void Generate_WithBackgroundColor_ContainsAnsiCodes()
    {
        var options = new QRCodeOptions { BackgroundColor = ConsoleColor.Blue };
        var result = QRCode.Generate("BG Test", options);

        Assert.Contains("\x1b[104m", result);  // Blue background
        Assert.Contains("\x1b[0m", result);
    }

    [Fact]
    public void Generate_WithBothColors_ContainsBothAnsiCodes()
    {
        var options = new QRCodeOptions
        {
            ForegroundColor = ConsoleColor.White,
            BackgroundColor = ConsoleColor.DarkBlue
        };
        var result = QRCode.Generate("Both Test", options);

        Assert.Contains("\x1b[97m", result);   // White foreground
        Assert.Contains("\x1b[44m", result);   // DarkBlue background
        Assert.Contains("\x1b[0m", result);
    }

    [Fact]
    public void Generate_WithTrueColor_ContainsTrueColorAnsiCodes()
    {
        var options = new QRCodeOptions
        {
            UseTrueColor = true,
            TrueColorForeground = (255, 0, 128),
            TrueColorBackground = (0, 0, 64)
        };
        var result = QRCode.Generate("TrueColor Test", options);

        Assert.Contains("\x1b[38;2;255;0;128m", result);  // Truecolor foreground
        Assert.Contains("\x1b[48;2;0;0;64m", result);     // Truecolor background
        Assert.Contains("\x1b[0m", result);
    }

    [Fact]
    public void Generate_WithTrueColorForegroundOnly_ContainsOnlyFgAnsi()
    {
        var options = new QRCodeOptions
        {
            UseTrueColor = true,
            TrueColorForeground = (100, 200, 50)
        };
        var result = QRCode.Generate("TC FG Only", options);

        Assert.Contains("\x1b[38;2;100;200;50m", result);
        Assert.DoesNotContain("\x1b[48;2;", result);       // No background truecolor
        Assert.Contains("\x1b[0m", result);
    }

    [Fact]
    public void Generate_WithTrueColorBackgroundOnly_ContainsOnlyBgAnsi()
    {
        var options = new QRCodeOptions
        {
            UseTrueColor = true,
            TrueColorBackground = (30, 30, 30)
        };
        var result = QRCode.Generate("TC BG Only", options);

        Assert.DoesNotContain("\x1b[38;2;", result);       // No foreground truecolor
        Assert.Contains("\x1b[48;2;30;30;30m", result);
        Assert.Contains("\x1b[0m", result);
    }

    // ── TrueColor takes precedence over ConsoleColor ──

    [Fact]
    public void Generate_TrueColorOverridesConsoleColor()
    {
        var options = new QRCodeOptions
        {
            ForegroundColor = ConsoleColor.Red,       // Should be ignored
            BackgroundColor = ConsoleColor.Blue,      // Should be ignored
            UseTrueColor = true,
            TrueColorForeground = (10, 20, 30),
            TrueColorBackground = (40, 50, 60)
        };
        var result = QRCode.Generate("Precedence Test", options);

        Assert.Contains("\x1b[38;2;10;20;30m", result);
        Assert.Contains("\x1b[48;2;40;50;60m", result);
        Assert.DoesNotContain("\x1b[91m", result);  // Not Red ConsoleColor
        Assert.DoesNotContain("\x1b[104m", result); // Not Blue ConsoleColor
    }

    // ── Each line has reset at end ──

    [Fact]
    public void Generate_WithColors_EachLineEndsWithReset()
    {
        var options = new QRCodeOptions { ForegroundColor = ConsoleColor.Cyan };
        var result = QRCode.Generate("Line Reset Test", options);

        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            Assert.EndsWith("\x1b[0m", line.TrimEnd('\r'));
        }
    }
}
