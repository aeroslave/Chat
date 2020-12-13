namespace ChatClient.Models
{
    /// <summary>
    /// Модель для передачи на сервер.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Флаг активности пользователя.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }
    }
}