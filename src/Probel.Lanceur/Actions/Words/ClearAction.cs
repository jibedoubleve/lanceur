using Probel.Lanceur.Core.Services;
using System;
using System.ComponentModel;
using System.Windows;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Clears the database. (This action cannot be undone)")]
    public class ClearAction : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {
            var nl = Environment.NewLine;
            var result = MessageBox.Show($"Do you want to erase all data from database?{nl}This action cannot be undone!", "QUESTION", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes) { DataService.Clear(); }

            return ExecutionResult.SuccessHide;
        }

        #endregion Methods
    }
}