namespace SignalRChatClient.VMs
{
    using System;

    using SignalRChatClient.Commands;

    /// <summary>
    /// Вью-модель окна добавления нового пользователя.
    /// </summary>
    public class AddPersonWindowVM : BaseViewModel
    {
        private bool _isEnabled;

        /// <summary>
        /// Вью-модель окна добавления нового пользователя.
        /// </summary>
        public AddPersonWindowVM()
        {
            PostToWebApiCommand = new PostToWebApiCommand();
        }

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
    }
}