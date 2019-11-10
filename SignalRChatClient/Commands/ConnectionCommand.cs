namespace SignalRChatClient.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Команда соединения с хабом.
    /// </summary>
    public class ConnectionCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            if (!(parameter is MainWindowVM mainWindowVM))
                return;

            try
            {
                await mainWindowVM.HubConnection.StartAsync();
                mainWindowVM.MessageList.Add("Connection stated!");
            }
            catch (Exception e)
            {
                mainWindowVM.MessageList.Add(e.Message);
            }
        }
    }
}