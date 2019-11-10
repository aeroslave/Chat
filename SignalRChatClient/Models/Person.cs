namespace SignalRChatClient.Models
{
    /// <summary>
    /// Модель для передачи на сервер.
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
        /// Пароль.
        /// </summary>
        public string Password { get; set; }
    }
}