using System.ComponentModel;                // Интерфейс INotifyPropertyChanged
using System.Runtime.CompilerServices;      // Атрибут CallerMemberName

namespace ComplexStackCalculator.Infrastructure
{
    // Базовый класс для всех ViewModel в MVVM
    // Реализует механизм уведомления UI об изменениях свойств
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // Событие, на которое подписывается интерфейс
        // Вызывается при изменении любого свойства
        public event PropertyChangedEventHandler? PropertyChanged;

        // Метод для вызова события PropertyChanged
        // CallerMemberName автоматически передаст имя свойства,
        // которое вызвало этот метод
        protected void OnPropertyChanged([CallerMemberName] string? prop = null)
        {
            // Если есть подписчики — уведомляем их
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        // Универсальный метод установки значения свойства
        // Позволяет избежать повторения кода в каждом свойстве
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? prop = null)
        {
            // Если значение не изменилось — ничего не делаем
            if (Equals(field, value))
                return false;

            // Обновляем поле
            field = value;

            // Уведомляем UI об изменении свойства
            OnPropertyChanged(prop);

            return true; // Значение изменилось
        }
    }
}