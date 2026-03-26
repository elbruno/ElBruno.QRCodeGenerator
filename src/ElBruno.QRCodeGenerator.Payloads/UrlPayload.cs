namespace ElBruno.QRCodeGenerator.Payloads;

/// <summary>
/// Builds a URL payload, optionally prepending a scheme.
/// </summary>
public class UrlPayload : IPayload
{
    private readonly string _url;
    private string _scheme = "https";

    /// <summary>
    /// Creates a new URL payload.
    /// </summary>
    /// <param name="url">The URL string. Cannot be null or empty.</param>
    public UrlPayload(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentException("URL cannot be null or empty.", nameof(url));

        _url = url;
    }

    /// <summary>Sets the scheme to prepend if the URL does not already include one.</summary>
    public UrlPayload WithScheme(string scheme = "https")
    {
        _scheme = scheme;
        return this;
    }

    /// <inheritdoc />
    public string GetPayloadString()
    {
        if (_url.StartsWith($"{_scheme}://", StringComparison.OrdinalIgnoreCase))
            return _url;

        return $"{_scheme}://{_url}";
    }
}
