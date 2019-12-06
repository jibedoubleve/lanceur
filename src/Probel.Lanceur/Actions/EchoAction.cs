using System.Windows;

namespace Probel.Lanceur.Actions
{
    [UiAction]
    public class EchoAction : BaseUiAction
    {
        #region Methods

        protected override void Configure() { }

        protected override void DoExecute(string arg) => MessageBox.Show(arg, "Easter Egg", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        #endregion Methods
    }
}