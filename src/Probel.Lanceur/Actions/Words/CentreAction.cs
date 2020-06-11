using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Services;
using System.ComponentModel;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Centre the window in the middle of the screen")]
    public class CentreAction : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {
            EventAggregator.PublishOnUIThread(UiEvent.CenterCommand);
            return ExecutionResult.SuccessHide;
        }

        #endregion Methods
    }
}