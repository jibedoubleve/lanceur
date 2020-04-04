using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Plugins;
using Probel.Lanceur.Core.Services;

namespace Probel.Lanceur.Core.PluginsImpl
{
    public abstract class PluginBase : IPlugin
    {
        #region Fields

        private bool _isInitialised = false;

        #endregion Fields

        #region Properties

        protected ILogService Logger { get; private set; }
        protected IMainView MainView { get; private set; }

        #endregion Properties

        #region Methods

        public abstract void Execute(Cmdline cmd);

        public void Initialise(ILogService logger, IApplicationManager vm)
        {
            Logger = logger;
            MainView = vm.GetMain();

            if (!_isInitialised)
            {
                Boot();
                _isInitialised = true;
            }
        }

        protected virtual void Boot()
        {
            /* By default nothing to do...*/
        }

        #endregion Methods
    }
}