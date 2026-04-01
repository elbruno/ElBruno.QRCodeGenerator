using Xunit;
using ElBruno.QRCodeGenerator.Payloads;

namespace ElBruno.QRCodeGenerator.Payloads.Tests;

public class VCardPayloadTests
{
    [Fact]
    public void BasicVCard_ContainsRequiredFields()
    {
        var payload = new VCardPayload("John Doe");
        var result = payload.GetPayloadString();

        Assert.Contains("BEGIN:VCARD", result);
        Assert.Contains("VERSION:4.0", result);
        Assert.Contains("FN:John Doe", result);
        Assert.Contains("END:VCARD", result);
    }

    [Fact]
    public void WithPhone_AddsPhoneLine()
    {
        var payload = new VCardPayload("Jane Doe")
            .WithPhone("+1234567890", VCardPhoneType.Mobile);
        var result = payload.GetPayloadString();

        Assert.Contains("TEL;TYPE=MOBILE:+1234567890", result);
    }

    [Fact]
    public void WithEmail_AddsEmailLine()
    {
        var payload = new VCardPayload("Jane Doe")
            .WithEmail("jane@example.com", VCardEmailType.Work);
        var result = payload.GetPayloadString();

        Assert.Contains("EMAIL;TYPE=WORK:jane@example.com", result);
    }

    [Fact]
    public void WithOrganization_AddsOrgLine()
    {
        var payload = new VCardPayload("Jane Doe")
            .WithOrganization("Acme Corp");
        var result = payload.GetPayloadString();

        Assert.Contains("ORG:Acme Corp", result);
    }

    [Fact]
    public void WithUrl_AddsUrlLine()
    {
        var payload = new VCardPayload("Jane Doe")
            .WithUrl("https://example.com");
        var result = payload.GetPayloadString();

        Assert.Contains("URL:https://example.com", result);
    }

    [Fact]
    public void WithAddress_AddsAddressLine()
    {
        var payload = new VCardPayload("Jane Doe")
            .WithAddress("123 Main St", "Springfield", "IL", "62704", "USA");
        var result = payload.GetPayloadString();

        Assert.Contains("ADR;TYPE=work:;;123 Main St;Springfield;IL;62704;USA", result);
    }

    [Fact]
    public void MultipleFields_AllIncluded()
    {
        var payload = new VCardPayload("John Doe")
            .WithPhone("+1111111111", VCardPhoneType.Work)
            .WithPhone("+2222222222", VCardPhoneType.Home)
            .WithEmail("john@work.com", VCardEmailType.Work)
            .WithEmail("john@home.com", VCardEmailType.Personal)
            .WithOrganization("Contoso")
            .WithUrl("https://contoso.com")
            .WithAddress("1 Corp Way", "Redmond", "WA", "98052", "USA");
        var result = payload.GetPayloadString();

        Assert.Contains("TEL;TYPE=WORK:+1111111111", result);
        Assert.Contains("TEL;TYPE=HOME:+2222222222", result);
        Assert.Contains("EMAIL;TYPE=WORK:john@work.com", result);
        Assert.Contains("EMAIL;TYPE=HOME:john@home.com", result);
        Assert.Contains("ORG:Contoso", result);
        Assert.Contains("URL:https://contoso.com", result);
        Assert.Contains("ADR;TYPE=work:", result);
    }

    [Fact]
    public void EmptyFullName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new VCardPayload(""));
    }

    [Fact]
    public void NullFullName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new VCardPayload(null!));
    }

    [Fact]
    public void WithName_AllComponents_ContainsStructuredName()
    {
        var payload = new VCardPayload("Dr. John Michael Doe Jr.")
            .WithName("Doe", "John", "Michael", "Dr.", "Jr.");
        var result = payload.GetPayloadString();

        Assert.Contains("N:Doe;John;Michael;Dr.;Jr.", result);
    }

    [Fact]
    public void WithName_MinimalComponents_EmptyOptionalFields()
    {
        var payload = new VCardPayload("John Doe")
            .WithName("Doe", "John");
        var result = payload.GetPayloadString();

        Assert.Contains("N:Doe;John;;;", result);
    }

    [Fact]
    public void WithName_StructuredNameAppearsBeforeFullName()
    {
        var payload = new VCardPayload("John Doe")
            .WithName("Doe", "John");
        var result = payload.GetPayloadString();

        var nIndex = result.IndexOf("N:Doe;John;;;");
        var fnIndex = result.IndexOf("FN:John Doe");
        Assert.True(nIndex >= 0, "N: line should be present");
        Assert.True(fnIndex >= 0, "FN: line should be present");
        Assert.True(nIndex < fnIndex, "N: line should appear before FN: line in vCard output");
    }

    [Fact]
    public void NoWithName_OutputDoesNotContainStructuredName()
    {
        var payload = new VCardPayload("John Doe")
            .WithPhone("+1234567890");
        var result = payload.GetPayloadString();

        Assert.DoesNotContain("\nN:", result);
    }

    [Fact]
    public void WithName_PlusAllOtherFields_AllPresent()
    {
        var payload = new VCardPayload("John Doe")
            .WithName("Doe", "John", "Michael", "Mr.", "III")
            .WithPhone("+1234567890", VCardPhoneType.Mobile)
            .WithEmail("john@example.com", VCardEmailType.Work)
            .WithOrganization("Contoso")
            .WithUrl("https://contoso.com")
            .WithAddress("1 Corp Way", "Redmond", "WA", "98052", "USA");
        var result = payload.GetPayloadString();

        Assert.Contains("N:Doe;John;Michael;Mr.;III", result);
        Assert.Contains("FN:John Doe", result);
        Assert.Contains("TEL;TYPE=MOBILE:+1234567890", result);
        Assert.Contains("EMAIL;TYPE=WORK:john@example.com", result);
        Assert.Contains("ORG:Contoso", result);
        Assert.Contains("URL:https://contoso.com", result);
        Assert.Contains("ADR;TYPE=work:;;1 Corp Way;Redmond;WA;98052;USA", result);
    }

    [Fact]
    public void WithName_EmptyStringOptionalFields_ProducesEmptySemicolonSeparators()
    {
        var payload = new VCardPayload("Jane Smith")
            .WithName("Smith", "Jane", "", "", "");
        var result = payload.GetPayloadString();

        Assert.Contains("N:Smith;Jane;;;", result);
    }
}
