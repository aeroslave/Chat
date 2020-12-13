namespace ChatClient.VMs
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Базовый класс вью-моделей.
    /// </summary>
    public class BaseViewModel: INotifyPropertyChanged
    {
        /// <summary>
        /// Событие, генерируемое при изменении свойств.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод генерации события при изменении определенного свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}