using Caliburn.Micro;
using System.Reflection;

namespace Probel.Lanceur.ViewModels
{
    public class SetupViewModel : Screen
    {
        #region Fields

        private string _appVersion;
        private EditDoubloonsViewModel _editDoubloonsViewModel;
        private EditPluginViewModel _editPluginViewModel;
        private EditSessionViewModel _editSessionViewModel;
        private ListAliasViewModel _listAliasViewModel;

        private int _selectedTab;
        private SettingsViewModel _setingsViewModel;

        #endregion Fields

        #region Constructors

        public SetupViewModel(ListAliasViewModel listAliasViewModel,
            SettingsViewModel settingsViewModel,
            EditSessionViewModel editSessionViewModel,
            EditDoubloonsViewModel editDoubloonsViewModel,
            EditPluginViewModel editPluginViewModel)
        {
            EditDoubloonsViewModel = editDoubloonsViewModel;
            EditPluginViewModel = editPluginViewModel;
            EditSessionViewModel = editSessionViewModel;
            ListAliasViewModel = listAliasViewModel;
            SettingsViewModel = settingsViewModel;

            var v = Assembly.GetExecutingAssembly().GetName().Version;
            AppVersion = $"v.{v.Major}.{v.Minor}.{v.Build}";
        }

        #endregion Constructors

        #region Properties

        public static bool IsBusy { get; internal set; }

        public string AppVersion
        {
            get => _appVersion;
            set => Set(ref _appVersion, value, nameof(AppVersion));
        }

        public EditDoubloonsViewModel EditDoubloonsViewModel
        {
            get => _editDoubloonsViewModel;
            set => Set(ref _editDoubloonsViewModel, value, nameof(EditDoubloonsViewModel));
        }

        public EditPluginViewModel EditPluginViewModel
        {
            get => _editPluginViewModel;
            set => Set(ref _editPluginViewModel, value, nameof(EditPluginViewModel));
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

        public int SelectedTab
        {
            get => _selectedTab;
            set => Set(ref _selectedTab, value, nameof(SelectedTab));
        }

        public SettingsViewModel SettingsViewModel
        {
            get => _setingsViewModel;
            set => Set(ref _setingsViewModel, value, nameof(SettingsViewModel));
        }

        #endregion Properties
    }
}