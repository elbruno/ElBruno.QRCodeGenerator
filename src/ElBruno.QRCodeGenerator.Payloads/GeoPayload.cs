using System.Globalization;

namespace ElBruno.QRCodeGenerator.Payloads;

/// <summary>
/// Builds a geo: URI payload for geographic coordinates.
/// </summary>
public class GeoPayload : IPayload
{
    private readonly double _latitude;
    private readonly double _longitude;
    private double? _altitude;
    private double? _uncertainty;

    /// <summary>
    /// Creates a new geographic coordinate payload.
    /// </summary>
    /// <param name="latitude">Latitude in degrees (-90 to 90).</param>
    /// <param name="longitude">Longitude in degrees (-180 to 180).</param>
    public GeoPayload(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90.");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180.");

        _latitude = latitude;
        _longitude = longitude;
    }

    /// <summary>Sets the altitude in meters.</summary>
    public GeoPayload WithAltitude(double altitude)
    {
        _altitude = altitude;
        return this;
    }

    /// <summary>Sets the uncertainty (accuracy) in meters.</summary>
    public GeoPayload WithUncertainty(double uncertainty)
    {
        _uncertainty = uncertainty;
        return this;
    }

    /// <inheritdoc />
    public string GetPayloadString()
    {
        var coords = string.Format(
            CultureInfo.InvariantCulture,
            "geo:{0},{1}",
            _latitude,
            _longitude);

        if (_altitude.HasValue)
            coords += string.Format(CultureInfo.InvariantCulture, ",{0}", _altitude.Value);

        if (_uncertainty.HasValue)
            coords += string.Format(CultureInfo.InvariantCulture, "?u={0}", _uncertainty.Value);

        return coords;
    }
}
