using ComplexStackCalculator.Services;

namespace ComplexStackCalculator.Tests;

public class StackServiceTests
{
    [Fact]
    public void PushPop_PreservesOrder()
    {
        IStackService<Cx> st = new StackService<Cx>();
        st.Push(new Cx(1, 0));
        st.Push(new Cx(2, 0));
        st.Push(new Cx(3, 0));

        Assert.Equal(3, st.Count);
        Assert.True(st.Peek().AlmostEquals(new Cx(3, 0)));

        var a = st.Pop();
        var b = st.Pop();

        Assert.True(a.AlmostEquals(new Cx(3, 0)));
        Assert.True(b.AlmostEquals(new Cx(2, 0)));
        Assert.True(st.Peek().AlmostEquals(new Cx(1, 0)));
    }

    [Fact]
    public void ItemsTopFirst_ReturnsTopAtZero()
    {
        IStackService<Cx> st = new StackService<Cx>();
        st.Push(new Cx(10, 0));
        st.Push(new Cx(20, 0));

        var items = st.ItemsTopFirst;
        Assert.True(items[0].AlmostEquals(new Cx(20, 0)));
        Assert.True(items[1].AlmostEquals(new Cx(10, 0)));
    }

    [Fact]
    public void Pop_Empty_Throws()
    {
        IStackService<Cx> st = new StackService<Cx>();
        Assert.Throws<InvalidOperationException>(() => st.Pop());
    }
}