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
            var split = cmdline.Split(' ');

            if (split.Length > 0)
            {
                var parameter = string.Join(" ", split, 1, split.Length - 1);
                return new Cmdline(split[0], parameter);
            }
            else { return new Cmdline(cmdline, string.Empty); }
        }

        #endregion Methods
    }
}