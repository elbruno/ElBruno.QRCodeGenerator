using System.Globalization;
using Xunit;
using ElBruno.QRCodeGenerator.Payloads;

namespace ElBruno.QRCodeGenerator.Payloads.Tests;

public class GeoPayloadTests
{
    [Fact]
    public void BasicCoordinates_ProducesGeoUri()
    {
        var payload = new GeoPayload(47.6062, -122.3321);
        var result = payload.GetPayloadString();

        Assert.Equal("geo:47.6062,-122.3321", result);
    }

    [Fact]
    public void WithAltitude_IncludesAltitude()
    {
        var payload = new GeoPayload(47.6062, -122.3321)
            .WithAltitude(100.5);
        var result = payload.GetPayloadString();

        Assert.Equal("geo:47.6062,-122.3321,100.5", result);
    }

    [Fact]
    public void WithUncertainty_IncludesUncertainty()
    {
        var payload = new GeoPayload(47.6062, -122.3321)
            .WithUncertainty(10);
        var result = payload.GetPayloadString();

        Assert.Equal("geo:47.6062,-122.3321?u=10", result);
    }

    [Fact]
    public void WithAltitudeAndUncertainty_IncludesBoth()
    {
        var payload = new GeoPayload(47.6062, -122.3321)
            .WithAltitude(200)
            .WithUncertainty(15.5);
        var result = payload.GetPayloadString();

        Assert.Equal("geo:47.6062,-122.3321,200?u=15.5", result);
    }

    [Fact]
    public void BoundaryValues_AreAccepted()
    {
        // Min/max boundaries
        var payload1 = new GeoPayload(-90, -180);
        Assert.Equal("geo:-90,-180", payload1.GetPayloadString());

        var payload2 = new GeoPayload(90, 180);
        Assert.Equal("geo:90,180", payload2.GetPayloadString());

        var payload3 = new GeoPayload(0, 0);
        Assert.Equal("geo:0,0", payload3.GetPayloadString());
    }

    [Fact]
    public void LatitudeOutOfRange_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GeoPayload(91, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new GeoPayload(-91, 0));
    }

    [Fact]
    public void LongitudeOutOfRange_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GeoPayload(0, 181));
        Assert.Throws<ArgumentOutOfRangeException>(() => new GeoPayload(0, -181));
    }

    [Fact]
    public void InvariantCulture_UsedForFormatting()
    {
        // Ensure decimal separator is always '.' regardless of thread culture
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE"); // Uses ',' as decimal separator
            var payload = new GeoPayload(48.8566, 2.3522)
                .WithAltitude(35.5)
                .WithUncertainty(12.3);
            var result = payload.GetPayloadString();

            Assert.Equal("geo:48.8566,2.3522,35.5?u=12.3", result);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }
}
