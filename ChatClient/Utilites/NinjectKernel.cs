namespace ChatClient.Utilites
{
    using ChatClient.Interfaces;
    using ChatClient.Services;

    using Ninject;

    public static class NinjectKernel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        static NinjectKernel()
        {
            Instance = new StandardKernel();
            Bind();
        }

        /// <summary>
        /// Экземпляр ядра.
        /// </summary>
        public static StandardKernel Instance { get; }

        /// <summary>
        /// Создать привязки.
        /// </summary>
        private static void Bind()
        {
            Instance.Bind<IPersonService>().To<PersonService>().InSingletonScope();
        }
    }
}