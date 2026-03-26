using Xunit;
using ElBruno.QRCodeGenerator.Payloads;

namespace ElBruno.QRCodeGenerator.Payloads.Tests;

public class SmsPayloadTests
{
    [Fact]
    public void BasicSms_ProducesScheme()
    {
        var payload = new SmsPayload("+1234567890");
        var result = payload.GetPayloadString();

        Assert.Equal("sms:+1234567890", result);
    }

    [Fact]
    public void WithMessage_AddsBody()
    {
        var payload = new SmsPayload("+1234567890")
            .WithMessage("Hello World");
        var result = payload.GetPayloadString();

        Assert.Equal("sms:+1234567890?body=Hello%20World", result);
    }

    [Fact]
    public void UrlEncoding_HandlesSpecialCharacters()
    {
        var payload = new SmsPayload("+1234567890")
            .WithMessage("Hello & Goodbye!");
        var result = payload.GetPayloadString();

        Assert.Contains("body=Hello%20%26%20Goodbye%21", result);
    }

    [Fact]
    public void EmptyPhone_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new SmsPayload(""));
    }

    [Fact]
    public void NullPhone_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new SmsPayload(null!));
    }
}
