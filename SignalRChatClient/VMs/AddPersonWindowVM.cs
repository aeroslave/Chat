namespace SignalRChatClient.VMs
{
    using System;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Runtime.CompilerServices;

    using SignalRChatClient.Commands;

    /// <summary>
    /// Вью-модель окна добавления нового пользователя.
    /// </summary>
    public class AddPersonWindowVM : INotifyPropertyChanged
    {
        private bool _isEnabled;

        /// <summary>
        /// Адрес вебАпи.
        /// </summary>
        public string WebApiAddress { get;}

        /// <summary>
        /// Вью-модель окна добавления нового пользователя.
        /// </summary>
        public AddPersonWindowVM(HttpClient httpClient, string webApiAddress)
        {
            WebApiAddress = webApiAddress;
            HttpClient = httpClient;
            PostToWebApiCommand = new PostToWebApiCommand();
        }

        /// <summary>
        /// Клиент Http.
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Флаг активности элементов окна.
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// ФИО пользователя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Команда отправки сообщения из WebAPI
        /// </summary>
        public PostToWebApiCommand PostToWebApiCommand { get; set; }

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