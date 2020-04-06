using System.Diagnostics;

namespace Probel.Lanceur.Core.Entities.Settings
{
    [DebuggerDisplay("ModifierKeys:{ModifierKeys} - Key:{Key}")]
    public class HotKeySettings
    {
        #region Constructors

        public HotKeySettings()
        {
        }

        #endregion Constructors

        #region Properties

        public int Key { get; set; }
        public int ModifierKeys { get; set; }

        #endregion Properties
    }
}