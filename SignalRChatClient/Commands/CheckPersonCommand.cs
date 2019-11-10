namespace SignalRChatClient.Commands
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using Newtonsoft.Json;

    using SignalRChatClient.Models;

    /// <summary>
    /// Команда проверки наличия пользователя.
    /// </summary>
    public class CheckPersonCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (!(parameter is MainWindowVM mainWindowVM))
                return;

            mainWindowVM.IsEnabled = false;
            Task.Run(() => CheckPersonAsync(mainWindowVM));
        }

        public async Task CheckPersonAsync(MainWindowVM mainWindowVM)
        {
            const string uri = "https://localhost:44340/api/chat";

            var person = new Person
            {
                Name = mainWindowVM.UserName,
                Password = "pass"
            };

            var jsonInString = JsonConvert.SerializeObject(person);
            var response = await mainWindowVM.HttpClient.PutAsync(uri,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
                await GetConnection(mainWindowVM);

            Application.Current.Dispatcher?.Invoke(() =>
            {
                mainWindowVM.MessageList.Add(response.IsSuccessStatusCode
                    ? $"Добро пожаловать {mainWindowVM.UserName}"
                    : "Пользователь с таким именем не зарегистрирован!");

                mainWindowVM.IsEnabled = true;
            });
        }

        public async Task GetConnection(MainWindowVM mainWindowVM)
        {
            try
            {
                await mainWindowVM.HubConnection.StartAsync();
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add("Connection stated!"));
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add(e.Message));
            }
        }
    }
}