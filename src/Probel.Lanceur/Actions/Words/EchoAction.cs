using Probel.Lanceur.Core.Services;
using System.ComponentModel;
using System.Windows;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("[DEBUG] Echoes a message using the internal notification facility.")]
    public class EchoAction : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {
            MessageBox.Show(arg, "Easter Egg", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return ExecutionResult.SuccessHide;
        }

        #endregion Methods
    }
}