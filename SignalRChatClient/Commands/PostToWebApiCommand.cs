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
    using SignalRChatClient.VMs;

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
            if (!(parameter is AddPersonWindowVM addPersonWindowVM))
                return;
            addPersonWindowVM.IsEnabled = false;
            Task.Run(() => PostMessageAsync(addPersonWindowVM));
        }

        /// <summary>
        /// Отправить запрос.
        /// </summary>
        /// <param name="addPersonWindowVM"> Вью-модель окна.</param>
        private static async Task PostMessageAsync(AddPersonWindowVM addPersonWindowVM)
        {
            var person = new Person
            {
                Name = addPersonWindowVM.Name,
                BirthDate = addPersonWindowVM.BirthDate,
                IsActive = false
            };

            var jsonInString = JsonConvert.SerializeObject(person);

            const string uri = "https://localhost:44340/api/chat";
            var isPersonExsist = await addPersonWindowVM.HttpClient.PutAsync(uri,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            if (isPersonExsist.IsSuccessStatusCode)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Пользователь с именем {person.Name} уже существует");
                    addPersonWindowVM.IsEnabled = true;
                });

                return;
            }

            var response = await addPersonWindowVM.HttpClient.PostAsync(uri,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (response.IsSuccessStatusCode)
                    MessageBox.Show($"Пользователь {person.Name} добавлен");
                addPersonWindowVM.IsEnabled = true;
            });
        }
    }
}