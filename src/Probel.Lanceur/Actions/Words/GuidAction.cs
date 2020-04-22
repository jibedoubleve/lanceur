using Probel.Lanceur.Core.Services;
using System;
using System.ComponentModel;
using System.Windows;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Save a new GUID into the clipboard.")]
    public class GuidAction : BaseUiAction
    {
        #region Methods

        protected override ExecutionResult DoExecute(string arg)
        {
            var guid = Guid.NewGuid();
            Clipboard.SetText(guid.ToString());
            return ExecutionResult.SuccessHide;
        }

        #endregion Methods
    }
}