namespace Probel.Lanceur.Plugin
{
    public class PluginContext : IPluginContext
    {
        #region Constructors

        public PluginContext(ILogService logService, IPluginViewFinder pluginViewAdapter, IUserNotifyer userNotifyer)
        {
            LogService = logService;
            PluginViewAdapter = pluginViewAdapter;
            UserNotifyer = userNotifyer;
        }

        #endregion Constructors

        #region Properties

        public ILogService LogService { get; }

        public IPluginViewFinder PluginViewAdapter { get; }

        public IUserNotifyer UserNotifyer
        {
            get;
        }

        #endregion Properties
    }
}