using System.Windows;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction]
    public class EchoAction : BaseUiAction
    {
        #region Methods

        protected override void DoExecute(string arg) => MessageBox.Show(arg, "Easter Egg", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        #endregion Methods
    }
}