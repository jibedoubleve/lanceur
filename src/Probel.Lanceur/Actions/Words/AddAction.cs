using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.ViewModels;
using System.ComponentModel;
using Unity;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Add a new keyword")]
    public class AddAction : BaseUiAction
    {
        #region Methods

        protected override void DoExecute(string arg)
        {
            var keywords = Container.Resolve<IKeywordLoader>();
            if (keywords.Contains(arg)) { return; }

            using (DeactivateHotKey.During())
            {
                var vm = Container.Resolve<SetupViewModel>();

                WindowManager.ShowWindow(vm);

                vm.ListAliasViewModel.CreateKeyword(arg);
                vm.SelectedTab = 0;
            }
            Container.Resolve<ILogService>().Trace("Closed settings");
        }

        #endregion Methods
    }
}