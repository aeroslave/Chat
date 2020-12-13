namespace ChatClient.Models
{
    /// <summary>
    /// Модель сообщения.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string Text { get; set; }
    }
}