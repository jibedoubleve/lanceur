using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.ViewModels;
using Unity;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction]
    internal class StatisticsAction : BaseUiAction
    {
        protected override void DoExecute(string arg)
        {
            using (DeactivateHotKey.During())
            {
                WindowManager.ShowDialog(Container.Resolve<StatisticsViewModel>());
            }
            Container.Resolve<ILogService>().Trace("Closed settings");
        }
    }
}