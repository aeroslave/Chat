namespace SignalRChatClient.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SignalRChatClient.Models;

    /// <summary>
    /// Интерфейс взаимодействия с АПИ.
    /// </summary>
    internal interface IPersonService
    {
        /// <summary>
        /// Проверить активность пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        /// <returns>True - если пользователь активен.</returns>
        Task<bool> CheckPersonActivityAsync(Person person);

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