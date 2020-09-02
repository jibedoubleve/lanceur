using Probel.Lanceur.SharedKernel.Logs;
using Probel.Lanceur.SharedKernel.UserCom;

namespace Probel.Lanceur.Plugin
{
    public abstract class PluginBase : IPlugin
    {
        #region Fields

        private bool _isInitialised = false;

        #endregion Fields

        #region Properties

        protected ILogService Logger { get; private set; }
        protected IPluginView MainView { get; private set; }
        protected IUserNotifyer Notifyer { get; private set; }

        #endregion Properties

        #region Methods

        protected virtual void Initialise()
        {
            /* By default nothing to do...*/
        }

        public abstract void Execute(PluginCmdline cmd);

        public void Initialise(IPluginContext context)
        {
            Logger = context.LogService;
            MainView = context.PluginViewAdapter.GetView();
            Notifyer = context.UserNotifyer;

            if (!_isInitialised)
            {
                Initialise();
                _isInitialised = true;
            }
        }

        #endregion Methods
    }
}