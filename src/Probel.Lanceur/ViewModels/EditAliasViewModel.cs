using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Models;
using Probel.Lanceur.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Probel.Lanceur.ViewModels
{
    public class EditAliasViewModel : Screen
    {
        #region Fields

        private readonly IDataSourceService _databaseService;
        private readonly IUserNotifyer _userNotifyer;
        private AliasModel _alias;
        private bool _isCreation;
        private ObservableCollection<AliasNameModel> _names;

        #endregion Fields

        #region Constructors

        public EditAliasViewModel(IDataSourceService databaseService, ILogService log, IUserNotifyer userNotifyer)
        {
            Log = log;
            _userNotifyer = userNotifyer;
            _databaseService = databaseService;
        }

        #endregion Constructors

        #region Properties

        private ListAliasViewModel ParentVm => Parent as ListAliasViewModel;

        public AliasModel Alias
        {
            get => _alias;
            set
            {
                if (Set(ref _alias, value, nameof(Alias)))
                {
                    IsCreation = value.Id > 0;
                    NotifyOfPropertyChange(nameof(IsCreation));
                }
            }
        }

        public bool IsCreation
        {
            get => _isCreation;
            set => Set(ref _isCreation, value, nameof(IsCreation));
        }

        public ILogService Log { get; }

        public ObservableCollection<AliasNameModel> Names
        {
            get => _names;
            set => Set(ref _names, value, nameof(Names));
        }

        #endregion Properties

        #region Methods

        private void RefreshParentList()
        {
            if (Parent is ListAliasViewModel vm) { vm.RefreshData(); }
        }

        public bool CanDeleteAlias() => _alias != null;

        public bool CanUpdateAlias() => Alias != null;

        public void CreateAlias()
        {
            _databaseService.Create(Alias.AsEntity(), Names.AsNames());
            RefreshParentList();
            _userNotifyer.NotifyInfo("Alias created!");
        }

        public async Task DeleteAliasAsync()
        {            
            if (await ParentVm.AskForDeletion(Alias.Name))
            {
                _databaseService.Delete(Alias.AsEntity());
                RefreshParentList();
            }
        }

        public void RefreshData(AliasModel model = null)
        {
            if (model != null) { Alias = model; }
            if (Alias != null)
            {
                var names = _databaseService.GetNamesOf(Alias.AsEntity());
                Names = new ObservableCollection<AliasNameModel>(names.AsModel());
            }
        }

        public void UpdateAlias()
        {
            _databaseService.Update(Alias.AsEntity());
            _databaseService.Update(Names.AsEntity());
            _userNotifyer.NotifyInfo("Alias updated!");
        }

        #endregion Methods
    }
}