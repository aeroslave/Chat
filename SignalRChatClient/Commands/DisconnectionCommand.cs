namespace SignalRChatClient.Commands
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.AspNetCore.SignalR.Client;

    using Newtonsoft.Json;

    using SignalRChatClient.Models;

    /// <summary>
    /// Команда соединения с хабом.
    /// </summary>
    public class DisconnectionCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Выполнить.
        /// </summary>
        public void Execute(object parameter)
        {
            if (!(parameter is MainWindowVM mainWindowVM))
                return;

            mainWindowVM.IsEnabled = false;
            Task.Run(() => LogOut(mainWindowVM));
        }

        /// <summary>
        /// Разлогиниться.
        /// </summary>
        /// <param name="mainWindowVM">Вью-модель главного окна.</param>
        public async Task LogOut(MainWindowVM mainWindowVM)
        {
            const string uri = "https://localhost:44340/api/chat/setactivityfalse";

            var person = new Person
            {
                Name = mainWindowVM.UserName
            };

            var jsonInString = JsonConvert.SerializeObject(person);
            var response = await mainWindowVM.HttpClient.PutAsync(uri,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            await mainWindowVM.HubConnection.InvokeAsync("UpdateUsersActivity", mainWindowVM.UserName, false);

            Application.Current.Dispatcher?.Invoke(() =>
            {
                if (response.IsSuccessStatusCode)
                    mainWindowVM.MessageList.Add($"Пользователь {mainWindowVM.UserName} покинул здание!");

                mainWindowVM.IsEnabled = true;
            });
        }
    }
}