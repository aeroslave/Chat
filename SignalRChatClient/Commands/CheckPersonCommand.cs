namespace SignalRChatClient.Commands
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    using Microsoft.AspNetCore.SignalR.Client;

    using Ninject;

    using SignalRChatClient.Interfaces;
    using SignalRChatClient.Models;
    using SignalRChatClient.Utilites;
    using SignalRChatClient.VMs;

    /// <summary>
    /// Команда проверки наличия пользователя.
    /// </summary>
    public class CheckPersonCommand : TypedBaseCommand<MainWindowVM>
    {
        /// <inheritdoc />
        public override void Execute(MainWindowVM mainWindowVM)
        {
            mainWindowVM.IsLogin = false;
            Task.Run(() => CheckPersonAsync(mainWindowVM));
        }

        /// <summary>
        /// Проверить пользователя.
        /// </summary>
        /// <param name="mainWindowVM">Вью-модель главного окна.</param>
        private static async Task CheckPersonAsync(MainWindowVM mainWindowVM)
        {
            if (string.IsNullOrWhiteSpace(mainWindowVM.UserName))
            {
                Application.Current.Dispatcher?.Invoke(() => mainWindowVM.MessageList.Add("Имя пользователя не может быть пустым."));
                return;
            }

            var person = new Person
            {
                Name = mainWindowVM.UserName
            };
            
            var connectionService = NinjectKernel.Instance.Get<IPersonService>();
            var isPersonExist = await connectionService.CheckPersonExistingAsync(person);
            var isPersonActive = await connectionService.CheckPersonActivityAsync(person);

            if (!isPersonActive && isPersonExist)
                await GetConnection(mainWindowVM);
            else
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add(!isPersonExist
                        ? "Пользователь с таким именем не зарегистрирован!"
                        : "Пользователь с таким именем уже залогинился!"));
        }

        /// <summary>
        /// Открывает соединение с хабом.
        /// </summary>
        /// <param name="mainWindowVM">Вью-модель главного окна.</param>
        public static async Task GetConnection(MainWindowVM mainWindowVM)
        {
            try
            {
                await mainWindowVM.HubConnection.InvokeAsync("UpdateUsersActivity", mainWindowVM.UserName, true);
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    mainWindowVM.MessageList.Add($"Добро пожаловать {mainWindowVM.UserName}");
                    mainWindowVM.IsLogin = true;
                });
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add(e.Message));
            }
        }
    }
}