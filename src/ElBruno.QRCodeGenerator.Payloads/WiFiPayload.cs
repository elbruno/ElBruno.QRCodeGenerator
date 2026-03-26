namespace ElBruno.QRCodeGenerator.Payloads;

/// <summary>
/// Builds a WiFi network connection payload using the WIFI: scheme.
/// </summary>
public class WiFiPayload : IPayload
{
    private readonly string _ssid;
    private readonly string _password;
    private readonly WiFiAuthType _authType;
    private bool _hidden;

    /// <summary>
    /// Creates a new WiFi payload.
    /// </summary>
    /// <param name="ssid">The network SSID. Cannot be null or empty.</param>
    /// <param name="password">The network password.</param>
    /// <param name="authType">The authentication type (defaults to WPA).</param>
    public WiFiPayload(string ssid, string password, WiFiAuthType authType = WiFiAuthType.WPA)
    {
        if (string.IsNullOrEmpty(ssid))
            throw new ArgumentException("SSID cannot be null or empty.", nameof(ssid));

        _ssid = ssid;
        _password = password;
        _authType = authType;
    }

    /// <summary>Sets whether the network is hidden.</summary>
    public WiFiPayload WithHidden(bool hidden)
    {
        _hidden = hidden;
        return this;
    }

    /// <summary>Sets whether the network is metered (informational only, not part of standard scheme).</summary>
    public WiFiPayload WithMetered(bool metered)
    {
        // Metered is informational; the WIFI: scheme does not include it,
        // but we support the fluent API for future compatibility.
        return this;
    }

    /// <inheritdoc />
    public string GetPayloadString()
    {
        var authString = _authType switch
        {
            WiFiAuthType.Open => "nopass",
            WiFiAuthType.WEP => "WEP",
            WiFiAuthType.WPA => "WPA",
            WiFiAuthType.WPA2 => "WPA",
            WiFiAuthType.WPA3 => "WPA",
            _ => "WPA"
        };

        var hiddenFlag = _hidden ? "true" : "false";

        return $"WIFI:T:{authString};S:{Escape(_ssid)};P:{Escape(_password)};H:{hiddenFlag};;";
    }

    private static string Escape(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return input
            .Replace("\\", "\\\\")
            .Replace(";", "\\;")
            .Replace(":", "\\:")
            .Replace("\"", "\\\"");
    }
}
