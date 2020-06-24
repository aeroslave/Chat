namespace SignalRChatClient.Commands
{
    using System.Threading.Tasks;
    using System.Windows;

    using Ninject;

    using SignalRChatClient.Interfaces;
    using SignalRChatClient.Models;
    using SignalRChatClient.VMs;

    /// <summary>
    /// Команда отправки сообщения.
    /// </summary>
    public class PostToWebApiCommand : TypedBaseCommand<AddPersonWindowVM>
    {
        /// <inheritdoc />
        public override void Execute(AddPersonWindowVM addPersonWindowVM)
        {
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

            var ninjectKernel = new StandardKernel();
            var connectionService = ninjectKernel.Get<IPersonService>();

            var isPersonExsist = await connectionService.CheckPersonExistingAsync(person);

            if (isPersonExsist)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Пользователь с именем {person.Name} уже существует");
                    addPersonWindowVM.IsEnabled = true;
                });

                return;
            }

            var isPersonCreated = await connectionService.CreatePersonAsync(person);

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (isPersonCreated)
                    MessageBox.Show($"Пользователь {person.Name} добавлен");
                addPersonWindowVM.IsEnabled = true;
            });
        }
    }
}