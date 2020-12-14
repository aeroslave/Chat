namespace ChatClient.Utilites
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;

    using ChatClient.Models;
    using ChatClient.VMs;

    using Microsoft.AspNetCore.SignalR.Client;

    using Newtonsoft.Json;

    /// <summary>
    /// Инструмент для работы с соединением.
    /// </summary>
    public static class ConnectionUtils
    {
        /// <summary>
        /// Получить адрес подключения.
        /// </summary>
        /// <returns>Адрес подключения.</returns>
        public static AddressConfig GetAddressConnection()
        {
            var path = Environment.CurrentDirectory;
            var configTextFromFile = File.ReadAllText(path + "\\address_config.json");

            return JsonConvert.DeserializeObject<AddressConfig>(configTextFromFile);
        }

        /// <summary>
        /// Инициализация соединения с хабом.
        /// </summary>
        public static async Task InitHubConnection(MainWindowVM mainWindowVM, string token)
        {
            var address = GetAddressConnection();
            mainWindowVM.HubConnection = new HubConnectionBuilder()
                .WithUrl(address.Address + "/chatHub",
                    o => o.AccessTokenProvider = () => Task.FromResult(token)).Build();

            RegisterHandler(mainWindowVM);

            mainWindowVM.HubConnection.Closed += error =>
            {
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    mainWindowVM.MessageList.Add("Соединение потеряно");
                    mainWindowVM.IsLogin = false;
                });

                return Task.CompletedTask;
            };

            await TryGetConnectionAsync(mainWindowVM);
        }

        /// <summary>
        /// Обновить активность пользователя.
        /// </summary>
        /// <param name="mainWindowVM">Вью-модель главного окна.</param>
        public static async Task UpdateUserActivity(MainWindowVM mainWindowVM)
        {
            try
            {
                await mainWindowVM.HubConnection.InvokeAsync("UpdateUsersActivity", mainWindowVM.UserName, true);
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    mainWindowVM.IsLogin = true;
                    mainWindowVM.MessageList.Add($"Добро пожаловать {mainWindowVM.UserName}");
                });
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add(e.Message));
            }
        }

        /// <summary>
        /// Зарегистрировать обработчики методов хаба.
        /// </summary>
        /// <param name="mainWindowVM">Вью-модель главного окна.</param>
        private static void RegisterHandler(MainWindowVM mainWindowVM)
        {
            mainWindowVM.HubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    var newMessage = $"{user} отправил сообщение: {message}";
                    mainWindowVM.MessageList.Add(newMessage);
                });
            });

            mainWindowVM.HubConnection.On<string, bool>("UpdateUsers",
                (user, isActive) => Application.Current.Dispatcher?.Invoke(mainWindowVM.GetPersons));
        }

        /// <summary>
        /// Попытаться соединиться с хабом.
        /// </summary>
        /// <param name="mainWindowVM">Вью-модель главного окна.</param>
        private static async Task TryGetConnectionAsync(MainWindowVM mainWindowVM)
        {
            try
            {
                await mainWindowVM.HubConnection.StartAsync();
            }
            catch (Exception exception)
            {
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add($"Не удалось соединиться с сервером: {exception.Message}"));

                await mainWindowVM.HubConnection.DisposeAsync();
            }
        }
    }
}