namespace ElBruno.QRCodeGenerator.Payloads;

/// <summary>
/// Interface for all QR code payload types.
/// </summary>
public interface IPayload
{
    /// <summary>
    /// Returns the formatted payload string suitable for QR code encoding.
    /// </summary>
    string GetPayloadString();
}
