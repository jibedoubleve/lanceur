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

        private void OnSearchKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.Search(_tbSearch.Text);
            }
        }

        #endregion Methods
    }
}