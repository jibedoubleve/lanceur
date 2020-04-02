using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.ViewModels;
using Unity;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction]
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