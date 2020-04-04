﻿using Caliburn.Micro;
using Probel.Lanceur.Services;
using Unity;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction]
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