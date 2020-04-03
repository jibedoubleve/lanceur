using MahApps.Metro.Controls;
using System.Windows.Input;

namespace Probel.Lanceur.Views
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : MetroWindow
    {
        #region Constructors

        public StatisticsView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { Close(); }
        }
    }
}