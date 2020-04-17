using Probel.Lanceur.Plugin.Evernote.ViewModels;
using System.Windows.Controls;

namespace Probel.Lanceur.Plugin.Evernote.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        #region Constructors

        public SettingsView()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }

        #endregion Constructors

        #region Properties

        private SettingsViewModel ViewModel => DataContext as SettingsViewModel;

        #endregion Properties

        #region Methods

        private void OnSaveSettings(object sender, System.Windows.RoutedEventArgs e) => ViewModel.SaveSettings();

        #endregion Methods
    }
}