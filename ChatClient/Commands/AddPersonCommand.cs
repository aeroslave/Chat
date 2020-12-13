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
    /// Команда добавления пользователя.
    /// </summary>
    public class AddPersonCommand : TypedBaseCommand<MainWindowVM>
    {
        /// <inheritdoc />
        public override void Execute(MainWindowVM addPersonWindowVM)
        {
            Task.Run(() => AddPersonAsync(addPersonWindowVM));
        }

        /// <summary>
        /// Добавить пользователя.
        /// </summary>
        /// <param name="mainWindowVM"> Вью-модель окна.</param>
        private static async Task AddPersonAsync(MainWindowVM mainWindowVM)
        {
            if (string.IsNullOrWhiteSpace(mainWindowVM.UserName))
            {
                Application.Current.Dispatcher?.Invoke(() =>
                    mainWindowVM.MessageList.Add("Имя пользователя не может быть пустым."));
                return;
            }

            var person = new Person
            {
                Name = mainWindowVM.UserName,
                IsActive = false
            };

            var connectionService = NinjectKernel.Instance.Get<IPersonService>();
            var isPersonExsist = await connectionService.CheckPersonExistingAsync(person);

            if (isPersonExsist)
            {
                Application.Current.Dispatcher.Invoke(() =>
                    mainWindowVM.MessageList.Add($"Пользователь с именем {person.Name} уже существует"));

                return;
            }

            var isPersonCreated = await connectionService.CreatePersonAsync(person);

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (isPersonCreated)
                    mainWindowVM.MessageList.Add($"Пользователь {person.Name} добавлен");
            });

            if (isPersonCreated)
                await ConnectionUtils.UpdateUserActivity(mainWindowVM);
        }
    }
}