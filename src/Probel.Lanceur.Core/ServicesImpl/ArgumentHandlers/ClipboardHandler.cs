using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;

namespace Probel.Lanceur.Core.ServicesImpl.ArgumentHandlers
{
    public class ClipboardHandler : ArgumentHandler
    {
        #region Fields

        private readonly IClipboardService _clipboard;

        #endregion Fields

        #region Constructors

        public ClipboardHandler(IClipboardService clipboard) : base(Wildcards.Clipboard) => _clipboard = clipboard;

        #endregion Constructors

        #region Methods

        protected override string DoHandle(string cmdline, string arguments)
        {
            var resolved = cmdline.Replace(Wildcard, _clipboard.GetText());
            return resolved;
        }

        #endregion Methods
    }
}