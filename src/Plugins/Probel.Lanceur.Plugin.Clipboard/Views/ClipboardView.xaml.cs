using Probel.Lanceur.Plugin.Clipboard.Models;
using Probel.Lanceur.Plugin.Clipboard.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Plugin.Clipboard.Views
{
    /// <summary>
    /// Interaction logic for ClipboardView.xaml
    /// </summary>
    public partial class ClipboardView : UserControl
    {
        #region Constructors

        public ClipboardView()
        {
            InitializeComponent();
            DataContext = new ClipboardViewModel();
        }

        #endregion Constructors

        private ClipboardViewModel ViewModel => DataContext as ClipboardViewModel;
        private void OnCopyInClipboard(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.Tag is ClipboardItem item)
            {
                System.Windows.Clipboard.SetText(item.Text);
            }
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.Tag is ClipboardItem item)
            {
                ViewModel.Delete(item);
                ViewModel.Load();
            }
        }
    }
}