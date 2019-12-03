using Probel.Lanceur.Core.Services;
using System.Collections.Generic;

namespace Probel.Lanceur.Services
{
    public class DefaultAliasService : IAliasService
    {
        #region Fields

        private readonly ICommandRunner _cmdRunner;
        private readonly IDataSourceService _databaseService;
        private readonly IMacroService _macroService;
        private readonly IParameterResolver _resolver;

        #endregion Fields

        #region Constructors

        public DefaultAliasService(IDataSourceService databaseService, IParameterResolver argumentHandler, ICommandRunner runner, ILogService log, IMacroService macroService)
        {
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

        public void Execute(string cmdline)
        {
            var splited = _resolver.Split(cmdline);

            var cmd = _databaseService.GetAlias(splited.Command);

            cmd = _resolver.Resolve(cmd, splited.Parameters);

            if (_macroService.Has(cmd.FileName))
            {
                _macroService.With(_cmdRunner, this).Handle(cmd);
            }
            else { _cmdRunner.Run(cmd); }
        }

        public IEnumerable<string> GetAliasNames(long sessionId) => _databaseService.GetAliasNames(sessionId);

        #endregion Methods
    }
}