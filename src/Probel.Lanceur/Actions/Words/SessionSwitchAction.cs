using Probel.Lanceur.Core.Services;
using System.ComponentModel;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction("switch"), Description("Switch session.")]
    public class SessionSwitchAction : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                Notifyer.NotifyWarning($"Cannot switch to an empty session!");
                return ExecutionResult.SuccessHide;
            }
            else
            {
                var s = DataService.GetSession(arg);

                if (s != null)
                {
                    var aps = SettingsService.Get();
                    aps.SessionId = s.Id;
                    SettingsService.Save(aps);

                    Log.Info($"Switched to Session '{s.Name}'");
                    Notifyer.NotifyInfo($"Switched to Session '{s.Name}'");
                }
                else { Notifyer.NotifyWarning($"Session '{arg}' does not exist."); }

                return ExecutionResult.SuccessHide;
            }
        }

        #endregion Methods
    }
}