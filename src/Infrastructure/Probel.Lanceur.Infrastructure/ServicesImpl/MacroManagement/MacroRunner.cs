using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.Services.MacroManagement;
using Probel.Lanceur.SharedKernel.Logs;
using System;
using System.Linq;
using System.Reflection;

namespace Probel.Lanceur.Infrastructure.ServicesImpl.MacroManagement
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
            var types = from t in Assembly.GetAssembly(typeof(Macros)).GetTypes()
                        where t.GetCustomAttribute<MacroAttribute>() != null
                           && t.GetCustomAttribute<MacroAttribute>().Name == cmd.FileName.ToUpper()
                        select t;

            if (types.Any())
            {
                var type = types.ElementAt(0);

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