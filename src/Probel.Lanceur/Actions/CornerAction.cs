using Caliburn.Micro;
using Probel.Lanceur.Services;
using Unity;

namespace Probel.Lanceur.Actions
{
    [UiAction]
    public class CornerAction : BaseUiAction
    {
        #region Fields

        private IEventAggregator _eventManager;

        #endregion Fields

        #region Methods

        protected override void Configure()
        {
            _eventManager = Container.Resolve<IEventAggregator>();
        }

        public override void Execute(string arg) => _eventManager.PublishOnUIThread(Notifications.CornerCommand);

        #endregion Methods
    }
}