using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System.Diagnostics;
using Probel.Lanceur.Core.Constants;

namespace Probel.Lanceur.Core.ServicesImpl
{
    public class CommandRunner : ICommandRunner
    {
        #region Fields

        private readonly IDataSourceService _databaseService;

        private readonly IReservedKeywordService _keywordService;

        #endregion Fields

        #region Constructors

        public CommandRunner(IReservedKeywordService keywordService, IDataSourceService databaseService)
        {
            _databaseService = databaseService;
            _keywordService = keywordService;
        }

        #endregion Constructors

        #region Methods

        private ProcessStartInfo GetProcessStartInfo(Alias s)
        {
            var psInfo = new ProcessStartInfo()
            {
                Arguments = s.Arguments,
                WindowStyle = s.StartMode.AsWindowsStyle(),
                FileName = s.FileName,
            };
            if (s.RunAs == RunAs.Admin) { psInfo.Verb = "runas"; }
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