using Caliburn.Micro;

namespace Probel.Lanceur.Models
{
    public class AliasSessionModel : PropertyChangedBase
    {
        #region Fields

        private long _id;
        private string _name;

        private string _notes;

        #endregion Fields

        #region Properties

        public long Id
        {
            get => _id;
            set => Set(ref _id, value, nameof(Id));
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }

        public string Notes
        {
            get => _notes;
            set => Set(ref _notes, value, nameof(Notes));
        }

        #endregion Properties
    }
}