using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Probel.Lanceur.Actions;
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

            ss.Bind(Keywords.Quit, arg => new QuitAction(_container).Execute(arg));
            ss.Bind(Keywords.Import, arg => new ImportAction(_container).Execute(arg));
            ss.Bind(Keywords.Setup, arg => new SetupAction(_container).Execute(arg));
            ss.Bind(Keywords.Corner, arg => new CornerAction(_container).Execute(arg));
            //---
            ss.Bind(Keywords.Clear, arg => new ClearAction(_container).Execute(arg));
            ss.Bind(Keywords.Echo, arg => new EchoAction(_container).Execute(arg)); ;
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