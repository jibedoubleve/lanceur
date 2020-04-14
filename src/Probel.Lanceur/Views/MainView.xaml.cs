using NHotkey;
using NHotkey.Wpf;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Events;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Probel.Lanceur.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window, IPluginView
    {
        #region Fields

        private bool _canSavePosition = false;

        #endregion Fields

        #region Constructors

        public MainView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        private MainViewModel ViewModel => DataContext as MainViewModel;

        #endregion Properties

        #region Methods

        public void HideResults()
        {
            Results.Visibility = Visibility.Collapsed;
            PluginArea.Visibility = Visibility.Visible;
        }

        public void SetPluginArea(object area)
        {
            PluginArea.Content = area; ;
        }

        public void ShowResults()
        {
            Results.Visibility = Visibility.Visible;
            PluginArea.Visibility = Visibility.Collapsed;
        }

        protected override void OnDeactivated(EventArgs e)
        {
#if !DEBUG
            HideControl();
#endif
        }

        private void HideControl()
        {
            ShowResults();
            AliasTextBox.Text = string.Empty;
            _self.Visibility = Visibility.Collapsed;
            ViewModel.SaveSettings();
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var a = (Results.SelectedItem as AliasText ?? new AliasText()).Name;
                var b = AliasTextBox.Text;
                var result = ViewModel?.ExecuteText(a, b) ?? ExecutionResult.Failure;

                if (!result.KeepShowing) { HideControl(); }
                if (result.IsError) { ViewModel.IsOnError = true; }

                e.Handled = true;
            }
            else if (e.Key == Key.Escape) { HideControl(); }
            else if (e.Key == Key.Up) { Results.SelectNextItem(); }
            else if (e.Key == Key.Down) { Results.SelectPreviousItem(); }
        }

        private void OnKeyPressedWindow(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { HideControl(); }
        }

        private void OnResultsAliasDoubleClicked(object sender, AliasTextEventArgs e)
        {
            var result = ViewModel?.ExecuteText(e.Alias.Name) ?? ExecutionResult.Failure;
            if (!result.IsError) { HideControl(); }
        }

        private void OnShowWindow(object sender, HotkeyEventArgs e)
        {
            if (!SetupViewModel.IsBusy)
            {
                ViewModel.IsOnError = false;
                ShowWindow();
                e.Handled = true;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)

        {
            ViewModel.IsOnError = false;
            ViewModel.RefreshAliases(AliasTextBox.Text);
            Results.SelectFirst();
        }

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
                var msg = $"NHotkey: key already binded!{Environment.NewLine}Default binding is 'SHIFT+WINDOWS+R'";
                ViewModel.LogService.Warning(msg);
                ViewModel.Notifyer.NotifyWarning(msg);

                var key = Key.R;
                var mod = ModifierKeys.Shift | ModifierKeys.Windows;
                HotkeyManager.Current.AddOrReplace("OnShowWindow", key, mod, OnShowWindow);
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
            ViewModel.LoadAliases();

            Visibility = Visibility.Visible;
            AliasTextBox.Focus();

            //https://stackoverflow.com/questions/3109080/focus-on-textbox-when-usercontrol-change-visibility
            Dispatcher.BeginInvoke((Action)delegate { Keyboard.Focus(AliasTextBox); });

            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        #endregion Methods
    }
}