using Probel.Lanceur.Core.Services;
using System;
using System.Linq;
using System.Reflection;
using Unity;

namespace Probel.Lanceur.Actions
{
    internal class ActionManager : IActionManager
    {
        #region Fields

        public readonly ILogService _logger;
        private readonly IUnityContainer _container;
        private readonly IReservedKeywordService _reservedKeywordService;

        #endregion Fields

        #region Constructors

        public ActionManager(IReservedKeywordService reservedKeywordService, IUnityContainer container)
        {
            _container = container;

            _logger = _container.Resolve<ILogService>();
            _reservedKeywordService = reservedKeywordService;
        }

        #endregion Constructors

        #region Methods

        public void Bind()
        {
            var types = from t in Assembly.GetAssembly(typeof(IUiAction)).GetTypes()
                        where t.GetCustomAttribute<UiActionAttribute>() != null
                        select t;
            foreach (var type in types)
            {
                var actionName = GetActionName(type).ToUpper();
                _logger.Trace($"Found type '{type.Name}' for action '{actionName}'");

                var action = (IUiAction)Activator.CreateInstance(type);

                _reservedKeywordService.Bind(actionName, arg => action.With(_container).Execute(arg));
            }
        }

        private string GetActionName(Type type)
        {
            var name = type.Name.Replace("Action", "");
            return name;
        }

        #endregion Methods
    }
}