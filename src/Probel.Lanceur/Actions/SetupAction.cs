using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.ViewModels;
using Unity;

namespace Probel.Lanceur.Actions
{
    public class SetupAction : BaseUiAction
    {
        #region Constructors

        public SetupAction(IUnityContainer container) : base(container)
        {
        }

        #endregion Constructors

        #region Methods

        public override void Execute(string arg)
        {
            using (DeactivateHotKey.During())
            {
                WindowManager.ShowDialog(Container.Resolve<SetupViewModel>());
            }
            Container.Resolve<ILogService>().Trace("Closed settings");
        }

        #endregion Methods
    }
}