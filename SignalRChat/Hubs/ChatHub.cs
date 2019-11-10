namespace SignalRChat.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        /// <summary>
        /// Отправить сообщение.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="message">Сообщение.</param>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}