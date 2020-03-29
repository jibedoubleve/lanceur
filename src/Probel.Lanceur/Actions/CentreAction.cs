using Caliburn.Micro;
using Probel.Lanceur.Services;
using Unity;

namespace Probel.Lanceur.Actions
{
    [UiAction]
    public class CentreAction : BaseUiAction
    {
        #region Fields

        private IEventAggregator _eventManager;

        #endregion Fields

        #region Methods

        protected override void Configure() => _eventManager = Container.Resolve<IEventAggregator>();

        protected override void DoExecute(string arg) => _eventManager.PublishOnUIThread(Notifications.CenterCommand);

        #endregion Methods
    }
}