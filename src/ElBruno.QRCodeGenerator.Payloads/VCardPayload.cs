using System.Text;

namespace ElBruno.QRCodeGenerator.Payloads;

/// <summary>
/// Builds a vCard 4.0 payload for contact information.
/// </summary>
public class VCardPayload : IPayload
{
    private readonly string _fullName;
    private readonly List<(string Number, VCardPhoneType Type)> _phones = [];
    private readonly List<(string Address, VCardEmailType Type)> _emails = [];
    private string? _organization;
    private string? _url;
    private (string Street, string City, string State, string Zip, string Country)? _address;
    private (string LastName, string FirstName, string MiddleName, string Prefix, string Suffix)? _structuredName;

    /// <summary>
    /// Creates a new vCard payload.
    /// </summary>
    /// <param name="fullName">The full name of the contact. Cannot be null or empty.</param>
    public VCardPayload(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
            throw new ArgumentException("Full name cannot be null or empty.", nameof(fullName));

        _fullName = fullName;
    }

    /// <summary>Adds a phone number to the vCard.</summary>
    public VCardPayload WithPhone(string number, VCardPhoneType type = VCardPhoneType.Mobile)
    {
        _phones.Add((number, type));
        return this;
    }

    /// <summary>Adds an email address to the vCard.</summary>
    public VCardPayload WithEmail(string address, VCardEmailType type = VCardEmailType.Personal)
    {
        _emails.Add((address, type));
        return this;
    }

    /// <summary>Sets the organization.</summary>
    public VCardPayload WithOrganization(string organization)
    {
        _organization = organization;
        return this;
    }

    /// <summary>Sets the URL.</summary>
    public VCardPayload WithUrl(string url)
    {
        _url = url;
        return this;
    }

    /// <summary>
    /// Sets the structured name components (vCard N: property) per vCard 4.0 spec.
    /// Format: N:lastName;firstName;middleName;prefix;suffix
    /// </summary>
    /// <param name="lastName">Family name / surname.</param>
    /// <param name="firstName">Given name / first name.</param>
    /// <param name="middleName">Additional / middle name(s). Defaults to empty.</param>
    /// <param name="prefix">Honorific prefix (e.g. "Dr.", "Mr."). Defaults to empty.</param>
    /// <param name="suffix">Honorific suffix (e.g. "Jr.", "III"). Defaults to empty.</param>
    /// <returns>This instance for fluent chaining.</returns>
    public VCardPayload WithName(string lastName, string firstName, string? middleName = null, string? prefix = null, string? suffix = null)
    {
        _structuredName = (lastName, firstName, middleName ?? string.Empty, prefix ?? string.Empty, suffix ?? string.Empty);
        return this;
    }

    /// <summary>Sets the address.</summary>
    public VCardPayload WithAddress(string street, string city, string state, string zip, string country)
    {
        _address = (street, city, state, zip, country);
        return this;
    }

    /// <inheritdoc />
    public string GetPayloadString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VCARD");
        sb.AppendLine("VERSION:4.0");

        if (_structuredName is not null)
        {
            var n = _structuredName.Value;
            sb.AppendLine($"N:{n.LastName};{n.FirstName};{n.MiddleName};{n.Prefix};{n.Suffix}");
        }

        sb.AppendLine($"FN:{_fullName}");

        foreach (var (number, type) in _phones)
        {
            var typeStr = type.ToString().ToUpperInvariant();
            sb.AppendLine($"TEL;TYPE={typeStr}:{number}");
        }

        foreach (var (address, type) in _emails)
        {
            var typeStr = type switch
            {
                VCardEmailType.Personal => "HOME",
                VCardEmailType.Work => "WORK",
                _ => "HOME"
            };
            sb.AppendLine($"EMAIL;TYPE={typeStr}:{address}");
        }

        if (_organization is not null)
            sb.AppendLine($"ORG:{_organization}");

        if (_url is not null)
            sb.AppendLine($"URL:{_url}");

        if (_address is not null)
        {
            var a = _address.Value;
            sb.AppendLine($"ADR;TYPE=work:;;{a.Street};{a.City};{a.State};{a.Zip};{a.Country}");
        }

        sb.Append("END:VCARD");
        return sb.ToString();
    }
}
