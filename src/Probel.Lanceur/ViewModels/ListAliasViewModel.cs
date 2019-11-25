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
    public class ListAliasViewModel : Conductor<IScreen>.Collection.OneActive
    {
        #region Fields

        private readonly IDatabaseService _databaseService;
        private readonly IDialogCoordinator _dialog;

        private readonly ILogService _log;
        private readonly ISettingsService _settingService;
        private AliasModel _selectedAlias;
        private ObservableCollection<AliasModel> aliases;
        public AppSettings _appSettings;

        #endregion Fields

        #region Constructors

        public ListAliasViewModel(IDatabaseService databaseService, IDialogCoordinator dialog, EditAliasViewModel editaliasViewModel, ILogService log, ISettingsService settingService)
        {
            _settingService = settingService;
            _appSettings = settingService.Get();
            _log = log;
            _dialog = dialog;
            _databaseService = databaseService;
            EditAliasViewModel = editaliasViewModel;
        }

        #endregion Constructors

        #region Properties

        public EditAliasViewModel EditAliasViewModel { get; }

        public AliasModel SelectedAlias
        {
            get => _selectedAlias;
            set => Set(ref _selectedAlias, value, nameof(SelectedAlias));
        }

        public ObservableCollection<AliasModel> Aliases
        {
            get => aliases;
            set => Set(ref aliases, value, nameof(Aliases));
        }

        #endregion Properties

        #region Methods

        public void ActivateDetail(object view)
        {
            if (view is AliasModel model)
            {
                EditAliasViewModel.RefreshData(model);
                ActivateItem(EditAliasViewModel);
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
                result = await _dialog.ShowMessageAsync(this, "QUESTION", $"Do you want to delte the alias '{name}'?", MessageDialogStyle.AffirmativeAndNegative, opt);
            }
            catch (Exception ex) { _log.Debug(ex); }

            return (result == MessageDialogResult.Affirmative);
        }

        public void CreateKeyword()
        {
            EditAliasViewModel.Alias = new AliasModel();
            EditAliasViewModel.Names = new ObservableCollection<AliasNameModel>();
            ActivateItem(EditAliasViewModel);
        }

        public void RefreshData()
        {
            _appSettings = _settingService.Get();
            var sc = _databaseService.GetAliases(_appSettings.SessionId).AsModel();
            Aliases = new ObservableCollection<AliasModel>(sc);
            DeactivateItem(EditAliasViewModel, true);
        }

        #endregion Methods
    }
}