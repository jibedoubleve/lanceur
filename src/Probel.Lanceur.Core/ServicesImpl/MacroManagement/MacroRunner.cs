using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.Services.MacroManagement;
using Probel.Lanceur.Infrastructure;
using System;
using System.Linq;
using System.Reflection;

namespace Probel.Lanceur.Core.ServicesImpl.MacroManagement
{
    public class MacroRunner : IMacroRunner
    {
        #region Fields

        private readonly ILogService _log;
        private readonly ICommandRunner _runner;

        #endregion Fields

        #region Constructors

        public MacroRunner(ILogService logservice, ICommandRunner runner)
        {
            _log = logservice;
            _runner = runner;
        }

        #endregion Constructors

        #region Methods

        public void Execute(Alias cmd)
        {
            var types = from t in Assembly.GetAssembly(typeof(IMacroAction)).GetTypes()
                        where t.GetCustomAttribute<MacroAttribute>() != null
                           && t.GetCustomAttribute<MacroAttribute>().Name == cmd.FileName.ToUpper()
                        select t;

            foreach (var type in types)
            {
                _log.Trace($"Found maro action of type '{type}'");
                var action = (IMacroAction)Activator.CreateInstance(type);
                action.With(_log, _runner)
                      .Execute(cmd);
            }
        }

        public bool Exists(string name) => Macros.Has(name);

        #endregion Methods
    }
}