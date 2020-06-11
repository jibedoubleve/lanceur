using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.ViewModels;
using System.ComponentModel;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Opens the settings.")]
    public class SetupAction : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {
            using (DeactivateHotKey.During())
            {
                WindowManager.ShowDialog(GetViewModel<SetupViewModel>());
            }
            Log.Trace("Closed settings");

            return ExecutionResult.SuccessHide;
        }

        #endregion Methods
    }
}