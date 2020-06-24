namespace SignalRChatClient
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using Microsoft.AspNetCore.SignalR.Client;

    using Ninject;

    using SignalRChatClient.Commands;
    using SignalRChatClient.Interfaces;
    using SignalRChatClient.Services;
    using SignalRChatClient.Utilites;
    using SignalRChatClient.VMs;

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
        /// Поле для <see cref="IsEnabled"/>
        /// </summary>
        private bool _isEnabled;

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

            ConnectionUtils.InitHubConnection(this);

            GetConnectionCommand = new GetConnectionCommand();
            DisconnectionCommand = new DisconnectionCommand();
            SendMessageCommand = new SendMessageCommand();
            ShowAddPersonWindowCommand = new ShowAddPersonWindowCommand();
            CheckPersonCommand = new CheckPersonCommand();

            var ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IPersonService>().To<PersonService>().InSingletonScope();

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
        /// Флаг активности элементов управления.
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
        /// Команда отправки сообщения из WebAPI
        /// </summary>
        public ShowAddPersonWindowCommand ShowAddPersonWindowCommand { get; set; }

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
            var ninjectKernel = new StandardKernel();
            var connectionService = ninjectKernel.Get<IPersonService>();

            var persons = await connectionService.GetPersonsAsync();

            ActiveUsers =
                new ObservableCollection<string>(persons.Where(person => person.IsActive).Select(it => it.Name));

            Application.Current.Dispatcher?.Invoke(() => IsEnabled = true);
        }
    }
}