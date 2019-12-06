namespace SignalRChatClient.Utilites
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;

    using Microsoft.AspNetCore.SignalR.Client;

    using Newtonsoft.Json;

    using SignalRChatClient.Models;

    /// <summary>
    /// Инструмент для работы с соединением.
    /// </summary>
    public static class ConnectionUtils
    {
        /// <summary>
        /// Получить адрес подключения.
        /// </summary>
        /// <returns>Адрес подключения.</returns>
        public static string GetAddressConnection()
        {
            var path = Environment.CurrentDirectory;
            var configTextFromFile = File.ReadAllText(path + "\\address_config.json");
            var connectionConfig = JsonConvert.DeserializeObject<ConnectionConfig>(configTextFromFile);

            return connectionConfig.Address;
        }

        /// <summary>
        /// Инициализация соединения с хабом.
        /// </summary>
        public static void InitHubConnection(MainWindowVM mainWindowVM)
        {
            var address = GetAddressConnection();
            mainWindowVM.HubConnection = new HubConnectionBuilder().WithUrl(address + "/ChatHub").Build();

            RegisterHandler(mainWindowVM);

            mainWindowVM.HubConnection.Closed += error =>
            {
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    mainWindowVM.MessageList.Add("Соединение потеряно");
                    mainWindowVM.NeedGetConnection = true;
                    mainWindowVM.IsEnabled = false;
                });

                return Task.CompletedTask;
            };

            Task.Run(async () =>
            {
                try
                {
                    Application.Current.Dispatcher?.Invoke(() => mainWindowVM.NeedGetConnection = false);

                    await mainWindowVM.HubConnection.StartAsync();
                    Application.Current.Dispatcher?.Invoke(() =>
                    {
                        mainWindowVM.IsEnabled = true;
                        mainWindowVM.NeedGetConnection = false;
                        mainWindowVM.MessageList.Add("Соединение установлено");
                    });
                }
                catch (Exception)
                {
                    Application.Current.Dispatcher?.Invoke(() =>
                    {
                        mainWindowVM.MessageList.Add("Не удалось соединиться с сервером");
                        mainWindowVM.NeedGetConnection = true;
                    });
                    await mainWindowVM.HubConnection.DisposeAsync();
                }
            });
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

            mainWindowVM.HubConnection.On<string, bool>("UpdateUsers", (user, isActive) =>
            {
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    if (isActive)
                        mainWindowVM.ActiveUsers.Add(user);
                    else
                        mainWindowVM.ActiveUsers.Remove(user);
                });
            });
        }
    }
}