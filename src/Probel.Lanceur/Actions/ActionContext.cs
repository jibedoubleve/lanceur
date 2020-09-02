using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.SharedKernel.Logs;
using Probel.Lanceur.SharedKernel.UserCom;
using Unity;

namespace Probel.Lanceur.Actions
{
    public interface IActionContext
    {
        #region Properties

        IUnityContainer Container { get; }
        IDataSourceService DataService { get; }
        IEventAggregator EventAggregator { get; }
        IKeywordLoader KeywordLoader { get; }
        ILogService Log { get; }
        IUserNotifyer Notifyer { get; }
        ISettingsService SettingsService { get; }
        ISlickRunImporterService SlickRunImporterService { get; }
        IMainViewFinder ViewFinder { get; }
        IWindowManager WindowManager { get; }

        #endregion Properties
    }

    public class ActionContext : IActionContext
    {
        #region Constructors

        public ActionContext(
            IDataSourceService dataService,
            ILogService log,
            IUserNotifyerFactory factory,
            IWindowManager windowManager,
            IMainViewFinder viewFinder,
            ISettingsService settingsService,
            IUnityContainer container,
            IEventAggregator eventAggregator,
            IKeywordLoader keywordLoader,
            ISlickRunImporterService slickRunImporterService)
        {
            KeywordLoader = keywordLoader;
            SlickRunImporterService = slickRunImporterService;
            EventAggregator = eventAggregator;
            Container = container;
            SettingsService = settingsService;
            ViewFinder = viewFinder;
            DataService = dataService;
            Log = log;
            Notifyer = factory.Get();
            WindowManager = windowManager;
        }

        #endregion Constructors

        #region Properties

        public IUnityContainer Container { get; }
        public IDataSourceService DataService { get; }
        public IEventAggregator EventAggregator { get; }
        public IKeywordLoader KeywordLoader { get; }
        public ILogService Log { get; }
        public IUserNotifyer Notifyer { get; }
        public ISettingsService SettingsService { get; }
        public ISlickRunImporterService SlickRunImporterService { get; }
        public IMainViewFinder ViewFinder { get; }
        public IWindowManager WindowManager { get; }

        #endregion Properties
    }
}