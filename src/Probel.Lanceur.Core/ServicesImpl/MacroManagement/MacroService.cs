using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.Services.MacroManagement;
using Probel.Lanceur.Plugin;
using System;
using System.Linq;
using System.Reflection;

namespace Probel.Lanceur.Core.ServicesImpl.MacroManagement
{
    public class MacroService : IMacroService
    {
        #region Fields

        private readonly ILogService _log;
        private IAliasService _aliasService;
        private ICommandRunner _cmdrunner;

        #endregion Fields

        #region Constructors

        public MacroService(ILogService logservice)
        {
            _log = logservice;
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
                action.With(_cmdrunner, _log, _aliasService)
                      .Execute(cmd);
            }
        }

        public bool Exists(string name) => Macros.Has(name);

        public IMacroService With(ICommandRunner cmdrunner, IAliasService aliasService)
        {
            _cmdrunner = cmdrunner;
            _aliasService = aliasService;
            return this;
        }

        #endregion Methods
    }
}