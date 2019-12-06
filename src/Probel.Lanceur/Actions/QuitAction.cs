using System.Windows;

namespace Probel.Lanceur.Actions
{
    [UiAction]
    public class QuitAction : BaseUiAction
    {
        #region Methods

        protected override void Configure() { }

        protected override void DoExecute(string arg) => Application.Current.Shutdown();

        #endregion Methods
    }
}