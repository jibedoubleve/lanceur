using Probel.Lanceur.Plugin;

namespace Probel.Lanceur.Plugin
{
    public class PluginContext : IPluginContext
    {
        #region Constructors

        public PluginContext(ILogService logService, IPluginViewAdapter pluginViewAdapter)
        {
            LogService = logService;
            PluginViewAdapter = pluginViewAdapter;
        }

        #endregion Constructors

        #region Properties

        public ILogService LogService { get; }

        public IPluginViewAdapter PluginViewAdapter { get; }

        #endregion Properties
    }
}