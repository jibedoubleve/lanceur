using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin;
using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.Core.ServicesImpl
{
    public class AliasService : IAliasService
    {
        #region Fields

        private readonly ICommandRunner _cmdRunner;
        private readonly IDataSourceService _databaseService;
        private readonly IMacroService _macroService;
        private readonly IPluginManager _pluginManager;
        private readonly IParameterResolver _resolver;

        private ILogService _log;

        #endregion Fields

        #region Constructors

        public AliasService(IDataSourceService databaseService,
            IParameterResolver argumentHandler,
            ICommandRunner runner,
            ILogService log,
            IMacroService macroService,
            IPluginManager pluginManager
            )
        {
            _pluginManager = pluginManager;
            _macroService = macroService;
            _cmdRunner = runner;

            _log = log;
            _databaseService = databaseService;
            _resolver = argumentHandler;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Executes the command line.
        /// </summary>
        /// <param name="cmdline">The command line to execute. That's the alias and the arguments (which are not mandatory)</param>
        public ExecutionResult Execute(string cmdline, long sessionId)
        {
            var cmd = _resolver.Split(cmdline);
            
            var alias = _databaseService.GetAlias(cmd.Command, sessionId);
            alias = _resolver.Resolve(alias, cmd.Parameters);

            if (_pluginManager.Exists(alias.Name))
            {
                _pluginManager.Execute(cmd);
                return ExecutionResult.SuccesShow; ;
            }
            else if (_macroService.Exists(alias.FileName))
            {
                _macroService.Execute(alias);
                return ExecutionResult.SuccessHide;
            }
            else
            {
                return _cmdRunner.Execute(alias)
                 ? ExecutionResult.SuccessHide
                 : ExecutionResult.Failure;
            }
        }

        public IEnumerable<AliasText> GetAliasNames(long sessionId) => _databaseService.GetAliasNames(sessionId);

        public IEnumerable<AliasText> GetAliasNames(long sessionId, string criterion)
        {
            var splited = _resolver.Split(criterion);
            criterion = splited.Command.ToLower();
            var result = (from a in _databaseService.GetAliasNames(sessionId)
                          where a.Name.ToLower().StartsWith(criterion)
                          select a).ToList();
            return result;
        }

        public string GetSession(long id) => _databaseService.GetSession(id)?.Name ?? string.Empty;

        #endregion Methods
    }
}