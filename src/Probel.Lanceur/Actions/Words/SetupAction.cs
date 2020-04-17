using Probel.Lanceur.Helpers;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.ViewModels;
using System.ComponentModel;
using Unity;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Opens the settings.")]
    public class SetupAction : BaseUiAction
    {
        #region Methods

        protected override void DoExecute(string arg)
        {
            using (DeactivateHotKey.During())
            {
                WindowManager.ShowDialog(Container.Resolve<SetupViewModel>());
            }
            Container.Resolve<ILogService>().Trace("Closed settings");
        }

        #endregion Methods
    }
}