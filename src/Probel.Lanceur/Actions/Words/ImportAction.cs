using Probel.Lanceur.Core.Services;
using Probel.Lanceur.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Import the keywords of Slickrun into the database.")]
    public class ImportAction : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {
            var vm = GetViewModel<ImportViewModel>();
            WindowManager.ShowWindow(vm);
            EventHandler<ImportUpdatedEventArg> update = (sender, e) =>
            {
                Application.Current.Dispatcher.Invoke(() => vm.Update(e.Progress, e.Output));
            };

            try
            {

                SlickRunImporterService.ImportUpdated += update;

                var sessionId = SlickRunImporterService.Import();

                //Now update the settings
                var s = SettingsService.Get();
                s.SessionId = sessionId;
                SettingsService.Save(s);
                return ExecutionResult.SuccessHide;
            }
            catch (Exception ex)
            {
                return ExecutionResult.Failure(ex.Message);
            }
            finally
            {
                SlickRunImporterService.ImportUpdated -= update;
                vm.TryClose();
            }
        }

        #endregion Methods
    }
}