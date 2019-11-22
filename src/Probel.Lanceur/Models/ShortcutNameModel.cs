using Caliburn.Micro;

namespace Probel.Lanceur.Models
{
    public class ShortcutNameModel : PropertyChangedBase
    {
        #region Fields

        private long _id;
        private long _idShortcut;
        private string _name;

        #endregion Fields

        #region Properties

        public long Id
        {
            get => _id;
            set => Set(ref _id, value, nameof(Id));
        }

        public long IdShortcut
        {
            get => _idShortcut;
            set => Set(ref _idShortcut, value, nameof(IdShortcut));
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }

        #endregion Properties
    }
}