using System;
using System.ComponentModel;
using System.Windows;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Save a new GUID into the clipboard.")]
    public class GuidAction : BaseUiAction
    {
        #region Methods

        protected override void DoExecute(string arg)
        {
            var guid = Guid.NewGuid();
            Clipboard.SetText(guid.ToString());
        }

        #endregion Methods
    }
}