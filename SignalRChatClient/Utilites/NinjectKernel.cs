namespace SignalRChatClient.Utilites
{
    using Ninject;

    using SignalRChatClient.Interfaces;
    using SignalRChatClient.Services;

    public static class NinjectKernel
    {
        static NinjectKernel()
        {
            Kernel = new StandardKernel();
            Bind();
        }

        public static StandardKernel Kernel { get; }

        private static void Bind()
        {
            Kernel.Bind<IPersonService>().To<PersonService>().InSingletonScope();
        }
    }
}