using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.SharedKernel.Logs;
using Probel.Lanceur.SharedKernel.UserCom;
using Unity;

namespace Probel.Lanceur.Actions
{
    public abstract class BaseUiAction : IUiAction
    {
        #region Fields

        private IUnityContainer _container;

        #endregion Fields

        #region Properties

        protected IDataSourceService DataService { get; private set; }
        protected IKeywordLoader KeywordLoader { get; private set; }
        protected ILogService Log { get; private set; }
        protected IUserNotifyer Notifyer { get; private set; }
        protected ISettingsService SettingsService { get; private set; }
        protected IMainViewFinder ViewFinder { get; private set; }
        protected IWindowManager WindowManager { get; private set; }
        public IEventAggregator EventAggregator { get; private set; }
        public ISlickRunImporterService SlickRunImporterService { get; private set; }

        #endregion Properties

        #region Methods

        protected virtual void Configure()
        {
        }

        protected abstract ExecutionResult DoExecute(string arg);

        protected T GetViewModel<T>() where T : Screen
        {
            var result = _container?.Resolve<T>();
            return result;
        }

        public ExecutionResult Execute(string arg)
        {
            Configure();
            return DoExecute(arg);
        }

        public IUiAction With(IActionContext c)
        {
            _container = c.Container;
            ViewFinder = c.ViewFinder;
            Notifyer = c.Notifyer;
            Log = c.Log;
            DataService = c.DataService;
            WindowManager = c.WindowManager;
            SettingsService = c.SettingsService;
            EventAggregator = c.EventAggregator;
            KeywordLoader = c.KeywordLoader;
            SlickRunImporterService = c.SlickRunImporterService;
            return this;
        }

        #endregion Methods
    }
}