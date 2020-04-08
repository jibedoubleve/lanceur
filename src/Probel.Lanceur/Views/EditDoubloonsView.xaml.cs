﻿using Probel.Lanceur.ViewModels;
using System.Windows.Controls;

namespace Probel.Lanceur.Views
{
    /// <summary>
    /// Interaction logic for EditDoubloonsView.xaml
    /// </summary>
    public partial class EditDoubloonsView : UserControl
    {
        #region Constructors

        public EditDoubloonsView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        private EditDoubloonsViewModel ViewModel => DataContext as EditDoubloonsViewModel;

        #endregion Properties

        #region Methods

        private void OnDeleteDoubloon(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is long id)
            {
                ViewModel.DeleteCurrent(id);
            }
        }

        #endregion Methods
    }
}