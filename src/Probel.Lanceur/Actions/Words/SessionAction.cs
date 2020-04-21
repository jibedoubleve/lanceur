using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System.ComponentModel;
using Unity;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction("switch"), Description("Switch session.")]
    public class SessionAction : BaseUiAction
    {
        #region Methods

        protected override void DoExecute(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                Notifyer.NotifyWarning($"Cannot switch to an empty session!");
                return;
            }
            else
            {
                var settings = Container.Resolve<ISettingsService>();

                var s = DataService.GetSession(arg);

                if (s != null)
                {
                    var aps = settings.Get();
                    aps.SessionId = s.Id;
                    settings.Save(aps);

                    Log.Info($"Switched to Session '{s.Name}'");
                    Notifyer.NotifyInfo($"Switched to Session [{s.Id}] - {s.Name}");
                }
                else { Notifyer.NotifyWarning($"Session '{arg}' does not exist."); }
            }
        }

        #endregion Methods
    }
}