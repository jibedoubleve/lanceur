using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Models;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Probel.Lanceur.ViewModels
{
    public class EditAliasViewModel : Screen
    {
        #region Fields

        private readonly IDataSourceService _databaseService;

        private AliasModel _alias;
        private bool _isCreation;
        private ObservableCollection<AliasNameModel> _names;
        private IUserNotifyer _userNotifyer;

        #endregion Fields

        #region Constructors

        public EditAliasViewModel(IDataSourceService databaseService, ILogService log, IUserNotifyer userNotifyer)
        {
            Log = log;
            UserNotifyer = userNotifyer;
            _databaseService = databaseService;
        }

        #endregion Constructors

        #region Properties

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

        public IUserNotifyer UserNotifyer
        {
            get => _userNotifyer;
            set => Set(ref _userNotifyer, value, nameof(UserNotifyer));
        }

        private ListAliasViewModel ParentVm => Parent as ListAliasViewModel;

        #endregion Properties

        #region Methods

        public bool CanDeleteAlias() => _alias != null;

        public bool CanUpdateAlias() => Alias != null;

        public void CreateAlias()
        {
            _databaseService.Create(Alias.AsEntity(), Names.AsNames());
            RefreshParentList();
            UserNotifyer.NotifyInfo("Alias created!");
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

        public System.Action OnRefresh { get; set; }

        public void UpdateAlias()
        {
            _databaseService.Update(Alias.AsEntity());

            foreach (var name in Names) { name.IdAlias = Alias.Id; }
            _databaseService.Update(Names.AsEntity(), Alias.Id);
            UserNotifyer.NotifyInfo("Alias updated!");
            OnRefresh?.Invoke();
        }

        private void RefreshParentList()
        {
            if (Parent is ListAliasViewModel vm) { vm.RefreshData(); }
        }

        #endregion Methods
    }
}