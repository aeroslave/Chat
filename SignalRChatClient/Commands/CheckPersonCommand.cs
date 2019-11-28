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

        /// <summary>
        /// Выполнить.
        /// </summary>
        public void Execute(object parameter)
        {
            if (!(parameter is MainWindowVM mainWindowVM))
                return;

            mainWindowVM.IsEnabled = false;
            Task.Run(() => CheckPersonAsync(mainWindowVM));
        }

        /// <summary>
        /// Проверить пользователя.
        /// </summary>
        /// <param name="mainWindowVM">Вью-модель главного окна.</param>
        public async Task CheckPersonAsync(MainWindowVM mainWindowVM)
        {
            const string uri = "https://localhost:44340/api/chat";

            var person = new Person
            {
                Name = mainWindowVM.UserName
            };

            var jsonInString = JsonConvert.SerializeObject(person);
            var responsePersonExist = await mainWindowVM.HttpClient.PutAsync(uri,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            var responseActivity = await mainWindowVM.HttpClient.PutAsync(uri + "/isActive",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            if (!responseActivity.IsSuccessStatusCode && responsePersonExist.IsSuccessStatusCode)
                await GetConnection(mainWindowVM);
            else
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add(!responsePersonExist.IsSuccessStatusCode
                        ? "Пользователь с таким именем не зарегистрирован!"
                        : "Пользователь с таким именем уже залогинился!"));


            Application.Current.Dispatcher?.Invoke(() => mainWindowVM.IsEnabled = true);
        }

        /// <summary>
        /// Открывает соединение с хабом.
        /// </summary>
        /// <param name="mainWindowVM">Вью-модель главного окна.</param>
        public async Task GetConnection(MainWindowVM mainWindowVM)
        {
            try
            {
                await mainWindowVM.HubConnection.StartAsync();
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add($"Добро пожаловать {mainWindowVM.UserName}"));
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add(e.Message));
            }
        }
    }
}