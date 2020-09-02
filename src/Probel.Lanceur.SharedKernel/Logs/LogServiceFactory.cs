namespace Probel.Lanceur.SharedKernel.Logs
{
    public static class LogServiceFactory
    {
        #region Fields

        private static ILogService _logService;

        #endregion Fields

        #region Methods

        internal static void Set(ILogService logService) => _logService = logService;

        public static ILogService Get() => _logService;

        #endregion Methods
    }
}