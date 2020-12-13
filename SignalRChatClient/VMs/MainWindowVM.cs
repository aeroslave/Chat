﻿namespace SignalRChatClient.VMs
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR.Client;

    using Ninject;

    using SignalRChatClient.Commands;
    using SignalRChatClient.Interfaces;
    using SignalRChatClient.Utilites;

    /// <summary>
    /// Вью-модель главного окна.
    /// </summary>
    public class MainWindowVM : BaseViewModel
    {
        /// <summary>
        /// Поле для <see cref="ActiveUsers"/>
        /// </summary>
        private ObservableCollection<string> _activeUsers;

        /// <summary>
        /// Поле для <see cref="IsLogin"/>
        /// </summary>
        private bool _isLogin;

        /// <summary>
        /// Поле для <see cref="NeedGetConnection"/>
        /// </summary>
        private bool _needGetConnection;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainWindowVM()
        {
            MessageList = new ObservableCollection<string>();
            NeedGetConnection = true;
            ActiveUsers = new ObservableCollection<string>();

            ConnectionUtils.InitHubConnection(this);

            GetConnectionCommand = new GetConnectionCommand();
            DisconnectionCommand = new DisconnectionCommand();
            SendMessageCommand = new SendMessageCommand();
            AddPersonCommand = new AddPersonCommand();
            CheckPersonCommand = new CheckPersonCommand();

            GetPersons();
        }

        /// <summary>
        /// Список участников.
        /// </summary>
        public ObservableCollection<string> ActiveUsers
        {
            get => _activeUsers;
            set
            {
                _activeUsers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Команда отправки сообщения из WebAPI
        /// </summary>
        public AddPersonCommand AddPersonCommand { get; set; }

        /// <summary>
        /// Проверить пользователя.
        /// </summary>
        public CheckPersonCommand CheckPersonCommand { get; set; }

        /// <summary>
        /// Команда для подключения.
        /// </summary>
        public DisconnectionCommand DisconnectionCommand { get; set; }

        /// <summary>
        /// Команда получения соединения.
        /// </summary>
        public GetConnectionCommand GetConnectionCommand { get; set; }

        /// <summary>
        /// Соединение с хабом.
        /// </summary>
        public HubConnection HubConnection { get; set; }

        /// <summary>
        /// Флаг того, что пользователь залогинился.
        /// </summary>
        public bool IsLogin
        {
            get => _isLogin;
            set
            {
                _isLogin = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Сообщение для отправки.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Список сообщений.
        /// </summary>
        public ObservableCollection<string> MessageList { get; set; }

        /// <summary>
        /// Флаг необходимости получения подключения.
        /// </summary>
        public bool NeedGetConnection
        {
            get => _needGetConnection;
            set
            {
                _needGetConnection = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Команда для отправки сообщения.
        /// </summary>
        public SendMessageCommand SendMessageCommand { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Получить список пользователей.
        /// </summary>
        private void GetPersons()
        {
            Task.Run(GetPersonsAsync);
        }

        /// <summary>
        /// Получить список пользователей асинхронно.
        /// </summary>
        private async Task GetPersonsAsync()
        {
            var connectionService = NinjectKernel.Instance.Get<IPersonService>();
            var persons = await connectionService.GetPersonsAsync();

            ActiveUsers =
                new ObservableCollection<string>(persons.Where(person => person.IsActive).Select(it => it.Name));
        }
    }
}