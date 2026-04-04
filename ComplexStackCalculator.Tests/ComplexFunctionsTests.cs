namespace ComplexStackCalculator.Tests;

public class ComplexFunctionsTests
{
    [Fact]
    public void Sin_Zero_IsZero()
    {
        var r = Cx.Sin(Cx.Zero);
        Assert.True(r.AlmostEquals(Cx.Zero, 1e-12));
    }

    [Fact]
    public void Cos_Zero_IsOne()
    {
        var r = Cx.Cos(Cx.Zero);
        Assert.True(r.AlmostEquals(Cx.One, 1e-12));
    }

    [Fact]
    public void Tan_Zero_IsZero()
    {
        var r = Cx.Tan(Cx.Zero);
        Assert.True(r.AlmostEquals(Cx.Zero, 1e-12));
    }

    [Fact]
    public void Sqrt_Of4_Is2()
    {
        var r = Cx.Sqrt(new Cx(4, 0));
        Assert.True(r.AlmostEquals(new Cx(2, 0), 1e-12));
    }

    [Fact]
    public void Ln_Of1_Is0()
    {
        var r = Cx.Ln(Cx.One);
        Assert.True(r.AlmostEquals(Cx.Zero, 1e-12));
    }

    [Fact]
    public void Exp_Ln_Z_IsZ_ForNonZero()
    {
        var z = new Cx(1.2, -0.7);
        var r = Cx.Exp(Cx.Ln(z));
        Assert.True(r.AlmostEquals(z, 1e-9));
    }
}