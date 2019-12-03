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

        protected virtual void Configure()
        {
        }

        public abstract void Execute(string arg);

        public BaseUiAction With(IUnityContainer container)
        {
            Container = container;
            WindowManager = Container.Resolve<IWindowManager>();
            return this;
        }

        #endregion Methods
    }
}