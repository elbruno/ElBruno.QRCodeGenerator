using System.CommandLine;
using System.CommandLine.Invocation;
using ElBruno.QRCodeGenerator.Ascii;
using ElBruno.QRCodeGenerator.CLI;
using ElBruno.QRCodeGenerator.Image;
using ElBruno.QRCodeGenerator.Pdf;
using ElBruno.QRCodeGenerator.Svg;

namespace ElBruno.QRCodeGenerator.Tool;

public static class GenerateCommand
{
    public static RootCommand Create()
    {
        var textArgument = new Argument<string>("text", "The text to encode in the QR code");

        var formatOption = new Option<string>(
            "--format",
            getDefaultValue: () => "console",
            description: "Output format (console|svg|png|jpeg|ascii|pdf)");

        var outputOption = new Option<string?>(
            "--output",
            description: "Write to file (required for png/jpeg/pdf)");

        var moduleSizeOption = new Option<int>(
            "--module-size",
            getDefaultValue: () => 10,
            description: "Pixels/units per module");

        var fgOption = new Option<string>(
            "--fg",
            getDefaultValue: () => "#000000",
            description: "Foreground color as hex");

        var bgOption = new Option<string>(
            "--bg",
            getDefaultValue: () => "#ffffff",
            description: "Background color as hex");

        var eccOption = new Option<string>(
            "--error-correction",
            getDefaultValue: () => "medium",
            description: "Error correction level (low|medium|quartile|high)");

        var invertOption = new Option<bool>(
            "--invert",
            getDefaultValue: () => false,
            description: "Invert colors (console mode only)");

        var qualityOption = new Option<int>(
            "--quality",
            getDefaultValue: () => 85,
            description: "JPEG quality 0-100");

        var asciiStyleOption = new Option<string>(
            "--ascii-style",
            getDefaultValue: () => "block",
            description: "ASCII art style (block|hash|dot|shade)");

        var titleOption = new Option<string?>(
            "--title",
            description: "Title for PDF output");

        var rootCommand = new RootCommand("QR code generator tool — encode text as console, SVG, PNG, JPEG, ASCII, or PDF")
        {
            textArgument,
            formatOption,
            outputOption,
            moduleSizeOption,
            fgOption,
            bgOption,
            eccOption,
            invertOption,
            qualityOption,
            asciiStyleOption,
            titleOption
        };

        rootCommand.SetHandler(
            (InvocationContext context) =>
            {
                var text = context.ParseResult.GetValueForArgument(textArgument);
                var format = context.ParseResult.GetValueForOption(formatOption)!;
                var output = context.ParseResult.GetValueForOption(outputOption);
                var moduleSize = context.ParseResult.GetValueForOption(moduleSizeOption);
                var fg = context.ParseResult.GetValueForOption(fgOption)!;
                var bg = context.ParseResult.GetValueForOption(bgOption)!;
                var ecc = context.ParseResult.GetValueForOption(eccOption)!;
                var invert = context.ParseResult.GetValueForOption(invertOption);
                var quality = context.ParseResult.GetValueForOption(qualityOption);
                var asciiStyle = context.ParseResult.GetValueForOption(asciiStyleOption)!;
                var title = context.ParseResult.GetValueForOption(titleOption);

                context.ExitCode = Execute(text, format, output, moduleSize, fg, bg, ecc, invert, quality, asciiStyle, title);
            });

        return rootCommand;
    }

    public static int Execute(
        string text,
        string format,
        string? output,
        int moduleSize,
        string fg,
        string bg,
        string ecc,
        bool invert,
        int quality,
        string asciiStyle,
        string? title)
    {
        try
        {
            switch (format.ToLowerInvariant())
            {
                case "console":
                    return HandleConsole(text, ecc, invert, output);

                case "svg":
                    return HandleSvg(text, ecc, moduleSize, fg, bg, output);

                case "png":
                    return HandleImage(text, ecc, moduleSize, fg, bg, output, ImageFormat.Png, quality);

                case "jpeg":
                    return HandleImage(text, ecc, moduleSize, fg, bg, output, ImageFormat.Jpeg, quality);

                case "ascii":
                    return HandleAscii(text, ecc, asciiStyle, output);

                case "pdf":
                    return HandlePdf(text, ecc, moduleSize, fg, bg, output, title);

                default:
                    Console.Error.WriteLine($"Error: Unknown format '{format}'. Use console, svg, png, jpeg, ascii, or pdf.");
                    return 1;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }

    private static int HandleConsole(string text, string ecc, bool invert, string? output)
    {
        var options = new QRCodeOptions
        {
            ErrorCorrection = MapCliEcc(ecc),
            InvertColors = invert
        };

        var result = QRCode.Generate(text, options);

        if (output is not null)
        {
            File.WriteAllText(output, result);
        }
        else
        {
            Console.Write(result);
        }

        return 0;
    }

    private static int HandleSvg(string text, string ecc, int moduleSize, string fg, string bg, string? output)
    {
        var qrOptions = new SvgQRCodeOptions
        {
            ErrorCorrection = MapSvgEcc(ecc)
        };

        var svgOptions = new SvgOptions
        {
            ModuleSize = moduleSize,
            ForegroundColor = fg,
            BackgroundColor = bg
        };

        var result = SvgQRCode.Generate(text, qrOptions, svgOptions);

        if (output is not null)
        {
            File.WriteAllText(output, result);
        }
        else
        {
            Console.Write(result);
        }

        return 0;
    }

    private static int HandleImage(string text, string ecc, int moduleSize, string fg, string bg, string? output, ImageFormat format, int quality)
    {
        if (string.IsNullOrEmpty(output))
        {
            Console.Error.WriteLine($"Error: --output is required for {format.ToString().ToLowerInvariant()} format.");
            return 1;
        }

        var qrOptions = new ImageQRCodeOptions
        {
            ErrorCorrection = MapImageEcc(ecc)
        };

        var imageOptions = new ImageOptions
        {
            Format = format,
            ModuleSize = moduleSize,
            ForegroundColor = fg,
            BackgroundColor = bg,
            JpegQuality = quality
        };

        var bytes = ImageQRCode.Generate(text, qrOptions, imageOptions);
        File.WriteAllBytes(output, bytes);
        return 0;
    }

    private static int HandleAscii(string text, string ecc, string asciiStyle, string? output)
    {
        var qrOptions = new AsciiQRCodeOptions
        {
            ErrorCorrection = MapAsciiEcc(ecc)
        };

        var asciiOptions = new AsciiOptions
        {
            Style = MapAsciiStyle(asciiStyle)
        };

        var result = AsciiQRCode.Generate(text, qrOptions, asciiOptions);

        if (output is not null)
        {
            File.WriteAllText(output, result);
        }
        else
        {
            Console.Write(result);
        }

        return 0;
    }

    private static int HandlePdf(string text, string ecc, int moduleSize, string fg, string bg, string? output, string? title)
    {
        if (string.IsNullOrEmpty(output))
        {
            Console.Error.WriteLine("Error: --output is required for pdf format.");
            return 1;
        }

        var qrOptions = new PdfQRCodeOptions
        {
            ErrorCorrection = MapPdfEcc(ecc)
        };

        var pdfOptions = new PdfOptions
        {
            ModuleSize = moduleSize,
            ForegroundColor = fg,
            BackgroundColor = bg,
            Title = title
        };

        var bytes = PdfQRCode.Generate(text, qrOptions, pdfOptions);
        File.WriteAllBytes(output, bytes);
        return 0;
    }

    public static ErrorCorrectionLevel MapCliEcc(string level) => level.ToLowerInvariant() switch
    {
        "low" => ErrorCorrectionLevel.L,
        "medium" => ErrorCorrectionLevel.M,
        "quartile" => ErrorCorrectionLevel.Q,
        "high" => ErrorCorrectionLevel.H,
        _ => ErrorCorrectionLevel.M
    };

    public static SvgErrorCorrectionLevel MapSvgEcc(string level) => level.ToLowerInvariant() switch
    {
        "low" => SvgErrorCorrectionLevel.L,
        "medium" => SvgErrorCorrectionLevel.M,
        "quartile" => SvgErrorCorrectionLevel.Q,
        "high" => SvgErrorCorrectionLevel.H,
        _ => SvgErrorCorrectionLevel.M
    };

    public static ImageErrorCorrectionLevel MapImageEcc(string level) => level.ToLowerInvariant() switch
    {
        "low" => ImageErrorCorrectionLevel.L,
        "medium" => ImageErrorCorrectionLevel.M,
        "quartile" => ImageErrorCorrectionLevel.Q,
        "high" => ImageErrorCorrectionLevel.H,
        _ => ImageErrorCorrectionLevel.M
    };

    public static AsciiErrorCorrectionLevel MapAsciiEcc(string level) => level.ToLowerInvariant() switch
    {
        "low" => AsciiErrorCorrectionLevel.Low,
        "medium" => AsciiErrorCorrectionLevel.Medium,
        "quartile" => AsciiErrorCorrectionLevel.Quartile,
        "high" => AsciiErrorCorrectionLevel.High,
        _ => AsciiErrorCorrectionLevel.Medium
    };

    public static PdfErrorCorrectionLevel MapPdfEcc(string level) => level.ToLowerInvariant() switch
    {
        "low" => PdfErrorCorrectionLevel.Low,
        "medium" => PdfErrorCorrectionLevel.Medium,
        "quartile" => PdfErrorCorrectionLevel.Quartile,
        "high" => PdfErrorCorrectionLevel.High,
        _ => PdfErrorCorrectionLevel.Medium
    };

    public static AsciiStyle MapAsciiStyle(string style) => style.ToLowerInvariant() switch
    {
        "block" => AsciiStyle.Block,
        "hash" => AsciiStyle.Hash,
        "dot" => AsciiStyle.Dot,
        "shade" => AsciiStyle.Shade,
        _ => AsciiStyle.Block
    };
}
