namespace ComplexStackCalculator.Tests;

public class ComplexPowersTests
{
    [Fact]
    public void Square_Works()
    {
        var z = new Cx(2, 3);
        var r = z.Square(); // -5 + 12i
        Assert.True(r.AlmostEquals(new Cx(-5, 12), 1e-12));
    }

    [Fact]
    public void Cube_Works()
    {
        var z = new Cx(1, 1);
        var r = z.Cube(); // -2 + 2i
        Assert.True(r.AlmostEquals(new Cx(-2, 2), 1e-12));
    }

    [Fact]
    public void Pow_Int_Works()
    {
        var z = new Cx(2, 0);
        var r = z.Pow(10);
        Assert.True(r.AlmostEquals(new Cx(1024, 0), 1e-12));
    }

    [Fact]
    public void Pow_Double_Works()
    {
        var z = new Cx(9, 0);
        var r = z.Pow(0.5);
        Assert.True(r.AlmostEquals(new Cx(3, 0), 1e-9));
    }

    [Fact]
    public void Pow_Complex_SmokeTest()
    {
        var z = new Cx(1, 1);
        var w = new Cx(2, -1);
        var r = z.Pow(w);

        var expected = Cx.Exp(w * Cx.Ln(z));
        Assert.True(r.AlmostEquals(expected, 1e-9));
    }
}