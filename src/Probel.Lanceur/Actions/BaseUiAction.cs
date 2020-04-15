using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Services;
using Unity;

namespace Probel.Lanceur.Actions
{
    public abstract class BaseUiAction : IUiAction
    {
        #region Properties

        protected IUnityContainer Container { get; private set; }
        protected IDataSourceService DataService { get; private set; }
        protected ILogService Log { get; private set; }
        protected IUserNotifyer Notifyer { get; private set; }
        protected IWindowManager WindowManager { get; private set; }

        #endregion Properties

        #region Methods

        public void Execute(string arg)
        {
            Configure();
            DoExecute(arg);
        }

        public IUiAction With(IUnityContainer container, IDataSourceService dataService, ILogService log, IUserNotifyer notifyer)
        {
            Notifyer = notifyer;
            Log = log;
            DataService = dataService;
            Container = container;
            WindowManager = Container.Resolve<IWindowManager>();
            return this;
        }

        protected virtual void Configure()
        {
        }

        protected abstract void DoExecute(string arg);

        #endregion Methods
    }
}