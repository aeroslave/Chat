namespace SignalRChatClient.Commands
{
    using System;
    using System.Windows.Input;

    using SignalRChatClient.Utilites;

    /// <summary>
    /// Команда получения соединения.
    /// </summary>
    public class GetConnectionCommand: ICommand
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

            ConnectionUtils.InitHubConnection(mainWindowVM);
        }

        public event EventHandler CanExecuteChanged;
    }
}