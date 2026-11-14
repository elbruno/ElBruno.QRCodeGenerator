using System.Reflection;
using PdfSharp.Fonts;

namespace ElBruno.QRCodeGenerator.Pdf;

internal static class PdfFontResolver
{
    private static readonly object SyncRoot = new();

    public static void EnsureInitialized()
    {
        lock (SyncRoot)
        {
            if (GlobalFontSettings.FontResolver is null)
            {
                GlobalFontSettings.FontResolver = new LiberationSansFontResolver();
            }
        }
    }

    private sealed class LiberationSansFontResolver : IFontResolver
    {
        private const string FaceName = "LiberationSans-Regular";
        private const string ResourceName = "ElBruno.QRCodeGenerator.Pdf.Fonts.LiberationSans-Regular.ttf";
        private static readonly byte[] FontBytes = LoadFontBytes();

        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (!familyName.Equals("Arial", StringComparison.OrdinalIgnoreCase) || isBold || isItalic)
            {
                return null;
            }

            return new FontResolverInfo(FaceName);
        }

        public byte[]? GetFont(string faceName)
        {
            return faceName == FaceName
                ? FontBytes
                : throw new ArgumentOutOfRangeException(nameof(faceName), faceName, "Unknown font face.");
        }

        private static byte[] LoadFontBytes()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceName)
                ?? throw new InvalidOperationException($"Embedded font resource '{ResourceName}' was not found.");
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
