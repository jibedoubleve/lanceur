using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Probel.Lanceur.ViewModels
{
    public class EditShortcutViewModel : Screen
    {
        #region Fields

        private readonly IDatabaseService _databaseService;
        private bool _isCreation;
        private ObservableCollection<ShortcutNameModel> _names;
        private ShortcutModel _shortcut;

        #endregion Fields

        #region Constructors

        public EditShortcutViewModel(IDatabaseService databaseService, ILogService log)
        {
            Log = log;
            _databaseService = databaseService;
        }

        #endregion Constructors

        #region Properties

        private ListShortcutViewModel ParentVm => Parent as ListShortcutViewModel;

        public bool IsCreation
        {
            get => _isCreation;
            set => Set(ref _isCreation, value, nameof(IsCreation));
        }

        public ILogService Log { get; }

        public ObservableCollection<ShortcutNameModel> Names
        {
            get => _names;
            set => Set(ref _names, value, nameof(Names));
        }

        public ShortcutModel Shortcut
        {
            get => _shortcut;
            set
            {
                if (Set(ref _shortcut, value, nameof(Shortcut)))
                {
                    IsCreation = value.Id > 0;
                    NotifyOfPropertyChange(nameof(IsCreation));
                }
            }
        }

        #endregion Properties

        #region Methods

        private void RefreshParentList()
        {
            if (Parent is ListShortcutViewModel vm) { vm.RefreshData(); }
        }

        public bool CanDeleteShortcut() => _shortcut != null;

        public bool CanUpdateShortcut() => Shortcut != null;

        public void CreateShortcut()
        {
            _databaseService.Create(Shortcut.AsEntity());
            RefreshParentList();
        }

        public async Task DeleteShortcutAsync()
        {
            if (await ParentVm.AskForDeletion(Shortcut.Name))
            {
                _databaseService.Delete(Shortcut.AsEntity());
                RefreshParentList();
            }
        }

        public void RefreshData(ShortcutModel model = null)
        {
            if (model != null) { Shortcut = model; }
            if (Shortcut != null)
            {
                var names = _databaseService.GetNamesOf(Shortcut.AsEntity());
                Names = new ObservableCollection<ShortcutNameModel>(names.AsModel());
            }
        }

        public void UpdateShortcut()
        {
            _databaseService.Update(Shortcut.AsEntity());
            _databaseService.Update(Names.AsEntity());
        }

        #endregion Methods
    }
}