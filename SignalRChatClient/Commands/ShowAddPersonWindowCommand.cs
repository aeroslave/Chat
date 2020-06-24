namespace SignalRChatClient.Commands
{
    using SignalRChatClient.VMs;
    using SignalRChatClient.Windows;

    /// <summary>
    /// Команда открытия окна добавления пользователя.
    /// </summary>
    public class ShowAddPersonWindowCommand : TypedBaseCommand<MainWindowVM>
    {
        /// <inheritdoc />
        public override void Execute(MainWindowVM mainWindowVM)
        {
            var addPersonWindowVM = new AddPersonWindowVM();
            var addPersonWindow = new AddPersonWindow { DataContext = addPersonWindowVM };
            addPersonWindow.ShowDialog();
        }
    }
}