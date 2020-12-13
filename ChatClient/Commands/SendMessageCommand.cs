namespace ChatClient.Commands
{
    using System;
    using System.Linq;

    using ChatClient.Interfaces;
    using ChatClient.Models;
    using ChatClient.Utilites;
    using ChatClient.VMs;

    using Microsoft.AspNetCore.SignalR.Client;

    using Ninject;

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
                var connectionService = NinjectKernel.Instance.Get<IPersonService>();
                connectionService.AddMessage(new Message { Text = $"{mainWindowVM.UserName} отправил сообщение: {mainWindowVM.Message}" });
            }
            catch (Exception e)
            {
                mainWindowVM.MessageList.Add("Error: " + e.Message);
            }
        }
    }
}