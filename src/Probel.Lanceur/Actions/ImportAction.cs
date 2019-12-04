using Probel.Lanceur.Core.Services;
using Probel.Lanceur.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace Probel.Lanceur.Actions
{
    [UiAction]
    public class ImportAction : BaseUiAction
    {
        #region Fields

        private ISlickRunImporterService _importer;
        private ISettingsService _settingsService;

        #endregion Fields

        #region Properties

        private ISlickRunImporterService Importer
        {
            get
            {
                if (_importer == null) { _importer = Container.Resolve<ISlickRunImporterService>(); }
                return _importer;
            }
        }

        private ISettingsService SettingsService
        {
            get
            {
                if (_settingsService == null) { _settingsService = Container.Resolve<ISettingsService>(); }
                return _settingsService;
            }
        }

        #endregion Properties

        #region Methods

        public override async void Execute(string arg)
        {
            var vm = Container.Resolve<ImportViewModel>();
            WindowManager.ShowWindow(vm);

            EventHandler<ImportUpdatedEventArg> update = (sender, e) =>
            {
                Application.Current.Dispatcher.Invoke(() => vm.Update(e.Progress, e.Output));
            };

            Importer.ImportUpdated += update;

            var sessionId = await Task.Run(() => Importer.Import());

            //Now update the settings
            var s = SettingsService.Get();
            s.SessionId = sessionId;
            SettingsService.Save(s);

            Importer.ImportUpdated -= update;
            vm.TryClose();
        }

        #endregion Methods
    }
}