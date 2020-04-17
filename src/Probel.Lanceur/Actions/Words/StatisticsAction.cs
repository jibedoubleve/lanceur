using Probel.Lanceur.Helpers;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.ViewModels;
using System.ComponentModel;
using Unity;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Shows usage statistics.")]
    internal class StatisticsAction : BaseUiAction
    {
        #region Methods

        protected override void DoExecute(string arg)
        {
            using (DeactivateHotKey.During())
            {
                WindowManager.ShowDialog(Container.Resolve<StatisticsViewModel>());
            }
            Container.Resolve<ILogService>().Trace("Closed settings");
        }

        #endregion Methods
    }
}