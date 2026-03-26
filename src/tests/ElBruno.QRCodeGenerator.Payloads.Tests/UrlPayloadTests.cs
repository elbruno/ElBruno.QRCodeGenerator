using Xunit;
using ElBruno.QRCodeGenerator.Payloads;

namespace ElBruno.QRCodeGenerator.Payloads.Tests;

public class UrlPayloadTests
{
    [Fact]
    public void BasicUrl_PrependsHttps()
    {
        var payload = new UrlPayload("example.com");
        var result = payload.GetPayloadString();

        Assert.Equal("https://example.com", result);
    }

    [Fact]
    public void WithScheme_PrependsCustomScheme()
    {
        var payload = new UrlPayload("example.com")
            .WithScheme("http");
        var result = payload.GetPayloadString();

        Assert.Equal("http://example.com", result);
    }

    [Fact]
    public void AlreadyHasScheme_ReturnsAsIs()
    {
        var payload = new UrlPayload("https://example.com");
        var result = payload.GetPayloadString();

        Assert.Equal("https://example.com", result);
    }

    [Fact]
    public void AlreadyHasCustomScheme_ReturnsAsIs()
    {
        var payload = new UrlPayload("http://example.com")
            .WithScheme("http");
        var result = payload.GetPayloadString();

        Assert.Equal("http://example.com", result);
    }

    [Fact]
    public void EmptyUrl_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new UrlPayload(""));
    }

    [Fact]
    public void NullUrl_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new UrlPayload(null!));
    }
}
