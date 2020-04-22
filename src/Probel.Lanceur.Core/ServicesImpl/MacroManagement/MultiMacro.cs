using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.Services.MacroManagement;
using Probel.Lanceur.Infrastructure;
using System.Text.RegularExpressions;
using System.Threading;

namespace Probel.Lanceur.Core.ServicesImpl.MacroManagement
{
    [Macro("MULTI")]
    public class MultiMacro : IMacroAction
    {
        #region Fields

        private ILogService _log;
        private ICommandRunner _runner;

        #endregion Fields

        #region Methods

        public void Execute(Alias alias)
        {
            _log.Trace($"Managing a '{alias.FileName}' with multiple aliases: {alias.Arguments}");

            var splited = Regex.Split(alias.Arguments, "([@])");

            foreach (var item in splited)
            {
                if (string.IsNullOrEmpty(item)) { continue; }
                else if (item == "@")
                {
                    _log.Trace("Sleep for one second");
                    Thread.Sleep(1_000);
                }
                else
                {
                    _log.Trace($"Multiple alias. Executing '{item}'.");
                    _runner.Execute(alias);
                }
            }
        }

        public IMacroAction With(ILogService log, ICommandRunner runner)
        {
            _runner = runner;
            _log = log;
            return this;
        }

        #endregion Methods
    }
}