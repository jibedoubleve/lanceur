using Probel.Lanceur.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Views
{
    /// <summary>
    /// Interaction logic for EditEmptyKeywordsView.xaml
    /// </summary>
    public partial class EditObsoleteKeywordsView : UserControl
    {
        #region Constructors

        public EditObsoleteKeywordsView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        private EditObsoleteKeywordsViewModel ViewModel => DataContext as EditObsoleteKeywordsViewModel;

        #endregion Properties

        #region Methods

        private void OnDeleteDoubloon(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is long id)
            {
                ViewModel.DeleteCurrent(id);
            }
        }

        #endregion Methods
    }
}