using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.ServicesImpl.ArgumentHandlers;

namespace Probel.Lanceur.Core.ServicesImpl
{
    public class ParameterResolver : IParameterResolver
    {
        #region Fields

        private readonly IClipboardService _clipboardService;
        private readonly ArgumentHandler _handler;

        #endregion Fields

        #region Constructors

        public ParameterResolver(IClipboardService clipboardService)
        {
            _clipboardService = clipboardService;
            _handler = new TextHandler()
                            .SetNext(new UrlHandler())
                            .SetNext(new ClipboardHandler(_clipboardService));
        }

        #endregion Constructors

        #region Methods

        private string Resolve(string text, string parameters) => _handler.Handle(text, parameters);

        public Alias Resolve(Alias cmd, string parameters)
        {
            var result = cmd.Clone();

            result.FileName = Resolve(cmd.FileName.ToNormalisedParameter(), parameters);
            result.Arguments = Resolve(cmd.Arguments.ToNormalisedParameter(), parameters);

            return result;
        }

        public Cmdline Split(string cmdline)
        {
            var split = (cmdline ?? string.Empty).Split(' ');

            if (split.Length > 0)
            {
                var parameter = string.Join(" ", split, 1, split.Length - 1);
                return new Cmdline(split[0], parameter);
            }
            else { return new Cmdline(cmdline, string.Empty); }
        }

        /// <summary>
        /// Take command from <paramref name="cmdline1"/> and merge it with
        /// parameters from <paramref name="cmdline2"/>.
        /// </summary>
        /// <param name="cmdline1">Command line with the command to use</param>
        /// <param name="cmdline2">Command line with the parameters to use</param>
        /// <returns>A merged command line</returns>
        public Cmdline Merge(string cmdline1, string cmdline2)
        {
            var cmd1 = Split(cmdline1);
            var cmd2 = Split(cmdline2);

            return new Cmdline(cmd1.Command, cmd2.Parameters);
        }

        #endregion Methods
    }
}