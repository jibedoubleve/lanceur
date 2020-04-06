using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Views
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
        }

        #endregion Constructors

        #region Methods

        private void OnSelectDbClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                _tbPath.Text = ofd.FileName;
            }
        }

        #endregion Methods
    }
}