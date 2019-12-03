using System.Windows;

namespace Probel.Lanceur.Actions
{
    [UiAction]
    public class EchoAction : BaseUiAction
    {
        #region Methods

        public override void Execute(string arg) => MessageBox.Show(arg, "Easter Egg", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        #endregion Methods
    }
}