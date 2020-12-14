namespace ChatClient.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChatClient.Models;

    /// <summary>
    /// Интерфейс взаимодействия с АПИ.
    /// </summary>
    internal interface IPersonService
    {
        /// <summary>
        /// Добавить сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        Task<bool> AddMessage(Message message);

        /// <summary>
        /// Проверить активность пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        /// <returns>True - если пользователь активен.</returns>
        Task<string> CheckPersonActivityAsync(Person person);

        /// <summary>
        /// Проверить существование пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        /// <returns>True - если пользователь существует.</returns>
        Task<bool> CheckPersonExistingAsync(Person person);

        /// <summary>
        /// Создать пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        /// <returns>True - если пользователь создан.</returns>
        Task<bool> CreatePersonAsync(Person person);

        /// <summary>
        /// Получить список сообщений.
        /// </summary>
        /// <returns>Список сообщений.</returns>
        Task<List<Message>> GetMessages();

        /// <summary>
        /// Получить список пользователей асинхронно.
        /// </summary>
        Task<List<Person>> GetPersonsAsync();

        /// <summary>
        /// Разлогинить пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        /// <returns>True - если успешно.</returns>
        Task<bool> LogOutAsync(Person person);
    }
}