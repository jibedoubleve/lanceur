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

        #endregion Properties

        #region Methods

        public abstract void Execute(Cmdline cmd);

        public void Initialise(IPluginContext context)
        {
            Logger = context.LogService;
            MainView = context.PluginViewAdapter.GetView();

            if (!_isInitialised)
            {
                Initialise();
                _isInitialised = true;
            }
        }

        protected virtual void Initialise()
        {
            /* By default nothing to do...*/
        }

        #endregion Methods
    }
}