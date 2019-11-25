using Caliburn.Micro;
using System.Reflection;

namespace Probel.Lanceur.ViewModels
{
    public class SetupViewModel : Screen
    {
        #region Fields

        private string _appVersion;
        private EditSessionViewModel _editSessionViewModel;
        private ListAliasViewModel _listAliasViewModel;

        private SettingsViewModel _setingsViewModel;

        #endregion Fields

        #region Constructors

        public SetupViewModel(ListAliasViewModel listAliasViewModel, SettingsViewModel settingsViewModel, EditSessionViewModel editSessionViewModel)
        {
            EditSessionViewModel = editSessionViewModel;
            ListAliasViewModel = listAliasViewModel;
            SettingsViewModel = settingsViewModel;

            var v = Assembly.GetExecutingAssembly().GetName().Version;
            AppVersion = $"v.{v.Major}.{v.Minor}.{v.Build}";
        }

        #endregion Constructors

        #region Properties

        public string AppVersion
        {
            get => _appVersion;
            set => Set(ref _appVersion, value, nameof(AppVersion));
        }

        public EditSessionViewModel EditSessionViewModel
        {
            get => _editSessionViewModel;
            set => Set(ref _editSessionViewModel, value, nameof(EditSessionViewModel));
        }

        public ListAliasViewModel ListAliasViewModel
        {
            get => _listAliasViewModel;
            set => Set(ref _listAliasViewModel, value, nameof(ListAliasViewModel));
        }

        public SettingsViewModel SettingsViewModel
        {
            get => _setingsViewModel;
            set => Set(ref _setingsViewModel, value, nameof(SettingsViewModel));
        }

        #endregion Properties
    }
}