using Probel.Lanceur.Core.Services;
using System.Windows;

namespace Probel.Lanceur.Services
{
    public class ClipboardService : IClipboardService
    {
        #region Methods

        public string GetText() => Clipboard.GetText();

        #endregion Methods
    }
}