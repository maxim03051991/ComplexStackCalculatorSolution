namespace ComplexStackCalculator.Tests;

public class ComplexArithmeticTests
{
    [Fact]
    public void Add_Works()
    {
        var a = new Cx(1, 2);
        var b = new Cx(3, -4);
        var r = a + b;
        Assert.True(r.AlmostEquals(new Cx(4, -2)));
    }

    [Fact]
    public void Sub_Works()
    {
        var a = new Cx(1, 2);
        var b = new Cx(3, -4);
        var r = a - b;
        Assert.True(r.AlmostEquals(new Cx(-2, 6)));
    }

    [Fact]
    public void Mul_Works()
    {
        var a = new Cx(1, 2);
        var b = new Cx(3, -4);
        var r = a * b; // 11 + 2i
        Assert.True(r.AlmostEquals(new Cx(11, 2)));
    }

    [Fact]
    public void Div_Works()
    {
        var a = new Cx(1, 2);
        var b = new Cx(3, -4);
        var r = a / b; // -0.2 + 0.4i
        Assert.True(r.AlmostEquals(new Cx(-0.2, 0.4), 1e-12));
    }

    [Fact]
    public void Reciprocal_ThrowsOnZero()
    {
        Assert.Throws<DivideByZeroException>(() => Cx.Zero.Reciprocal());
    }

    [Fact]
    public void Div_ThrowsOnZero()
    {
        var a = new Cx(1, 2);
        Assert.Throws<DivideByZeroException>(() => _ = a / Cx.Zero);
    }
}