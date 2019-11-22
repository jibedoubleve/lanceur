using Caliburn.Micro;
using System.Reflection;

namespace Probel.Lanceur.ViewModels
{
    public class SetupViewModel : Screen
    {
        #region Fields

        private string _appVersion;
        private EditSessionViewModel _editSessionViewModel;
        private ListShortcutViewModel _listShortcutViewModel;

        private SettingsViewModel _setingsViewModel;

        #endregion Fields

        #region Constructors

        public SetupViewModel(ListShortcutViewModel listShortcutViewModel, SettingsViewModel settingsViewModel, EditSessionViewModel editSessionViewModel)
        {
            EditSessionViewModel = editSessionViewModel;
            ListShortcutViewModel = listShortcutViewModel;
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

        public ListShortcutViewModel ListShortcutViewModel
        {
            get => _listShortcutViewModel;
            set => Set(ref _listShortcutViewModel, value, nameof(ListShortcutViewModel));
        }

        public SettingsViewModel SettingsViewModel
        {
            get => _setingsViewModel;
            set => Set(ref _setingsViewModel, value, nameof(SettingsViewModel));
        }

        #endregion Properties
    }
}