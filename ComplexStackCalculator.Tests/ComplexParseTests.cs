namespace ComplexStackCalculator.Tests;

public class ComplexParseTests
{
    [Theory]
    [InlineData("3+4i", 3, 4)]
    [InlineData("3-4i", 3, -4)]
    [InlineData("4i", 0, 4)]
    [InlineData("-i", 0, -1)]
    [InlineData("5", 5, 0)]
    [InlineData("(3,4)", 3, 4)]
    [InlineData("(3;4)", 3, 4)]
    [InlineData("0.5+2.1i", 0.5, 2.1)]
    public void Parse_ValidFormats_Works(string s, double re, double im)
    {
        var z = Cx.Parse(s);
        Assert.True(z.AlmostEquals(new Cx(re, im), 1e-12));
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("3++4i")]
    [InlineData("i3")]
    [InlineData("3+4")]
    public void TryParse_Invalid_ReturnsFalse(string s)
    {
        bool ok = Cx.TryParse(s, out _);
        Assert.False(ok);
    }
}