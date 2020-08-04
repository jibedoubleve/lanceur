using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using Probel.Lanceur.Actions;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.PluginsImpl;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.ServicesImpl;
using Probel.Lanceur.Core.ServicesImpl.MacroManagement;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Infrastructure;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.Repositories;
using Probel.Lanceur.Services;
using Probel.Lanceur.SQLiteDb;
using Probel.Lanceur.SQLiteDb.Services;
using Probel.Lanceur.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Unity;

namespace Probel.Lanceur
{
    public class Bootstrapper : BootstrapperBase
    {
        #region Fields

        private IUnityContainer _container = new UnityContainer();

        #endregion Fields

        #region Constructors

        public Bootstrapper() => Initialize();

        #endregion Constructors

        #region Methods

        protected override void BuildUp(object instance) => _container.BuildUp(instance);

        protected override void Configure()
        {
            /* IOC */
            _container.RegisterSingleton<IWindowManager, WindowManager>();
            _container.RegisterSingleton<IEventAggregator, EventAggregator>();
            _container.RegisterSingleton<IActionManager, ActionManager>();

            _container.RegisterInstance(typeof(IDialogCoordinator), DialogCoordinator.Instance);

            _container.RegisterType<IClipboardService, ClipboardService>();
            _container.RegisterType<IAliasService, AliasService>();
            _container.RegisterType<ICommandRunner, CommandRunner>();
            _container.RegisterType<IParameterResolver, ParameterResolver>();
            _container.RegisterType<IKeywordService, KeywordService>();

            _container.RegisterType<IDataSourceService, SQLiteDatabaseService>();
            _container.RegisterType<ISlickRunImporterService, SQLiteSlickRunImporterService>();
            _container.RegisterType<ISlickRunExtractor, SlickRunExtractor>();
#if DEBUG
            _container.RegisterType<ISettingsService, DebugSettingsService>();
#else
            _container.RegisterType<ISettingsService, JsonSettingsService>();
#endif

            //_container.RegisterType<ILogService, TraceLogger>();
            _container.RegisterType<ILogService, NLogLogger>();
            _container.RegisterType<IScreenRuler, ScreenRuler>();
            _container.RegisterType<IKeywordService, KeywordService>();
            _container.RegisterType<IMacroRunner, MacroRunner>();
            _container.RegisterType<IUpdateService, SQLiteUpdateService>();
            _container.RegisterType<IKeywordLoader, KeywordLoader>();

            //UI
            //_container.RegisterType<IUserNotifyer, UserNotifyer>();
            _container.RegisterType<IUserNotifyer, Win10UserNotifyer>();

            _container.RegisterSingleton<INotificationManager, NotificationManager>();
            _container.RegisterSingleton<IAppRestarter, AppRestarter>();

            //Settings
            _container.RegisterType<IConnectionStringManager, ConnectionStringManager>();

            //Reserved keywords
            _container.RegisterType<IMainViewFinder, MainViewFinder>();
            _container.RegisterType<IActionContext, ActionContext>();

            //Plugins
            _container.RegisterType<IPluginLoader, PluginLoader>();
            _container.RegisterType<IPluginManager, PluginManager>();
            _container.RegisterType<IPluginConfigurator, PluginConfigurator>();
            _container.RegisterType<IPluginViewFinder, MainViewFinder>();
            _container.RegisterType<IActionCollection, ActionCollection>();
            _container.RegisterType<IPluginContext, PluginContext>();

            //Repositories
            _container.RegisterType<IAliasRepositoryBuilder, AliasRepositoryBuilder>();
            _container.RegisterType<IRepositoryContext, RepositoryContext>();

            //Views
            _container.RegisterSingleton<MainViewModel>();

            _container.RegisterType<StatisticsViewModel>();
            _container.RegisterType<SetupViewModel>();
            _container.RegisterType<EditSessionViewModel>();
            _container.RegisterType<EditPluginViewModel>();
            _container.RegisterType<EditDoubloonsViewModel>();
            _container.RegisterType<EditObsoleteKeywordsViewModel>();

            /* Default commands */
            ConfigureInternalCommands();
        }

        protected override IEnumerable<object> GetAllInstances(Type service) => _container.ResolveAll(service);

        protected override object GetInstance(Type service, string key) => _container.Resolve(service, key);

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var hasMutex = SingleInstance.WaitOne();

            if (hasMutex)
            {
                var u = _container.Resolve<IUpdateService>();
                u.UpdateDatabase();
                DisplayRootViewFor<MainViewModel>();
            }
            else
            {
                MessageBox.Show("An instance of Lanceur is already running.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Environment.Exit(0);
            }
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var l = _container.Resolve<ILogService>();
            var n = _container.Resolve<IUserNotifyer>();

            n.NotifyError($"Unexpected crash occured: {e.Exception.Message}");
            l.Fatal($"Unexpected crash occured: {e.Exception.Message}", e.Exception);

            e.Handled = true;
            base.OnUnhandledException(sender, e);

            SingleInstance.ReleaseMutex();
        }

        private void ConfigureInternalCommands()
        {
            var actionManager = _container.Resolve<IActionManager>();

            actionManager.Bind();
        }

        #endregion Methods
    }
}