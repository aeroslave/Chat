namespace SignalRChatClient.Commands
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.SignalR.Client;

    /// <summary>
    /// Команда отправки сообщеий.
    /// </summary>
    public class SendMessageCommand : TypedBaseCommand<MainWindowVM>
    {
        /// <inheritdoc />
        public override void Execute(MainWindowVM mainWindowVM)
        {
            if (mainWindowVM.ActiveUsers.All(user => user != mainWindowVM.UserName))
            {
                mainWindowVM.MessageList.Add("Необходимо залогиниться.");
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
    }
}