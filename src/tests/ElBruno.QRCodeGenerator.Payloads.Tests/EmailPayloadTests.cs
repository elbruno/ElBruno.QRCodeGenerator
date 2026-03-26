using Xunit;
using ElBruno.QRCodeGenerator.Payloads;

namespace ElBruno.QRCodeGenerator.Payloads.Tests;

public class EmailPayloadTests
{
    [Fact]
    public void BasicEmail_ProducesMailto()
    {
        var payload = new EmailPayload("test@example.com");
        var result = payload.GetPayloadString();

        Assert.Equal("mailto:test@example.com", result);
    }

    [Fact]
    public void WithSubject_AddsSubjectParam()
    {
        var payload = new EmailPayload("test@example.com")
            .WithSubject("Hello World");
        var result = payload.GetPayloadString();

        Assert.StartsWith("mailto:test@example.com?", result);
        Assert.Contains("subject=Hello%20World", result);
    }

    [Fact]
    public void WithAllParams_IncludesAll()
    {
        var payload = new EmailPayload("to@example.com")
            .WithSubject("Test")
            .WithBody("Body text")
            .WithCc("cc@example.com")
            .WithBcc("bcc@example.com");
        var result = payload.GetPayloadString();

        Assert.Contains("subject=Test", result);
        Assert.Contains("body=Body%20text", result);
        Assert.Contains("cc=cc@example.com", result);
        Assert.Contains("bcc=bcc@example.com", result);
    }

    [Fact]
    public void UrlEncoding_HandlesSpecialCharacters()
    {
        var payload = new EmailPayload("test@example.com")
            .WithSubject("Hello & Goodbye")
            .WithBody("Line1\nLine2");
        var result = payload.GetPayloadString();

        Assert.Contains("subject=Hello%20%26%20Goodbye", result);
        Assert.Contains("body=Line1%0ALine2", result);
    }

    [Fact]
    public void EmptyTo_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new EmailPayload(""));
    }

    [Fact]
    public void NullTo_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new EmailPayload(null!));
    }
}
