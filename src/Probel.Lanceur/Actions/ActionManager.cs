using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Infrastructure;
using System;

namespace Probel.Lanceur.Actions
{
    internal class ActionManager : IActionManager
    {
        #region Fields

        public readonly ILogService _logger;
        private readonly IActionCollection _actions;
        private readonly IActionContext _context;
        private readonly IKeywordService _keywordService;

        #endregion Fields

        #region Constructors

        public ActionManager(IKeywordService keywordService, IActionCollection actions, IActionContext context)
        {
            _context = context;
            _actions = actions;
            _logger = _context.Log;
            _keywordService = keywordService;
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

                _keywordService.Bind(actionName, arg => action.With(_context)
                                                                      .Execute(arg));
            }
        }

        #endregion Methods
    }
}