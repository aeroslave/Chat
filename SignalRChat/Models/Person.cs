namespace SignalRChat.Models
{
    using System;

    /// <summary>
    /// Модель персонажа.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Флаг активности пользователя.
        /// </summary>
        public bool IsActive { get; set; }
    }
}