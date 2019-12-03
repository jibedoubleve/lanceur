using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity;

namespace Probel.Lanceur.Actions
{
    internal class ActionManager
    {
        #region Fields

        private readonly IUnityContainer _container;

        private readonly IReservedKeywordService _reservedKeywordService;

        public readonly ILogService _logger;

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

        private static IEnumerable<Keywords> GetKeyword(string name)
        {
            return (from a in Enum.GetValues(typeof(Keywords)).Cast<Keywords>()
                    where a.ToString().ToLower() == name.ToLower()
                    select a);
        }

        private Keywords GetActionName(Type type)
        {
            var name = type.Name.Replace("Action", "");
            var action = GetKeyword(name);

            if (action.Count() == 0)
            {
                var r = GetKeyword(type.GetCustomAttribute<UiActionAttribute>().Action);
                if (r.Count() > 0) { return r.First(); }
                else { throw new NotSupportedException($"The action '{name}' does not exist or is not supported. Did you forget to add it in the enum '{typeof(Keywords)}'?"); }
            }
            else { return action.First(); }
        }

        public void Bind()
        {
            var types = from t in Assembly.GetAssembly(typeof(IUiAction)).GetTypes()
                        where t.GetCustomAttribute<UiActionAttribute>() != null
                        select t;
            foreach (var type in types)
            {
                var actionName = GetActionName(type);
                _logger.Trace($"Found type '{type.Name}' for action '{actionName}'");

                var action = (IUiAction)Activator.CreateInstance(type);

                _reservedKeywordService.Bind(actionName, arg => action.With(_container).Execute(arg));
            }
        }

        #endregion Methods
    }
}