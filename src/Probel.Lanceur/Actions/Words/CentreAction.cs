using Caliburn.Micro;
using Probel.Lanceur.Services;
using System.ComponentModel;
using Unity;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction, Description("Centre the window in the middle of the screen")]
    public class CentreAction : BaseUiAction
    {
        #region Fields

        private IEventAggregator _eventManager;

        #endregion Fields

        #region Methods

        protected override void Configure() => _eventManager = Container.Resolve<IEventAggregator>();

        protected override void DoExecute(string arg) => _eventManager.PublishOnUIThread(UiEvent.CenterCommand);

        #endregion Methods
    }
}