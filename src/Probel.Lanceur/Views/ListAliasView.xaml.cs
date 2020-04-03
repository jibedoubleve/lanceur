using Probel.Lanceur.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Probel.Lanceur.Views
{
    /// <summary>
    /// Interaction logic for ListAliasView.xaml
    /// </summary>
    public partial class ListAliasView : UserControl
    {
        #region Constructors

        public ListAliasView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        private ListAliasViewModel ViewModel => DataContext as ListAliasViewModel;

        #endregion Properties

        #region Methods

        private void OnIsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible) { FocusSearchBar(); }
        }

        private void FocusSearchBar()
        {
            _tbSearch.Focus();
            Keyboard.Focus(_tbSearch);
        }

        private void OnSearchKeyDown(object sender, KeyEventArgs e)
        {
            // https://stackoverflow.com/questions/5750722/how-to-detect-modifier-key-states-in-wpf
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.F) { FocusSearchBar(); }
            }
            if (e.Key == Key.Enter)
            {
                ViewModel.Search(_tbSearch.Text);
            }
        }

        #endregion Methods
    }
}