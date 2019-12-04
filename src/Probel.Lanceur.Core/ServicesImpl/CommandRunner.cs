using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using System.Diagnostics;

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

        public void Run(Alias alias)
        {
            if (alias.IsExecutable)
            {
                var pInfo = GetProcessStartInfo(alias);
                var ps = new Process { StartInfo = pInfo };
                ps.Start();
                _databaseService.SetUsage(alias);
            }
            else { _keywordService.ExecuteActionFor(alias.Name, alias.Arguments); }
        }

        #endregion Methods
    }
}