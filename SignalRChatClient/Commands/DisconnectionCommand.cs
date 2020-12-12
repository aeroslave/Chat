namespace SignalRChatClient.Commands
{
    using System.Threading.Tasks;
    using System.Windows;

    using Microsoft.AspNetCore.SignalR.Client;

    using Ninject;

    using SignalRChatClient.Interfaces;
    using SignalRChatClient.Models;
    using SignalRChatClient.Utilites;

    /// <summary>
    /// Команда соединения с хабом.
    /// </summary>
    public class DisconnectionCommand : TypedBaseCommand<MainWindowVM>
    {
        /// <inheritdoc />
        public override void Execute(MainWindowVM mainWindowVM)
        {
            mainWindowVM.IsEnabled = false;
            Task.Run(() => LogOut(mainWindowVM));
        }

        /// <summary>
        /// Разлогиниться.
        /// </summary>
        /// <param name="mainWindowVM">Вью-модель главного окна.</param>
        private static async Task LogOut(MainWindowVM mainWindowVM)
        {
            var person = new Person
            {
                Name = mainWindowVM.UserName
            };

            var connectionService = NinjectKernel.Kernel.Get<IPersonService>();
            var isSuccess = await connectionService.LogOutAsync(person);

            await mainWindowVM.HubConnection.InvokeAsync("UpdateUsersActivity", mainWindowVM.UserName, false);

            Application.Current.Dispatcher?.Invoke(() =>
            {
                if (isSuccess)
                    mainWindowVM.MessageList.Add($"Пользователь {mainWindowVM.UserName} покинул здание!");

                mainWindowVM.IsEnabled = true;
            });
        }
    }
}