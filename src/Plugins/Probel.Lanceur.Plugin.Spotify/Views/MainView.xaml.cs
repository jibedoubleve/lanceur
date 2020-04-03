using Probel.Lanceur.Plugin.Spotify.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Plugin.Spotify.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        #region Constructors

        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        private MainViewModel ViewModel => DataContext as MainViewModel;
        #endregion Constructors

        private void OnUserControlIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                Debug.WriteLine("===========> VISIBLE");
                ViewModel.Resume();
            }
            else
            {
                Debug.WriteLine("===========> INVISIBLE");
                ViewModel.Pause();
            }
        }
    }
}