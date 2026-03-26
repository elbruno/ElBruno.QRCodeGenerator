namespace ElBruno.QRCodeGenerator.Payloads;

/// <summary>
/// Builds a mailto: URI payload for email composition.
/// </summary>
public class EmailPayload : IPayload
{
    private readonly string _to;
    private string? _subject;
    private string? _body;
    private string? _cc;
    private string? _bcc;

    /// <summary>
    /// Creates a new email payload.
    /// </summary>
    /// <param name="to">The recipient email address. Cannot be null or empty.</param>
    public EmailPayload(string to)
    {
        if (string.IsNullOrEmpty(to))
            throw new ArgumentException("Recipient email cannot be null or empty.", nameof(to));

        _to = to;
    }

    /// <summary>Sets the email subject.</summary>
    public EmailPayload WithSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    /// <summary>Sets the email body.</summary>
    public EmailPayload WithBody(string body)
    {
        _body = body;
        return this;
    }

    /// <summary>Sets the CC recipient.</summary>
    public EmailPayload WithCc(string cc)
    {
        _cc = cc;
        return this;
    }

    /// <summary>Sets the BCC recipient.</summary>
    public EmailPayload WithBcc(string bcc)
    {
        _bcc = bcc;
        return this;
    }

    /// <inheritdoc />
    public string GetPayloadString()
    {
        var parameters = new List<string>();

        if (_subject is not null)
            parameters.Add($"subject={Uri.EscapeDataString(_subject)}");

        if (_body is not null)
            parameters.Add($"body={Uri.EscapeDataString(_body)}");

        if (_cc is not null)
            parameters.Add($"cc={_cc}");

        if (_bcc is not null)
            parameters.Add($"bcc={_bcc}");

        var query = parameters.Count > 0 ? "?" + string.Join("&", parameters) : "";
        return $"mailto:{_to}{query}";
    }
}
