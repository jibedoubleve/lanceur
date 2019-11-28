using Probel.Lanceur.Core.Services;
using System.Collections.Generic;

namespace Probel.Lanceur.Services
{
    public class DefaultAliasService : IAliasService
    {
        #region Fields

        private readonly ICommandRunner _cmdRunner;
        private readonly IDataSourceService _databaseService;
        private readonly IParameterResolver _resolver;

        #endregion Fields

        #region Constructors

        public DefaultAliasService(IDataSourceService databaseService, IParameterResolver argumentHandler, ICommandRunner runner, ILogService log)
        {
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

            _log.Trace($"Executing '{cmd.Name}' [{cmd.FileName}] with args '{cmd.Arguments}'");

            _cmdRunner.Run(cmd);
        }

        public IEnumerable<string> GetAliasNames(long sessionId) => _databaseService.GetAliasNames(sessionId);

        #endregion Methods
    }
}