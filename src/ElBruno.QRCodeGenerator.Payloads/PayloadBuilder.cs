namespace ElBruno.QRCodeGenerator.Payloads;

/// <summary>
/// Static factory for creating payload builders.
/// </summary>
public static class PayloadBuilder
{
    /// <summary>Creates a WiFi connection payload.</summary>
    public static WiFiPayload Wifi(string ssid, string password) => new(ssid, password);

    /// <summary>Creates a vCard contact payload.</summary>
    public static VCardPayload VCard(string fullName) => new(fullName);

    /// <summary>Creates a mailto: email payload.</summary>
    public static EmailPayload Email(string to) => new(to);

    /// <summary>Creates an SMS payload.</summary>
    public static SmsPayload Sms(string phoneNumber) => new(phoneNumber);

    /// <summary>Creates a geographic coordinate payload.</summary>
    public static GeoPayload Geo(double lat, double lon) => new(lat, lon);

    /// <summary>Creates a URL payload.</summary>
    public static UrlPayload Url(string url) => new(url);
}
