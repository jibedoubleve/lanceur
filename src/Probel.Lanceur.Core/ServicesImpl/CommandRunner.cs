﻿using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Infrastructure;
using System.Diagnostics;
using System.Net.Configuration;

namespace Probel.Lanceur.Core.ServicesImpl
{
    public class CommandRunner : ICommandRunner
    {
        #region Fields

        private readonly IDataSourceService _databaseService;
        private readonly IReservedKeywordService _keywordService;
        private readonly ILogService _log;

        #endregion Fields

        #region Constructors

        public CommandRunner(IReservedKeywordService keywordService, IDataSourceService databaseService, ILogService logService)
        {
            _log = logService;
            _databaseService = databaseService;
            _keywordService = keywordService;
        }

        #endregion Constructors

        #region Methods

        public ExecutionResult Execute(string cmd, long idSession)
        {
            var alias = _databaseService.GetAlias(cmd, idSession);
            return Execute(alias);
        }

        public ExecutionResult Execute(Alias alias)
        {
            if (alias.IsExecutable)
            {
                var pInfo = GetProcessStartInfo(alias);
                using (var ps = new Process { StartInfo = pInfo }) { ps.Start(); }
                _databaseService.SetUsage(alias);
                return ExecutionResult.SuccessHide;
            }
            else if (_keywordService.IsReserved(alias.Name))
            {
                return _keywordService.ExecuteActionFor(alias.Name, alias.Arguments);
            }
            else
            {
                var msg = $"Alias '{alias.Name}' does not exist in the database.";
                _log.Warning(msg);
                return ExecutionResult.Failure(msg);
            }
        }

        private ProcessStartInfo GetProcessStartInfo(Alias alias)
        {
            _log.Debug($"Executing '{alias.FileName}' with args '{alias.Arguments}'");

            var psInfo = new ProcessStartInfo()
            {
                Arguments = alias.Arguments,
                WindowStyle = alias.StartMode.AsWindowsStyle(),
                FileName = alias.FileName,
                WorkingDirectory = alias.WorkingDirectory,
            };
            if (alias.RunAs == RunAs.Admin) { psInfo.Verb = "runas"; }
            return psInfo;
        }

        #endregion Methods
    }
}