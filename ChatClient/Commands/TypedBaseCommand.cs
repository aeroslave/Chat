namespace ChatClient.Commands
{
    /// <summary>
    /// Базовый класс команд с проверкой типа параметра.
    /// </summary>
    /// <typeparam name="T">Тип параметра.</typeparam>
    public abstract class TypedBaseCommand<T> : BaseCommand
    {
        /// <summary>
        /// Выполнение команды.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        public override void Execute(object parameter)
        {
            if (parameter is T typeParameter)
                Execute(typeParameter);
        }

        /// <summary>
        /// Выполнение команды.
        /// </summary>
        /// <param name="parameter">Параметр заданного типа.</param>
        public abstract void Execute(T parameter);
    }
}