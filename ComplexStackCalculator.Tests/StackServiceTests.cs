using ComplexStackCalculator.Services;
namespace ComplexStackCalculator.Tests;

// Класс с тестами для StackService
public class StackServiceTests
{
    [Fact]
    public void PushPop_PreservesOrder()
    {
        // Создаём стек для объектов типа Cx
        // IStackService<Cx> — интерфейс
        // StackService<Cx> — реализация
        IStackService<Cx> st = new StackService<Cx>();

        // Добавляем элемент (1 + 0i) в стек
        st.Push(new Cx(1, 0));

        // Добавляем элемент (2 + 0i)
        st.Push(new Cx(2, 0));

        // Добавляем элемент (3 + 0i)
        st.Push(new Cx(3, 0));

        // Проверяем, что в стеке сейчас 3 элемента
        Assert.Equal(3, st.Count);

        // Проверяем, что верхний элемент стека равен (3 + 0i)
        // Peek() возвращает верхний элемент без удаления
        Assert.True(st.Peek().AlmostEquals(new Cx(3, 0)));

        // Извлекаем верхний элемент стека
        var a = st.Pop();

        // Извлекаем следующий элемент
        var b = st.Pop();

        // Проверяем, что первый извлечённый элемент был (3 + 0i)
        Assert.True(a.AlmostEquals(new Cx(3, 0)));

        // Проверяем, что второй извлечённый элемент был (2 + 0i)
        Assert.True(b.AlmostEquals(new Cx(2, 0)));

        // Проверяем, что теперь на вершине остался (1 + 0i)
        Assert.True(st.Peek().AlmostEquals(new Cx(1, 0)));
    }

    // Тест проверки порядка элементов в коллекции ItemsTopFirst
    [Fact]
    public void ItemsTopFirst_ReturnsTopAtZero()
    {
        // Создаём новый стек
        IStackService<Cx> st = new StackService<Cx>();

        // Кладём первый элемент
        st.Push(new Cx(10, 0));

        // Кладём второй элемент
        st.Push(new Cx(20, 0));

        // Получаем список элементов стека
        // ItemsTopFirst возвращает элементы начиная с вершины
        var items = st.ItemsTopFirst;

        // Проверяем, что элемент с индексом 0 — верхушка стека
        Assert.True(items[0].AlmostEquals(new Cx(20, 0)));

        // Проверяем, что следующий элемент — нижний
        Assert.True(items[1].AlmostEquals(new Cx(10, 0)));
    }

    // Тест проверки исключения при попытке извлечения из пустого стека
    [Fact]
    public void Pop_Empty_Throws()
    {
        // Создаём пустой стек
        IStackService<Cx> st = new StackService<Cx>();

        // Проверяем, что Pop() выбрасывает InvalidOperationException
        Assert.Throws<InvalidOperationException>(() => st.Pop());
    }
}