using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.ViewModels;
using Unity;

namespace Probel.Lanceur.Actions
{
    [UiAction]
    public class SetupAction : BaseUiAction
    {
        #region Methods

        public override void Execute(string arg)
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