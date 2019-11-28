namespace SignalRChatClient
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows;

    using Microsoft.AspNetCore.SignalR.Client;

    using SignalRChatClient.Commands;

    public class MainWindowVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainWindowVM()
        {
            HubConnection = new HubConnectionBuilder().WithUrl("https://localhost:44340/ChatHub").Build();
            DisconnectionCommand = new DisconnectionCommand();
            SendMessageCommand = new SendMessageCommand();
            GetFromWebApiCommand = new GetFromWebApiCommand();
            ShowAddPersonWindowCommand = new ShowAddPersonWindowCommand();
            CheckPersonCommand = new CheckPersonCommand();

            MessageList = new ObservableCollection<string>();

            InitHubConnection();

            HttpClient = new HttpClient();
            IsEnabled = true;
        }

        public CheckPersonCommand CheckPersonCommand { get; set; }

        /// <summary>
        /// Команда для подключения.
        /// </summary>
        public DisconnectionCommand DisconnectionCommand { get; set; }

        /// <summary>
        /// Команда получение сообщения из WebAPI
        /// </summary>
        public GetFromWebApiCommand GetFromWebApiCommand { get; set; }

        /// <summary>
        /// Клиент Http.
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Соединение с хабом.
        /// </summary>
        public HubConnection HubConnection { get; }

        public bool IsEnabled { get; set; }

        /// <summary>
        /// Сообщение для отправки.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Список сообщений.
        /// </summary>
        public ObservableCollection<string> MessageList { get; set; }

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

        /// <summary>
        /// Инициализация соединения с хабом.
        /// </summary>
        private void InitHubConnection()
        {
            HubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    var newMessage = $"{user} отправил сообщение: {message}";
                    MessageList.Add(newMessage);
                });
            });

            HubConnection.Closed += error =>
            {
                Application.Current.Dispatcher?.Invoke(() => { MessageList.Add("Соединение потеряно"); });

                return Task.CompletedTask;
            };
        }
    }
}