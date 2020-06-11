using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.ViewModels;
using System.ComponentModel;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Shows usage statistics.")]
    internal class StatisticsAction : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {
            using (DeactivateHotKey.During())
            {
                WindowManager.ShowDialog(GetViewModel<StatisticsViewModel>());
            }
            Log.Trace("Closed settings");
            return ExecutionResult.SuccessHide;
        }

        #endregion Methods
    }
}