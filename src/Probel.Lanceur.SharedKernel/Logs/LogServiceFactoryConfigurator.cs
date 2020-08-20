namespace Probel.Lanceur.SharedKernel.Logs
{
    public static class LogServiceFactoryConfigurator
    {
        #region Methods

        public static void Configure(ILogService logService)
        {
            LogServiceFactory.Set(logService);
        }

        #endregion Methods
    }
}