using System.Windows;

namespace Probel.Lanceur.Actions
{
    [UiAction]
    public class QuitAction : BaseUiAction
    {
        #region Methods

        public override void Execute(string arg) => Application.Current.Shutdown();

        #endregion Methods
    }
}