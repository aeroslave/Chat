namespace SignalRChat
{
    using System;

    /// <summary>
    /// Исключение для контроллера.
    /// </summary>
    public class ChatControllerException : Exception
    {
        /// <summary>
        /// Исключение для контроллера.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public ChatControllerException(string message) : base(message)
        {
        }
    }
}