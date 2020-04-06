using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Probel.Lanceur.Core.ServicesImpl.MacroManagement
{
    [Macro("MULTI")]
    public class MultiMacro : IMacroAction
    {
        #region Fields

        private IAliasService _aliasService;
        private ICommandRunner _cmdrunner;
        private ILogService _log;

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
                    Thread.Sleep(1 * 1000);
                }
                else
                {
                    _log.Trace($"Multiple alias. Executing '{item}'.");
                    _aliasService.Execute(item, alias.IdSession);
                }
            }
        }

        public IMacroAction With(ICommandRunner cmdrunner, ILogService log, IAliasService aliasService)
        {
            _aliasService = aliasService;
            _log = log;
            _cmdrunner = cmdrunner;
            return this;
        }

        #endregion Methods
    }
}