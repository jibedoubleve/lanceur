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

        public HotKeySettings(int modifierKeys, int key)
        {
            Key = key;
            ModifierKeys = modifierKeys;
        }

        #endregion Constructors

        #region Properties

        public int Key { get; set; }
        public int ModifierKeys { get; set; }

        #endregion Properties
    }
}