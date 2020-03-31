using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin.Calculator.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Probel.Lanceur.Plugin.Calculator.Views
{
    /// <summary>
    /// Interaction logic for ResultView.xaml
    /// </summary>
    public partial class ResultView : UserControl
    {
        #region Constructors

        public ResultView()
        {
            InitializeComponent();
            DataContext = new ResultViewModel();
        }

        #endregion Constructors

        #region Properties

        private ILogService Log => ViewModel.Log;
        private ResultViewModel ViewModel => DataContext as ResultViewModel;

        #endregion Properties

        #region Methods


        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (e.Key == Key.Enter) { ViewModel.Process(tb.Text); }
            }
        }

        #endregion Methods
    }
}