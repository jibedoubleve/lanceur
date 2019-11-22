using NHotkey;
using NHotkey.Wpf;
using Probel.Lanceur.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Probel.Lanceur.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        #region Fields

        private bool _canSavePosition = false;

        #endregion Fields

        #region Constructors

        public MainView() => InitializeComponent();

        #endregion Constructors

        #region Properties

        private MainViewModel ViewModel => DataContext as MainViewModel;

        #endregion Properties

        #region Methods

        private void HideControl()
        {
            ShortcutNameList.Text = string.Empty;
            _self.Visibility = Visibility.Collapsed;
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel?.ExecuteText(ShortcutNameList.Text);
                HideControl();
            }
            else if (e.Key == Key.Escape) { HideControl(); }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e) => Visibility = Visibility.Collapsed;

        private void OnShowWindow(object sender, HotkeyEventArgs e) => ShowWindow();

        private void OnWindowClosing(object sender, CancelEventArgs e) => ViewModel.SaveSettings();

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var key = ViewModel.AppSettings.HotKey.Key;
                var mod = ViewModel.AppSettings.HotKey.ModifierKeys;
                HotkeyManager.Current.AddOrReplace("OnShowWindow", (Key)key, (ModifierKeys)mod, OnShowWindow);
            }
            catch (HotkeyAlreadyRegisteredException)
            {
                ViewModel.LogService.Warning("NHotkey: key already binded!");
            }

            ShowWindow();

            Left = ViewModel.Left;
            Top = ViewModel.Top;

            _canSavePosition = true;
        }

        private void OnWindowLocationChanged(object sender, EventArgs e)
        {
            if (_canSavePosition) { SavePosition(); }
        }

        private void OnWindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }

        private void SavePosition()
        {
            ViewModel.Left = Left;
            ViewModel.Top = Top;
        }

        private void ShowWindow()
        {
            ViewModel.LoadSettings();
            ViewModel.LoadShortcuts();

            Visibility = Visibility.Visible;
            ShortcutNameList.Focus();

            //https://stackoverflow.com/questions/3109080/focus-on-textbox-when-usercontrol-change-visibility
            Dispatcher.BeginInvoke((Action)delegate { Keyboard.Focus(ShortcutNameList); });
        }

        #endregion Methods
    }
}