using MahApps.Metro.Controls;
using Probel.Lanceur.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Probel.Lanceur.Views
{
    /// <summary>
    /// Interaction logic for SetupView.xaml
    /// </summary>
    public partial class SetupView : MetroWindow
    {
        #region Constructors

        public SetupView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        private SetupViewModel ViewModel => DataContext as SetupViewModel;

        #endregion Properties

        #region Methods

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { Close(); }
        }

        private void OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var tab = e.AddedItems[0];
                if (tab == _tabKeywords) { ViewModel.ListAliasViewModel.RefreshData(); }
                else if (tab == _tabSettings) { ViewModel.SettingsViewModel.RefreshData(); }
                else if (tab == _tabSessions) { ViewModel.EditSessionViewModel.RefreshData(); }
                else if (tab == _tabPlugins) { ViewModel.EditPluginViewModel.RefreshData(); }
                else if (tab == _tabDoubloons) { ViewModel.EditDoubloonsViewModel.RefreshData(); }
            }
        }

        #endregion Methods
    }
}