using Probel.Lanceur.Infrastructure;

namespace Probel.Lanceur.Plugin
{
    public class PluginContext : IPluginContext
    {
        #region Constructors

        public PluginContext(ILogService logService, IPluginViewFinder pluginViewAdapter, IUserNotifyerFactory factory)
        {
            LogService = logService;
            PluginViewAdapter = pluginViewAdapter;
            UserNotifyer = factory.Get();
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