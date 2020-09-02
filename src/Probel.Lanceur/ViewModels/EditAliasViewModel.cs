using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Models;
using Probel.Lanceur.SharedKernel.Logs;
using Probel.Lanceur.SharedKernel.UserCom;
using Probel.UwpHelpers;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace Probel.Lanceur.ViewModels
{
    public class EditAliasViewModel : Screen
    {
        #region Fields

        private readonly string _currentUserId = WindowsIdentity.GetCurrent().User.Value;
        private readonly IDataSourceService _databaseService;

        private readonly UwpAppFactory _uwpFactory;
        private AliasModel _alias;
        private bool _isCreation;
        private ObservableCollection<AliasNameModel> _names;
        private IUserNotifyer _userNotifyer;

        #endregion Fields

        #region Constructors

        public EditAliasViewModel(IDataSourceService databaseService, ILogService log, IUserNotifyerFactory factory)
        {
            Log = log;
            UserNotifyer = factory.Get();
            _databaseService = databaseService;
            _uwpFactory = new UwpAppFactory();
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

        public System.Action OnRefresh { get; set; }

        public IUserNotifyer UserNotifyer
        {
            get => _userNotifyer;
            set => Set(ref _userNotifyer, value, nameof(UserNotifyer));
        }

        #endregion Properties

        #region Methods

        private void RefreshAlias(Package package)
        {
            var pack = _uwpFactory.Create(package);
            Alias.FileName = $"package:{pack.UniqueIdentifier}";
            Alias.IsPackage = true;
            Alias.UniqueIdentifyer = pack.UniqueIdentifier;
            Alias.Icon = pack.LogoPath;
        }

        private void RefreshParentList()
        {
            if (Parent is ListAliasViewModel vm) { vm.RefreshData(); }
        }

        public bool CanDeleteAlias() => _alias != null;

        public bool CanUpdateAlias() => Alias != null;

        public void CreateAlias()
        {
            if (_uwpFactory.IsUwp(_currentUserId, Alias.FileName, out Package package))
            {
                RefreshAlias(package);
            }

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

        public void UpdateAlias()
        {
            if (_uwpFactory.IsUwp(_currentUserId, Alias.FileName, out Package package))
            {
                RefreshAlias(package);
            }

            _databaseService.Update(Alias.AsEntity());

            foreach (var name in Names) { name.IdAlias = Alias.Id; }
            _databaseService.Update(Names.AsEntity(), Alias.Id);
            UserNotifyer.NotifyInfo("Alias updated!");
            OnRefresh?.Invoke();
        }

        #endregion Methods
    }
}