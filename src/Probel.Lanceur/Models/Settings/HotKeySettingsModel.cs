using Caliburn.Micro;
using System.Diagnostics;

namespace Probel.Lanceur.Models.Settings
{
    [DebuggerDisplay("{ModifierKeys:{ModifierKeys} - Key:{Key}")]
    public class HotKeySettingsModel : PropertyChangedBase
    {
        #region Fields

        private int _key;
        private int _modifierKeys;

        #endregion Fields

        #region Constructors

        public HotKeySettingsModel()
        {
        }

        public HotKeySettingsModel(int modifierKeys, int key)
        {
            ModifierKeys = modifierKeys;
            Key = key;
        }

        #endregion Constructors

        #region Properties

        public int Key
        {
            get => _key;
            set => Set(ref _key, value, nameof(Key));
        }

        public int ModifierKeys
        {
            get => _modifierKeys;
            set => Set(ref _modifierKeys, value, nameof(ModifierKeys));
        }

        #endregion Properties
    }
}