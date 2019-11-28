using System.Windows;
using Unity;

namespace Probel.Lanceur.Actions
{
    public class EchoAction : BaseUiAction
    {
        #region Constructors

        public EchoAction(IUnityContainer container) : base(container)
        {
        }

        #endregion Constructors

        #region Methods

        public override void Execute(string arg) => MessageBox.Show(arg, "Easter Egg", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        #endregion Methods
    }
}