using System.Windows;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction]
    public class QuitAction : BaseUiAction
    {
        #region Methods

        protected override void DoExecute(string arg) => Application.Current.Shutdown();

        #endregion Methods
    }
}