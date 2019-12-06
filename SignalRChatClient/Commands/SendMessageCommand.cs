namespace SignalRChatClient.Commands
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    using Microsoft.AspNetCore.SignalR.Client;

    /// <summary>
    /// Команда отправки сообщеий.
    /// </summary>
    public class SendMessageCommand: ICommand
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
            if(!(parameter is MainWindowVM mainWindowVM)) 
                return;

            if (mainWindowVM.ActiveUsers.All(user => user != mainWindowVM.UserName))
            {
                mainWindowVM.MessageList.Add("Необходимой залогиниться.");
                return;
            }

            try
            {
                mainWindowVM.HubConnection.InvokeAsync("SendMessage", mainWindowVM.UserName, mainWindowVM.Message);
            }
            catch (Exception e)
            {
                mainWindowVM.MessageList.Add("Error: " + e.Message);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}