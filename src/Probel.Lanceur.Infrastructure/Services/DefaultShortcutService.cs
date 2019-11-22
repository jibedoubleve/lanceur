using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System.Collections.Generic;
using System.Diagnostics;

namespace Probel.Lanceur.Infrastructure.Services
{
    public class DefaultShortcutService : IShortcutService
    {
        public ILogService _log { get; }
        #region Fields

        private readonly IDatabaseService _databaseService;
        private readonly IParameterResolver _resolver;
        private readonly ICommandRunner _cmdRunner;

        #endregion Fields

        #region Constructors

        public DefaultShortcutService(IDatabaseService databaseService, IParameterResolver argumentHandler, ICommandRunner runner, ILogService log)
        {
            _log = log;
            _databaseService = databaseService;
            _resolver = argumentHandler;
            _cmdRunner = runner;
        }

        #endregion Constructors

        #region Methods

        public void Execute(string cmdline)
        {
            var splited = _resolver.Split(cmdline);
            var cmd = _databaseService.GetShortcut(splited.Command);

            cmd = _resolver.Resolve(cmd, splited.Parameters);

            _log.Debug($"Executing '{cmd.Name}' [{cmd.FileName}] with args '{cmd.Arguments}'");

            _cmdRunner.Run(cmd);
        }

        public IEnumerable<string> GetShortcutsNames(long sessionId) => _databaseService.GetShortcutNames(sessionId);

        #endregion Methods
    }
}