using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.ViewModels;
using System.ComponentModel;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Add a new keyword")]
    public class AddAction : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {            
            if (KeywordLoader.Contains(arg)) { return ExecutionResult.SuccessHide; }

            using (DeactivateHotKey.During())
            {
                var vm = GetViewModel<SetupViewModel>();

                WindowManager.ShowWindow(vm);

                vm.ListAliasViewModel.CreateKeyword(arg);
                vm.SelectedTab = 0;
            }
            Log.Trace("Closed settings");
            return ExecutionResult.SuccessHide;
        }

        #endregion Methods
    }
}