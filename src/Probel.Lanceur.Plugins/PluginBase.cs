using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugins;

namespace Probel.Lanceur.Core.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        #region Fields

        private bool _isInitialised = false;

        protected IMainView MainView { get; private set; }

        #endregion Fields

        #region Properties

        protected ILogService Logger { get; private set; }

        #endregion Properties

        #region Methods

        public abstract void Execute(string parameters);

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