namespace SignalRChatClient
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows;

    using Microsoft.AspNetCore.SignalR.Client;

    using Newtonsoft.Json;

    using SignalRChatClient.Commands;
    using SignalRChatClient.Models;
    using SignalRChatClient.Utilites;

    public class MainWindowVM : INotifyPropertyChanged
    {
        private ObservableCollection<string> _activeUsers;
        private bool _isEnabled;
        private bool _needGetConnection;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainWindowVM()
        {
            MessageList = new ObservableCollection<string>();
            Address = ConnectionUtils.GetAddressConnection();
            NeedGetConnection = true;
            //HubConnection = new HubConnectionBuilder().WithUrl(Address + "/ChatHub").Build();

            ConnectionUtils.InitHubConnection(this);

            GetConnectionCommand = new GetConnectionCommand();
            DisconnectionCommand = new DisconnectionCommand();
            SendMessageCommand = new SendMessageCommand();
            ShowAddPersonWindowCommand = new ShowAddPersonWindowCommand();
            CheckPersonCommand = new CheckPersonCommand();

            //InitHubConnection();

            HttpClient = new HttpClient();

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
        /// Адрес.
        /// </summary>
        private string Address { get; }

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
        /// Клиент Http.
        /// </summary>
        public HttpClient HttpClient { get; }

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
        /// Адрес веб апи.
        /// </summary>
        public string WebApiAddress => Address + "/api/chat";

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
        /// Получить адрес подключения.
        /// </summary>
        /// <returns>Адрес подключения.</returns>
        private static string GetAddressConnection()
        {
            var path = Environment.CurrentDirectory;
            var configTextFromFile = File.ReadAllText(path + "\\address_config.json");
            var connectionConfig = JsonConvert.DeserializeObject<ConnectionConfig>(configTextFromFile);

            return connectionConfig.Address;
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
            var uri = $"{Address}/api/chat";

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
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    MessageList.Add("Соединение потеряно");
                    IsEnabled = false;
                });

                return Task.CompletedTask;
            };

            Task.Run(async () =>
            {
                try
                {
                    await HubConnection.StartAsync();
                    Application.Current.Dispatcher?.Invoke(() => IsEnabled = true);
                }
                catch (Exception)
                {
                    Application.Current.Dispatcher?.Invoke(() => MessageList.Add("Не удалось соединиться с сервером"));
                }
            });
        }
    }
}