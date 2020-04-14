using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin;
using System;
using Unity;

namespace Probel.Lanceur.Actions
{
    internal class ActionManager : IActionManager
    {
        #region Fields

        public readonly ILogService _logger;
        private readonly IActionCollection _actions;
        private readonly IUnityContainer _container;
        private readonly IDataSourceService _dataSource;
        private readonly IReservedKeywordService _reservedKeywordService;

        #endregion Fields

        #region Constructors

        public ActionManager(IReservedKeywordService reservedKeywordService, IUnityContainer container, IActionCollection actions, IDataSourceService dataSource)
        {
            _dataSource = dataSource;
            _actions = actions;
            _container = container;

            _logger = _container.Resolve<ILogService>();
            _reservedKeywordService = reservedKeywordService;
        }

        #endregion Constructors

        #region Methods

        public void Bind()
        {
            foreach (var a in _actions)
            {
                var actionName = a.Name;
                _logger.Trace($"Found type '{a.Type.Name:25}' for action '{actionName}'");

                var action = (IUiAction)Activator.CreateInstance(a.Type);

                _reservedKeywordService.Bind(actionName, arg => action.With(_container, _dataSource, _logger)
                                                                      .Execute(arg));
            }
        }

        #endregion Methods
    }
}