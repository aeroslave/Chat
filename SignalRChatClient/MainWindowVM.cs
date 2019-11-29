namespace SignalRChatClient
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows;

    using Microsoft.AspNetCore.SignalR.Client;

    using Newtonsoft.Json;

    using SignalRChatClient.Commands;
    using SignalRChatClient.Models;

    public class MainWindowVM : INotifyPropertyChanged
    {
        private ObservableCollection<string> _activeUsers;

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

            GetPersons();
            IsEnabled = true;
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
        /// Флаг активности элементов управления.
        /// </summary>
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
            const string uri = "https://localhost:44340/api/chat";

            var response = await HttpClient.GetAsync(uri);
            var responseResult = await response.Content.ReadAsStringAsync();

            var persons = JsonConvert.DeserializeObject<List<Person>>(responseResult);
            ActiveUsers =
                new ObservableCollection<string>(persons.Where(person => person.IsActive).Select(it => it.Name));

            Application.Current.Dispatcher?.Invoke(() => IsEnabled = true);
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

            HubConnection.On<string, bool>("UpdateUsers", (user, isActive) =>
            {
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    if (isActive)
                        ActiveUsers.Add(user);
                    else
                        ActiveUsers.Remove(user);
                });
            });

            HubConnection.Closed += error =>
            {
                Application.Current.Dispatcher?.Invoke(() => { MessageList.Add("Соединение потеряно"); });

                return Task.CompletedTask;
            };

            Task.Run(async () => await HubConnection.StartAsync());
        }
    }
}