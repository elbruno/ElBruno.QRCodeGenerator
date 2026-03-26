namespace ElBruno.QRCodeGenerator.Payloads;

/// <summary>
/// Builds an SMS payload using the sms: URI scheme.
/// </summary>
public class SmsPayload : IPayload
{
    private readonly string _phoneNumber;
    private string? _message;

    /// <summary>
    /// Creates a new SMS payload.
    /// </summary>
    /// <param name="phoneNumber">The phone number. Cannot be null or empty.</param>
    public SmsPayload(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));

        _phoneNumber = phoneNumber;
    }

    /// <summary>Sets the pre-filled message text.</summary>
    public SmsPayload WithMessage(string message)
    {
        _message = message;
        return this;
    }

    /// <inheritdoc />
    public string GetPayloadString()
    {
        if (_message is not null)
            return $"sms:{_phoneNumber}?body={Uri.EscapeDataString(_message)}";

        return $"sms:{_phoneNumber}";
    }
}
