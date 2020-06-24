namespace SignalRChatClient.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Базовый класс команд.
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        /// <summary>
        /// Можно ли выполнить?
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <returns>True - если можно выполнить.</returns>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Обработчик события проверки выполнения.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Выполнение команды.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        public abstract void Execute(object parameter);
    }
}