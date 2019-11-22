using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Probel.Lanceur.ViewModels
{
    public class ListShortcutViewModel : Conductor<IScreen>.Collection.OneActive
    {
        #region Fields

        private readonly IDatabaseService _databaseService;
        private readonly IDialogCoordinator _dialog;

        private readonly ILogService _log;
        private readonly ISettingsService _settingService;
        private ShortcutModel _selectedShortcut;
        private ObservableCollection<ShortcutModel> _shortcuts;
        public AppSettings _appSettings;

        #endregion Fields

        #region Constructors

        public ListShortcutViewModel(IDatabaseService databaseService, IDialogCoordinator dialog, EditShortcutViewModel editShortcutViewModel, ILogService log, ISettingsService settingService)
        {
            _settingService = settingService;
            _appSettings = settingService.Get();
            _log = log;
            _dialog = dialog;
            _databaseService = databaseService;
            EditShortcutViewModel = editShortcutViewModel;
        }

        #endregion Constructors

        #region Properties

        public EditShortcutViewModel EditShortcutViewModel { get; }

        public ShortcutModel SelectedShortcut
        {
            get => _selectedShortcut;
            set => Set(ref _selectedShortcut, value, nameof(SelectedShortcut));
        }

        public ObservableCollection<ShortcutModel> Shortcuts
        {
            get => _shortcuts;
            set => Set(ref _shortcuts, value, nameof(Shortcuts));
        }

        #endregion Properties

        #region Methods

        public void ActivateDetail(object view)
        {
            if (view is ShortcutModel model)
            {
                EditShortcutViewModel.RefreshData(model);
                ActivateItem(EditShortcutViewModel);
            }
        }

        public async Task<bool> AskForDeletion(string name)
        {
            var opt = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            var result = MessageDialogResult.Negative;
            try
            {
                result = await _dialog.ShowMessageAsync(this, "QUESTION", $"Do you want to delte the shortcut '{name}'?", MessageDialogStyle.AffirmativeAndNegative, opt);
            }
            catch (Exception ex) { _log.Debug(ex); }

            return (result == MessageDialogResult.Affirmative);
        }

        public void CreateKeyword()
        {
            EditShortcutViewModel.Shortcut = new ShortcutModel();
            EditShortcutViewModel.Names = new ObservableCollection<ShortcutNameModel>();
            ActivateItem(EditShortcutViewModel);
        }

        public void RefreshData()
        {
            _appSettings = _settingService.Get();
            var sc = _databaseService.GetShortcuts(_appSettings.SessionId).AsModel();
            Shortcuts = new ObservableCollection<ShortcutModel>(sc);
            DeactivateItem(EditShortcutViewModel, true);
        }

        #endregion Methods
    }
}