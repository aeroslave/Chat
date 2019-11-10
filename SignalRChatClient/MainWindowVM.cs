namespace SignalRChatClient
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
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
            ConnectionCommand = new ConnectionCommand();
            SendMessageCommand = new SendMessageCommand();
            GetFromWebApiCommand = new GetFromWebApiCommand();
            PostToWebApiCommand = new PostToWebApiCommand();
            CheckPersonCommand = new CheckPersonCommand();

            MessageList = new ObservableCollection<string>();

            HubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user} отправил сообщение: {message}";
                    MessageList.Add(newMessage);
                });
            });

            HttpClient = new HttpClient();
            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }
        public CheckPersonCommand CheckPersonCommand { get; set; }

        /// <summary>
        /// Команда для подключения.
        /// </summary>
        public ConnectionCommand ConnectionCommand { get; set; }

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

        /// <summary>
        /// Сообщение для отправки.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Список сообщений.
        /// </summary>
        public ObservableCollection<string> MessageList { get; set; }

        /// <summary>
        /// Команда отправки сообщения из WebAPI
        /// </summary>
        public PostToWebApiCommand PostToWebApiCommand { get; set; }

        /// <summary>
        /// Команда для отправки сообщения.
        /// </summary>
        public SendMessageCommand SendMessageCommand { get; set; }

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
    }
}