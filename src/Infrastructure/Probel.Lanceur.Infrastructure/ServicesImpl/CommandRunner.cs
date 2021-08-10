using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.SharedKernel.Logs;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Probel.Lanceur.Infrastructure.ServicesImpl
{
    public class CommandRunner : ICommandRunner
    {
        #region Fields

        private readonly IDataSourceService _databaseService;
        private readonly IKeywordService _keywordService;
        private readonly ILogService _log;

        #endregion Fields

        #region Constructors

        public CommandRunner(IKeywordService keywordService, IDataSourceService databaseService, ILogService logService)
        {
            _log = logService;
            _databaseService = databaseService;
            _keywordService = keywordService;
        }

        #endregion Constructors

        #region Methods

        private ExecutionResult ExecuteUwp(Alias alias)
        {
            try
            {
                const string noArgs = "";
                const ApplicationActivationHelper.ActivateOptions noFlags = ApplicationActivationHelper.ActivateOptions.None;

                var manager = new ApplicationActivationHelper.ApplicationActivationManager();
                Task.Run(() => manager.ActivateApplication(alias.UniqueIdentifier, noArgs, noFlags, out var processId));
                return ExecutionResult.SuccessHide;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                return ExecutionResult.Failure(ex.Message);
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

        public ExecutionResult Execute(string cmd, long idSession)
        {
            var alias = _databaseService.GetAlias(cmd, idSession);
            return Execute(alias);
        }

        public ExecutionResult Execute(Alias alias)
        {
            ExecutionResult result = ExecutionResult.None;
            if (alias.IsPackaged) { result = ExecuteUwp(alias); }
            else if (alias.IsExecutable)
            {
                var pInfo = GetProcessStartInfo(alias);
                Task.Run(() =>
                {
                    using (var ps = new Process { StartInfo = pInfo }) { ps.Start(); }
                });
                result = ExecutionResult.SuccessHide;
            }
            else if (_keywordService.IsReserved(alias.Name))
            {
                result = _keywordService.ExecuteActionFor(alias.Name, alias.Arguments);
            }
            else
            {
                var psi = new ProcessStartInfo
                {
                    FileName = alias.FileName,
                    Verb = "open",
                    UseShellExecute = true,
                };
                Process.Start(psi);
                result = ExecutionResult.SuccessHide;
                //var msg = $"Alias '{alias.Name}' does not exist in the database.";
                //_log.Warning(msg);
                //result = ExecutionResult.Failure(msg);
            }

            _databaseService.SetUsage(alias);
            return result;
        }

        #endregion Methods
    }
}