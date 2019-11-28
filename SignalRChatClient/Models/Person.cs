namespace SignalRChatClient.Models
{
    using System;

    /// <summary>
    /// Модель для передачи на сервер.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Флаг активности пользователя.
        /// </summary>
        public bool IsActive { get; set; }
    }
}