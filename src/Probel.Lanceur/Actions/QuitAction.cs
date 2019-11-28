using System.Windows;
using Unity;

namespace Probel.Lanceur.Actions
{
    public class QuitAction : BaseUiAction
    {
        #region Constructors

        public QuitAction(IUnityContainer container) : base(container)
        {
        }

        #endregion Constructors

        #region Methods

        public override void Execute(string arg) => Application.Current.Shutdown();

        #endregion Methods
    }
}