using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : Window
    {
        #region Constructors

        public ImportView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Output.ScrollToEnd();
            e.Handled = true;
        }

        #endregion Methods
    }
}