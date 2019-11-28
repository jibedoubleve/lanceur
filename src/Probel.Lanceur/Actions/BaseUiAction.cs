using Caliburn.Micro;
using Unity;

namespace Probel.Lanceur.Actions
{
    public abstract class BaseUiAction : IUiAction
    {
        #region Fields

        protected IWindowManager WindowManager { get; }
        protected IUnityContainer Container { get; }

        #endregion Fields

        #region Constructors

        public BaseUiAction(IUnityContainer container)
        {
            Container = container;

            WindowManager = Container.Resolve<IWindowManager>();
        }

        #endregion Constructors

        #region Methods

        public abstract void Execute(string arg);

        #endregion Methods
    }
}