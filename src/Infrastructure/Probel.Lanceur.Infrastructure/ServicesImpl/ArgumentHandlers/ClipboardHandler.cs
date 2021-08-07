using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System.Net;

namespace Probel.Lanceur.Infrastructure.ServicesImpl.ArgumentHandlers
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
            var p = WebUtility.UrlEncode(_clipboard.GetText());
            var resolved = cmdline.ToLower().Replace(Wildcard, p);
            return resolved;
        }

        #endregion Methods
    }
}