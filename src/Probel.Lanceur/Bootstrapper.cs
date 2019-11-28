using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.ServicesImpl;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Services;
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

        private void ConfigureInternalCommands()
        {
            var ss = _container.Resolve<IReservedKeywordService>();
            var importer = _container.Resolve<ISlickRunImporterService>();
            var db = _container.Resolve<IDataSourceService>();
            var windowManager = _container.Resolve<IWindowManager>();
            var eManager = _container.Resolve<IEventAggregator>();

            ss.Bind(Keywords.Quit, arg => Application.Current.Shutdown());
            ss.Bind(Keywords.Import, arg => importer.Import());
            ss.Bind(Keywords.Setup, arg => windowManager.ShowDialog(_container.Resolve<SetupViewModel>()));
            ss.Bind(Keywords.Corner, arg => eManager.PublishOnUIThread(Notifications.CornerCommand));
            //---
            ss.Bind(Keywords.Clear, arg => db.Clear());
            ss.Bind(Keywords.Echo, arg => MessageBox.Show(arg, "Easter Egg", MessageBoxButton.OK, MessageBoxImage.Exclamation));
        }

        protected override void BuildUp(object instance) => _container.BuildUp(instance);

        protected override void Configure()
        {
            /* IOC */
            _container.RegisterSingleton<IWindowManager, WindowManager>();
            _container.RegisterSingleton<IEventAggregator, EventAggregator>();

            _container.RegisterInstance(typeof(IDialogCoordinator), DialogCoordinator.Instance);

            _container.RegisterType<IClipboardService, ClipboardService>();
            _container.RegisterType<IAliasService, DefaultAliasService>();
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

            _container.RegisterType<MainViewModel>();
            _container.RegisterType<SetupViewModel>();
            _container.RegisterType<EditSessionViewModel>();

            /* Default commands */
            ConfigureInternalCommands();
        }

        protected override IEnumerable<object> GetAllInstances(Type service) => _container.ResolveAll(service);

        protected override object GetInstance(Type service, string key) => _container.Resolve(service, key);

        protected override void OnStartup(object sender, StartupEventArgs e) => DisplayRootViewFor<MainViewModel>();

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Unexpected crash occured: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _container.Resolve<ILogService>().Fatal($"Unexpected crash occured: {e.Exception.Message}", e.Exception);
            base.OnUnhandledException(sender, e);
        }
        #endregion Methods
    }
}