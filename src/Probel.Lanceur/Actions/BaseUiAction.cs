using Caliburn.Micro;
using Unity;

namespace Probel.Lanceur.Actions
{
    public abstract class BaseUiAction : IUiAction
    {
        #region Properties

        protected IUnityContainer Container { get; private set; }
        protected IWindowManager WindowManager { get; private set; }

        #endregion Properties

        #region Methods

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

        protected abstract void Configure();

        protected abstract void DoExecute(string arg);

        #endregion Methods
    }
}