using ComplexStackCalculator.Operations;

namespace ComplexStackCalculator.Tests;

public class OperationRegistryTests
{
    [Fact]
    public void Registry_HasBasicOps()
    {
        var reg = new OperationRegistry();

        Assert.True(reg.TryGet("+", out var add));
        Assert.Equal(2, add.Arity);

        Assert.True(reg.TryGet("sin", out var sin));
        Assert.Equal(1, sin.Arity);

        Assert.True(reg.TryGet("inv", out var inv));
        Assert.Equal(1, inv.Arity);
    }

    [Fact]
    public void Add_ExecutesCorrectly()
    {
        var reg = new OperationRegistry();
        Assert.True(reg.TryGet("+", out var add));

        var r = add.Execute(new Cx(1, 2), new Cx(3, -4));
        Assert.True(r.AlmostEquals(new Cx(4, -2), 1e-12));
    }

    [Fact]
    public void Inv_ExecutesCorrectly()
    {
        var reg = new OperationRegistry();
        Assert.True(reg.TryGet("inv", out var inv));

        var r = inv.Execute(new Cx(2, 0));
        Assert.True(r.AlmostEquals(new Cx(0.5, 0), 1e-12));
    }
}