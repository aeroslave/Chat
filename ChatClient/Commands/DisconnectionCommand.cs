namespace ChatClient.Commands
{
    using System.Threading.Tasks;
    using System.Windows;

    using ChatClient.Interfaces;
    using ChatClient.Models;
    using ChatClient.Utilites;
    using ChatClient.VMs;

    using Microsoft.AspNetCore.SignalR.Client;

    using Ninject;

    /// <summary>
    /// Команда соединения с хабом.
    /// </summary>
    public class DisconnectionCommand : TypedBaseCommand<MainWindowVM>
    {
        /// <inheritdoc />
        public override void Execute(MainWindowVM mainWindowVM)
        {
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

            var connectionService = NinjectKernel.Instance.Get<IPersonService>();
            var isSuccess = await connectionService.LogOutAsync(person);

            await mainWindowVM.HubConnection.InvokeAsync("UpdateUsersActivity", mainWindowVM.UserName, false);
            await mainWindowVM.HubConnection.DisposeAsync();

            Application.Current.Dispatcher?.Invoke(() =>
            {
                if (isSuccess)
                    mainWindowVM.MessageList.Add($"Пользователь {mainWindowVM.UserName} покинул здание!");

                mainWindowVM.IsLogin = false;
            });
        }
    }
}