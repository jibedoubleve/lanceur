using Caliburn.Micro;
using Probel.Lanceur.Services;
using Unity;

namespace Probel.Lanceur.Actions
{
    public class CornerAction : BaseUiAction
    {
        #region Fields

        private readonly IEventAggregator _eventManager;

        #endregion Fields

        #region Constructors

        public CornerAction(IUnityContainer container) : base(container)
        {
            _eventManager = Container.Resolve<IEventAggregator>();
        }

        #endregion Constructors

        #region Methods

        public override void Execute(string arg) => _eventManager.PublishOnUIThread(Notifications.CornerCommand);


#endregion Methods
    }
}