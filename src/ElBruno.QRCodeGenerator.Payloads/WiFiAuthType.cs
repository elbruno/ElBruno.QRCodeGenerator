namespace ElBruno.QRCodeGenerator.Payloads;

/// <summary>
/// WiFi authentication types for the WIFI: QR code scheme.
/// </summary>
public enum WiFiAuthType
{
    /// <summary>Open network (no authentication).</summary>
    Open,

    /// <summary>WEP authentication.</summary>
    WEP,

    /// <summary>WPA authentication.</summary>
    WPA,

    /// <summary>WPA2 authentication (maps to WPA in the WIFI: scheme).</summary>
    WPA2,

    /// <summary>WPA3 authentication (maps to WPA in the WIFI: scheme).</summary>
    WPA3
}
