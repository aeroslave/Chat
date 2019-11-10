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
    /// Команда отправки сообщения.
    /// </summary>
    public class PostToWebApiCommand : ICommand
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
            Task.Run(() => PostMessageAsync(mainWindowVM));
        }

        /// <summary>
        /// Отправить запрос.
        /// </summary>
        /// <param name="mainWindowVM"> Вью-модель окна.</param>
        private static async Task PostMessageAsync(MainWindowVM mainWindowVM)
        {
            var person = new Person
            {
                Name = mainWindowVM.UserName,
                Password = "pass"
            };

            var jsonInString = JsonConvert.SerializeObject(person);

            const string uri = "https://localhost:44340/api/chat";
            var isPersonExsist = await mainWindowVM.HttpClient.PutAsync(uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            if (isPersonExsist.IsSuccessStatusCode)
            { Application.Current.Dispatcher.Invoke(() =>
                {
                    mainWindowVM.MessageList.Add($"Пользователь с именем {person.Name} уже существует");
                    mainWindowVM.IsEnabled = true;
                });

                return;
            }

            var response = await mainWindowVM.HttpClient.PostAsync(uri,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (response.IsSuccessStatusCode)
                    mainWindowVM.MessageList.Add($"Пользователь {person.Name} добавлен");
                mainWindowVM.IsEnabled = true;
            });
        }
    }
}