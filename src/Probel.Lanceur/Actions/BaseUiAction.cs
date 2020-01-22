using Caliburn.Micro;
using Unity;

namespace Probel.Lanceur.Actions
{
    public abstract class BaseUiAction : IUiAction
    {
        #region Fields

        private bool _isConfigured = false;

        #endregion Fields

        #region Properties

        protected IUnityContainer Container { get; private set; }
        protected IWindowManager WindowManager { get; private set; }

        #endregion Properties

        #region Methods

        private void DoConfigure()
        {
            if (_isConfigured == false) { Configure(); }
        }

        protected abstract void Configure();

        protected abstract void DoExecute(string arg);
        public void Execute(string arg)
        {
            Configure();
            DoExecute(arg);
        }

        public IUiAction With(IUnityContainer container)
        {
            Container = container;
            WindowManager = Container.Resolve<IWindowManager>();
            return this;
        }

        #endregion Methods
    }
}