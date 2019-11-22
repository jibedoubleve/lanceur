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

        private readonly IDatabaseService _databaseService;

        private readonly IKeywordService _keywordService;

        #endregion Fields

        #region Constructors

        public CommandRunner(IKeywordService keywordService, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _keywordService = keywordService;
        }

        #endregion Constructors

        #region Methods

        private ProcessStartInfo GetProcessStartInfo(Shortcut s)
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

        public void Run(Shortcut shortcut)
        {
            if (shortcut.IsExecutable)
            {
                var pInfo = GetProcessStartInfo(shortcut);
                var ps = new Process { StartInfo = pInfo };
                ps.Start();
                _databaseService.SetUsage(shortcut);
            }
            else { _keywordService.ExecuteActionFor(shortcut.Name, shortcut.Arguments); }
        }

        #endregion Methods
    }
}