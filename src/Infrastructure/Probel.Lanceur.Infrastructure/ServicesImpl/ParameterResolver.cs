using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Infrastructure.ServicesImpl.ArgumentHandlers;
using System.Text.RegularExpressions;

namespace Probel.Lanceur.Infrastructure.ServicesImpl
{
    public class ParameterResolver : IParameterResolver
    {
        #region Fields

        private readonly IDataSourceService _dataService;

        private readonly ArgumentHandler _handler;

        #endregion Fields

        #region Constructors

        public ParameterResolver(IClipboardService clipboardService, IDataSourceService dataService)
        {
            _dataService = dataService;
            _handler = new TextHandler()
                            .SetNext(new UriHandler())
                            .SetNext(new ClipboardHandler(clipboardService))
                            .SetNext(new ClipboardRawHandler(clipboardService));
        }

        #endregion Constructors

        #region Methods

        private bool CheckAliasExists(string[] cmdSplitted, long sessionId)
        {
            if (cmdSplitted.Length > 0)
            {
                var r = _dataService.AliasExists(cmdSplitted[0], sessionId);
                return r;
            }
            return false;
        }

        /// <summary>
        /// Take command from <paramref name="cmdline1"/> and merge it with
        /// parameters from <paramref name="cmdline2"/>.
        /// </summary>
        /// <param name="cmdline1">Command line with the command to use</param>
        /// <param name="cmdline2">Command line with the parameters to use</param>
        /// <returns>A merged command line</returns>
        public Cmdline Merge(string cmdline1, string cmdline2, long sessionId)
        {
            if (string.IsNullOrEmpty(cmdline1)) { return Split(cmdline2, sessionId); }
            else if (string.IsNullOrEmpty(cmdline2)) { return Split(cmdline1, sessionId); }
            else
            {
                var cmd1 = Split(cmdline1, sessionId);
                var cmd2 = Split(cmdline2, sessionId);

                return new Cmdline(cmd1.Command, cmd2.Arguments);
            }
        }

        public Alias Resolve(Alias cmd, string parameters)
        {
            var result = cmd.Clone();

            result.FileName = Resolve(cmd.FileName.ToNormalisedParameter(), parameters);
            result.Arguments = Resolve(cmd.Arguments.ToNormalisedParameter(), parameters);

            return result;
        }

        public string Resolve(string text, string parameters) => _handler.Handle(text, parameters);

        public Cmdline Split(string cmd, long sessionId)
        {
            var cmdline = (cmd ?? string.Empty);
            if (CheckAliasExists(cmdline.Split(' '), sessionId) == false)
            {
                /*
                 * Normalise command line: every command line that starts with
                 * an *NOT* space and *NOT* alphanumeric should be concidered as
                 * a command shortcut if it doesn't exist as is in the DB
                 */
                cmdline = Regex.Replace(cmdline, @"^([\W])", "$1 ");
            }

            var split = cmdline.Split(' ');

            if (split.Length > 0)
            {
                var parameter = string.Join(" ", split, 1, split.Length - 1).Trim();
                return new Cmdline(split[0].Trim(), parameter);
            }
            else { return new Cmdline(cmdline, string.Empty); }
        }

        #endregion Methods
    }
}