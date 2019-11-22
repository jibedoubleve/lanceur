using MahApps.Metro.IconPacks;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Probel.Lanceur.Views
{
    /// <summary>
    /// Interaction logic for EditShortcutView.xaml
    /// </summary>
    public partial class EditShortcutView : UserControl
    {
        #region Constructors

        public EditShortcutView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        private EditShortcutViewModel ViewModel => DataContext as EditShortcutViewModel;

        #endregion Properties

        #region Methods

        private void OnProcess()
        {
            try
            {
                var ps = ProcessHelper.GetExecutablePath();
                ViewModel.Shortcut.FileName = ps.FileName;
            }
            catch (Exception ex) { ViewModel.Log.Warning($"Error occured while trying to find process under mouse.", ex); }
        }

        private void OnProcessFinderMouseMouve(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is PackIconMaterial ctrl)
            {
                DragDrop.DoDragDrop(ctrl, new object(), DragDropEffects.None);
                OnProcess();
            }
        }

        #endregion Methods
    }
}