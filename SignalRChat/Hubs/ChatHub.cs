namespace SignalRChat.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    using SignalRChat.Models;

    public class ChatHub : Hub
    {
        private readonly UsersContext _context;

        public ChatHub(UsersContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Завершение подключения.
        /// </summary>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User.Identity.Name;
            SetActivity(userName, false);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Отправить сообщение.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="message">Сообщение.</param>
        [Authorize]
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        /// <summary>
        /// Обновить список активных пользователей.
        /// </summary>
        [Authorize]
        public async Task UpdateUsersActivity(string user, bool isActive)
        {
            SetActivity(user, isActive);
            await Clients.All.SendAsync("UpdateUsers", user, isActive);
        }

        /// <summary>
        /// Установить активность пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="isActive">Флаг активности.</param>
        private void SetActivity(string userName, bool isActive)
        {
            var dBPerson = _context.Persons.FirstOrDefaultAsync(it => it.Name == userName).Result;

            if (dBPerson == null)
                return;

            dBPerson.IsActive = isActive;
            _context.SaveChanges();
        }
    }
}