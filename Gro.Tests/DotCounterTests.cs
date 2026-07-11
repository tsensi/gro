using Gro.Rendering;

namespace Gro.Tests;

public class DotCounterTests
{
    [Fact]
    public void Decompose_Zero_ReturnsEmpty()
    {
        var dots = DotCounter.Decompose(0);
        Assert.Empty(dots);
    }

    [Fact]
    public void Decompose_One_ReturnsSingleSmallDot()
    {
        var dots = DotCounter.Decompose(1);
        Assert.Single(dots);
        Assert.Equal(DotShape.SmallDot, dots[0]);
    }

    [Fact]
    public void Decompose_Ten_ReturnsSingleCircle()
    {
        var dots = DotCounter.Decompose(10);
        Assert.Single(dots);
        Assert.Equal(DotShape.Circle, dots[0]);
    }

    [Fact]
    public void Decompose_23_ReturnsCorrectMix()
    {
        var dots = DotCounter.Decompose(23);
        Assert.Equal(5, dots.Count);
        Assert.Equal(2, dots.Count(d => d == DotShape.Circle));
        Assert.Equal(3, dots.Count(d => d == DotShape.SmallDot));
    }

    [Fact]
    public void Decompose_999_CapsAtGridSize()
    {
        var dots = DotCounter.Decompose(999);
        Assert.Equal(DotCounter.GridSize, dots.Count);
        Assert.Equal(9, dots.Count(d => d == DotShape.Square));
    }

    [Fact]
    public void Decompose_1000_ReturnsSingleDiamond()
    {
        var dots = DotCounter.Decompose(1000);
        Assert.Single(dots);
        Assert.Equal(DotShape.Diamond, dots[0]);
    }

    [Fact]
    public void Decompose_LargeValue_UsesHighMagnitudeShapes()
    {
        var dots = DotCounter.Decompose(5_000_000_000.0);
        Assert.Equal(5, dots.Count);
        Assert.All(dots, d => Assert.Equal(DotShape.Ring, d));
    }

    [Fact]
    public void Decompose_MaxMagnitude_UsesOctagon()
    {
        var dots = DotCounter.Decompose(30_000_000_000.0);
        Assert.Equal(3, dots.Count(d => d == DotShape.Octagon));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 1)]
    [InlineData(100, 1)]
    [InlineData(1234, 9)]
    [InlineData(99999, 9)]
    public void Decompose_TotalDotCount(double value, int expectedCount)
    {
        var dots = DotCounter.Decompose(value);
        Assert.Equal(expectedCount, dots.Count);
    }

    [Fact]
    public void Decompose_NeverExceedsGridSize()
    {
        var dots = DotCounter.Decompose(99_999_999_999.0);
        Assert.True(dots.Count <= DotCounter.GridSize);
    }

    [Fact]
    public void Decompose_OrdersHighMagnitudeFirst()
    {
        var dots = DotCounter.Decompose(1234);
        Assert.Equal(DotShape.Diamond, dots[0]);
        Assert.Equal(DotShape.Square, dots[1]);
        Assert.Equal(DotShape.Square, dots[2]);
    }

    [Fact]
    public void Decompose_FractionalValue_TruncatesToInteger()
    {
        var dots = DotCounter.Decompose(2.9);
        Assert.Equal(2, dots.Count);
        Assert.All(dots, d => Assert.Equal(DotShape.SmallDot, d));
    }

    [Fact]
    public void Decompose_NegativeValue_ReturnsEmpty()
    {
        var dots = DotCounter.Decompose(-5);
        Assert.Empty(dots);
    }
}
