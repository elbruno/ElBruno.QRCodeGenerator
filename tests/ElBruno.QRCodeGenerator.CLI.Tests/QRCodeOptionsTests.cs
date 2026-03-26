using Xunit;

namespace ElBruno.QRCodeGenerator.CLI.Tests;

public class QRCodeOptionsTests
{
    [Fact]
    public void DefaultOptions_HasCorrectDefaults()
    {
        // Arrange & Act
        var options = new QRCodeOptions();

        // Assert
        Assert.Equal(ErrorCorrectionLevel.M, options.ErrorCorrection);
        Assert.False(options.InvertColors);
        Assert.Equal(1, options.QuietZoneSize);
    }

    [Theory]
    [InlineData(ErrorCorrectionLevel.L)]
    [InlineData(ErrorCorrectionLevel.M)]
    [InlineData(ErrorCorrectionLevel.Q)]
    [InlineData(ErrorCorrectionLevel.H)]
    public void ErrorCorrectionLevel_AllValuesValid(ErrorCorrectionLevel level)
    {
        // Arrange
        var options = new QRCodeOptions();

        // Act
        options.ErrorCorrection = level;

        // Assert
        Assert.Equal(level, options.ErrorCorrection);
    }

    [Fact]
    public void QuietZoneSize_CanBeSetToZero()
    {
        // Arrange
        var options = new QRCodeOptions();

        // Act
        options.QuietZoneSize = 0;

        // Assert
        Assert.Equal(0, options.QuietZoneSize);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public void QuietZoneSize_CanBeSetToLargeValue(int size)
    {
        // Arrange
        var options = new QRCodeOptions();

        // Act
        options.QuietZoneSize = size;

        // Assert
        Assert.Equal(size, options.QuietZoneSize);
    }

    [Fact]
    public void InvertColors_CanBeToggled()
    {
        // Arrange
        var options = new QRCodeOptions();

        // Act & Assert (default false)
        Assert.False(options.InvertColors);

        // Act & Assert (set to true)
        options.InvertColors = true;
        Assert.True(options.InvertColors);

        // Act & Assert (set back to false)
        options.InvertColors = false;
        Assert.False(options.InvertColors);
    }

    [Fact]
    public void Options_CanBeInitializedWithObjectInitializer()
    {
        // Arrange & Act
        var options = new QRCodeOptions
        {
            ErrorCorrection = ErrorCorrectionLevel.H,
            InvertColors = true,
            QuietZoneSize = 2
        };

        // Assert
        Assert.Equal(ErrorCorrectionLevel.H, options.ErrorCorrection);
        Assert.True(options.InvertColors);
        Assert.Equal(2, options.QuietZoneSize);
    }
}
