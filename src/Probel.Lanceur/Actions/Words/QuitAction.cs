using Probel.Lanceur.Core.Services;
using System.ComponentModel;
using System.Windows;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Quits the application.")]
    public class QuitAction : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {
            Application.Current.Shutdown();
            return ExecutionResult.SuccessHide;
        }

        #endregion Methods
    }
}