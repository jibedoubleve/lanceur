using Probel.Lanceur.Core.Services;

namespace Probel.Lanceur.Infrastructure.ServicesImpl.ArgumentHandlers
{
    public class ClipboardRawHandler : ArgumentHandler
    {
        #region Fields

        private readonly IClipboardService _clipboard;

        #endregion Fields

        #region Constructors

        public ClipboardRawHandler(IClipboardService clipboard) : base(Wildcards.RawClipboard) => _clipboard = clipboard;

        #endregion Constructors

        #region Methods

        protected override string DoHandle(string cmdline, string parameters)
        {
            var resolved = cmdline.ToLower().Replace(Wildcard, _clipboard.GetText());
            return resolved;
        }

        #endregion Methods
    }
}