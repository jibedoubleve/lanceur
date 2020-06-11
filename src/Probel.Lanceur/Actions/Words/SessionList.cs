using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using System.ComponentModel;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction("sessions"), Description("List sessions.")]
    public class SessionList : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {
            var sessions = DataService.GetSessions().AsSwitchSessionResult();

            foreach (var s in sessions) { s.Icon = "FormatListNumbers"; }

            ViewFinder.GetViewModel().SetResult(sessions, keepalive: true);
            return ExecutionResult.SuccesShow;
        }

        #endregion Methods
    }
}