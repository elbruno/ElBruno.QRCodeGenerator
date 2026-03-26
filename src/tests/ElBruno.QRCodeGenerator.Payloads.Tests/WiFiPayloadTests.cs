using Xunit;
using ElBruno.QRCodeGenerator.Payloads;

namespace ElBruno.QRCodeGenerator.Payloads.Tests;

public class WiFiPayloadTests
{
    [Fact]
    public void DefaultWpa_ProducesCorrectPayload()
    {
        var payload = new WiFiPayload("MyNetwork", "MyPassword");
        var result = payload.GetPayloadString();

        Assert.Equal("WIFI:T:WPA;S:MyNetwork;P:MyPassword;H:false;;", result);
    }

    [Theory]
    [InlineData(WiFiAuthType.Open, "nopass")]
    [InlineData(WiFiAuthType.WEP, "WEP")]
    [InlineData(WiFiAuthType.WPA, "WPA")]
    [InlineData(WiFiAuthType.WPA2, "WPA")]
    [InlineData(WiFiAuthType.WPA3, "WPA")]
    public void AllAuthTypes_MapCorrectly(WiFiAuthType authType, string expected)
    {
        var payload = new WiFiPayload("Net", "Pass", authType);
        var result = payload.GetPayloadString();

        Assert.Contains($"T:{expected};", result);
    }

    [Fact]
    public void WithHidden_SetsHiddenTrue()
    {
        var payload = new WiFiPayload("Net", "Pass").WithHidden(true);
        var result = payload.GetPayloadString();

        Assert.Contains("H:true", result);
    }

    [Fact]
    public void SpecialCharacters_AreEscaped()
    {
        var payload = new WiFiPayload("My;Net:work", "Pass\\word\"123");
        var result = payload.GetPayloadString();

        Assert.Contains(@"S:My\;Net\:work", result);
        Assert.Contains(@"P:Pass\\word\""123", result);
    }

    [Fact]
    public void EmptySsid_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new WiFiPayload("", "password"));
    }

    [Fact]
    public void NullSsid_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new WiFiPayload(null!, "password"));
    }

    [Fact]
    public void FluentApi_ReturnsSameInstance()
    {
        var payload = new WiFiPayload("Net", "Pass");
        var result = payload.WithHidden(true).WithMetered(false);

        Assert.Same(payload, result);
    }
}
