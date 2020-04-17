namespace Probel.Lanceur.Plugin
{
    public class PluginContext : IPluginContext
    {
        #region Constructors

        public PluginContext(ILogService logService, IPluginViewAdapter pluginViewAdapter, IUserNotifyer userNotifyer)
        {
            LogService = logService;
            PluginViewAdapter = pluginViewAdapter;
            UserNotifyer = userNotifyer;
        }

        #endregion Constructors

        #region Properties

        public ILogService LogService { get; }

        public IPluginViewAdapter PluginViewAdapter { get; }

        public IUserNotifyer UserNotifyer
        {
            get;
        }

        #endregion Properties
    }
}