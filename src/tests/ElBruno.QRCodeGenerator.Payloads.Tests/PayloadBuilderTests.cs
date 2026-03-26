using Xunit;
using ElBruno.QRCodeGenerator.Payloads;

namespace ElBruno.QRCodeGenerator.Payloads.Tests;

public class PayloadBuilderTests
{
    [Fact]
    public void Wifi_ReturnsWiFiPayload()
    {
        var payload = PayloadBuilder.Wifi("SSID", "pass");
        Assert.IsType<WiFiPayload>(payload);
    }

    [Fact]
    public void VCard_ReturnsVCardPayload()
    {
        var payload = PayloadBuilder.VCard("John Doe");
        Assert.IsType<VCardPayload>(payload);
    }

    [Fact]
    public void Email_ReturnsEmailPayload()
    {
        var payload = PayloadBuilder.Email("test@test.com");
        Assert.IsType<EmailPayload>(payload);
    }

    [Fact]
    public void Sms_ReturnsSmsPayload()
    {
        var payload = PayloadBuilder.Sms("+1234567890");
        Assert.IsType<SmsPayload>(payload);
    }

    [Fact]
    public void Geo_ReturnsGeoPayload()
    {
        var payload = PayloadBuilder.Geo(0, 0);
        Assert.IsType<GeoPayload>(payload);
    }

    [Fact]
    public void Url_ReturnsUrlPayload()
    {
        var payload = PayloadBuilder.Url("example.com");
        Assert.IsType<UrlPayload>(payload);
    }

    [Fact]
    public void AllPayloads_ImplementIPayload()
    {
        IPayload wifi = PayloadBuilder.Wifi("Net", "Pass");
        IPayload vcard = PayloadBuilder.VCard("Name");
        IPayload email = PayloadBuilder.Email("a@b.com");
        IPayload sms = PayloadBuilder.Sms("+1");
        IPayload geo = PayloadBuilder.Geo(0, 0);
        IPayload url = PayloadBuilder.Url("example.com");

        Assert.NotEmpty(wifi.GetPayloadString());
        Assert.NotEmpty(vcard.GetPayloadString());
        Assert.NotEmpty(email.GetPayloadString());
        Assert.NotEmpty(sms.GetPayloadString());
        Assert.NotEmpty(geo.GetPayloadString());
        Assert.NotEmpty(url.GetPayloadString());
    }
}
