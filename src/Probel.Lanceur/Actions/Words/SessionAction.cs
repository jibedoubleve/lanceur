using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System.ComponentModel;
using Unity;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction("chs"), Description("Changes the current session.")]
    public class SessionAction : BaseUiAction
    {
        #region Methods

        protected override void DoExecute(string arg)
        {
            var settings = Container.Resolve<ISettingsService>();

            var s = DataService.GetSession(arg) ?? new AliasSession();

            var aps = settings.Get();
            aps.SessionId = s.Id;
            settings.Save(aps);

            Log.Info($"Switched to Session [{s.Id}] - {s.Name}");
        }

        #endregion Methods
    }
}