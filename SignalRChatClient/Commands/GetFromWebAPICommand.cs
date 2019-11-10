namespace SignalRChatClient.Commands
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Newtonsoft.Json;

    using SignalRChatClient.Models;

    /// <summary>
    /// Команда отправки сообщения на WebAPI.
    /// </summary>
    public class GetFromWebApiCommand : ICommand
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

            Task.Run(() => GetStringAsync(mainWindowVM));
        }

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="mainWindowVM"> Вью-модель окна.</param>
        private static async Task GetStringAsync(MainWindowVM mainWindowVM)
        {
            var response = await mainWindowVM.HttpClient.GetStringAsync("https://localhost:44340/api/chat/1");
            var person = JsonConvert.DeserializeObject<Person>(response);
            //if (response.IsSuccessStatusCode)
            //{
            //    var message = await response.Content.ReadAsStringAsync();
            //    mainWindowVM.MessageList.Add(message);
            //}
        }
    }
}