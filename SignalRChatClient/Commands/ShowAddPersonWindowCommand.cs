namespace SignalRChatClient.Commands
{
    using System;
    using System.Windows.Input;

    using SignalRChatClient.VMs;
    using SignalRChatClient.Windows;

    /// <summary>
    /// Команда открытия окна добавления пользователя.
    /// </summary>
    public class ShowAddPersonWindowCommand: ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Выполнить.
        /// </summary>
        public void Execute(object parameter)
        {
            if (!(parameter is MainWindowVM mainWindowVM))
                return;

            var addPersonWindowVM = new AddPersonWindowVM(mainWindowVM.HttpClient);
            var addPersonWindow = new AddPersonWindow{DataContext = addPersonWindowVM};
            addPersonWindow.ShowDialog();
        }

        public event EventHandler CanExecuteChanged;
    }
}