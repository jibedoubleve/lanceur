using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Plugins;
using Probel.Lanceur.Core.Services;
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
            _log = log;
            _databaseService = databaseService;
            _resolver = argumentHandler;
            _cmdRunner = runner;
        }

        #endregion Constructors

        #region Properties

        public ILogService _log { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Executes the command line.
        /// </summary>
        /// <param name="cmdline">The command line to execute. That's the alias and the arguments (which are not mandatory)</param>
        public ExecutionResult Execute(string cmdline)
        {
            var splited = _resolver.Split(cmdline);
            var cmd = _databaseService.GetAlias(splited.Command);

            cmd = _resolver.Resolve(cmd, splited.Parameters);

            if (_pluginManager.Exists(cmd.Name))
            {
                _pluginManager.Build(cmd.Name).Execute(cmd.Arguments);
                return ExecutionResult.SuccesShow; ;
            }
            else if (_macroService.Has(cmd.FileName))
            {
                _macroService.With(_cmdRunner, this)
                             .Handle(cmd);
                return ExecutionResult.SuccessHide;
            }
            else
            {
                return _cmdRunner.Run(cmd)
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

        #endregion Methods
    }


}