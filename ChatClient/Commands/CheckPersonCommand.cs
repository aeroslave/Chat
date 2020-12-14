namespace ChatClient.Commands
{
    using System.Threading.Tasks;
    using System.Windows;

    using ChatClient.Interfaces;
    using ChatClient.Models;
    using ChatClient.Utilites;
    using ChatClient.VMs;

    using Ninject;

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
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add("Имя пользователя не может быть пустым."));
                return;
            }

            var person = new Person
            {
                Name = mainWindowVM.UserName
            };

            var connectionService = NinjectKernel.Instance.Get<IPersonService>();
            var isPersonExist = await connectionService.CheckPersonExistingAsync(person);
            var token = await connectionService.CheckPersonActivityAsync(person);

            if (!string.IsNullOrWhiteSpace(token) && isPersonExist)
            {
                await ConnectionUtils.InitHubConnection(mainWindowVM, token);
                await ConnectionUtils.UpdateUserActivity(mainWindowVM);
            }
            else
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add(!isPersonExist
                        ? "Пользователь с таким именем не зарегистрирован!"
                        : "Пользователь с таким именем уже залогинился!"));
        }
    }
}