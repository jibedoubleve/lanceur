using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using Probel.Lanceur.Actions;
using Probel.Lanceur.Core;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Plugins;
using Probel.Lanceur.Core.PluginsImpl;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.ServicesImpl;
using Probel.Lanceur.Core.ServicesImpl.MacroManagement;
using Probel.Lanceur.Helpers;
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
            _container.RegisterType<IReservedKeywordService, ReservedKeywordService>();

            _container.RegisterType<IDataSourceService, SQLiteDatabaseService>();
            _container.RegisterType<ISlickRunImporterService, SQLiteSlickRunImporterService>();
            _container.RegisterType<ISlickRunExtractor, SlickRunExtractor>();
            _container.RegisterType<ISettingsService, JsonSettingsService>();
            //_container.RegisterType<ILogService, TraceLogger>();
            _container.RegisterType<ILogService, NLogLogger>();
            _container.RegisterType<IScreenRuler, ScreenRuler>();
            _container.RegisterType<IReservedKeywordService, ReservedKeywordService>();
            _container.RegisterType<IMacroService, MacroService>();
            _container.RegisterType<IUpdateService, SQLiteUpdateService>();
            _container.RegisterType<IKeywordLoader, KeywordLoader>();

            //UI
            _container.RegisterType<IUserNotifyer, UserNotifyer>();
            _container.RegisterSingleton<INotificationManager, NotificationManager>();

            //Settings
            _container.RegisterType<IConnectionStringManager, ConnectionStringManager>();

            //Plugins
            _container.RegisterType<IPluginLoader, PluginLoader>();
            _container.RegisterType<IPluginManager, PluginManager>();
            _container.RegisterType<IApplicationManager, ApplicationManager>();

            //Views
            _container.RegisterSingleton<MainViewModel>();

            _container.RegisterType<StatisticsViewModel>();
            _container.RegisterType<SetupViewModel>();
            _container.RegisterType<EditSessionViewModel>();

            /* Default commands */
            ConfigureInternalCommands();
        }

        protected override IEnumerable<object> GetAllInstances(Type service) => _container.ResolveAll(service);

        protected override object GetInstance(Type service, string key) => _container.Resolve(service, key);

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var u = _container.Resolve<IUpdateService>();

            u.UpdateDatabase();

            DisplayRootViewFor<MainViewModel>();
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var l = _container.Resolve<ILogService>();
            var n = _container.Resolve<IUserNotifyer>();

            n.NotifyError($"Unexpected crash occured: {e.Exception.Message}");
            l.Fatal($"Unexpected crash occured: {e.Exception.Message}", e.Exception);
            e.Handled = true;
            base.OnUnhandledException(sender, e);
        }

        private void ConfigureInternalCommands()
        {
            var actionManager = _container.Resolve<IActionManager>();

            actionManager.Bind();
        }

        #endregion Methods
    }
}