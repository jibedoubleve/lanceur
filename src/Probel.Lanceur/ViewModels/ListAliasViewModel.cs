using Caliburn.Micro;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Models;
using Probel.Lanceur.SharedKernel.UserCom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Lanceur.ViewModels
{
    public class ListAliasViewModel : Conductor<IScreen>.Collection.OneActive
    {
        #region Fields

        private readonly IDataSourceService _databaseService;
        private readonly IUserNotifyer _notifyer;
        private readonly ISettingsService _settingService;
        private IEnumerable<AliasModel> _bufferAlias;
        private AliasModel _selectedAlias;
        private ObservableCollection<AliasModel> aliases;
        public AppSettings _appSettings;

        #endregion Fields

        #region Constructors

        public ListAliasViewModel(IDataSourceService databaseService,
            EditAliasViewModel editaliasViewModel,
            ISettingsService settingService,
            IUserNotifyerFactory factory)
        {
            _notifyer = factory.Get();
            _settingService = settingService;
            _appSettings = settingService.Get();
            _databaseService = databaseService;
            EditAliasViewModel = editaliasViewModel;
            EditAliasViewModel.OnRefresh = () => RefreshData();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<AliasModel> Aliases
        {
            get => aliases;
            set => Set(ref aliases, value, nameof(Aliases));
        }

        public EditAliasViewModel EditAliasViewModel { get; }

        public AliasModel SelectedAlias
        {
            get => _selectedAlias;
            set => Set(ref _selectedAlias, value, nameof(SelectedAlias));
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
            var answer = await _notifyer.AskAsync($"Do you want to delete the alias '{name}'?");
            return answer == NotificationResult.Affirmative;
        }

        public void CreateKeyword(string name = null)
        {
            EditAliasViewModel.Alias = new AliasModel() { IdSession = _appSettings.SessionId };
            EditAliasViewModel.Names = new ObservableCollection<AliasNameModel>();

            if (string.IsNullOrEmpty(name) == false) { EditAliasViewModel.Names.Add(name); }

            ActivateItem(EditAliasViewModel);
        }

        public void RefreshData()
        {
            _appSettings = _settingService.Get();
            _bufferAlias = _databaseService.GetAliases(_appSettings.SessionId).AsModel();
            Aliases = new ObservableCollection<AliasModel>(_bufferAlias);
            DeactivateItem(EditAliasViewModel, true);
        }

        public void Search(object c)
        {
            var criterion = c.ToString();
            if (string.IsNullOrWhiteSpace(criterion)) { Aliases = new ObservableCollection<AliasModel>(_bufferAlias); }
            else
            {
                var tmp = (from a in _bufferAlias
                           where a.Name.ToLower().Contains(criterion)
                           select a);
                Aliases = new ObservableCollection<AliasModel>(tmp);
            }
        }

        #endregion Methods
    }
}