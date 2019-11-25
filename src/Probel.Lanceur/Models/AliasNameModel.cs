using Caliburn.Micro;

namespace Probel.Lanceur.Models
{
    public class AliasNameModel : PropertyChangedBase
    {
        #region Fields

        private long _id;
        private long _idAlias;
        private string _name;

        #endregion Fields

        #region Properties

        public long Id
        {
            get => _id;
            set => Set(ref _id, value, nameof(Id));
        }

        public long IdAlias
        {
            get => _idAlias;
            set => Set(ref _idAlias, value, nameof(IdAlias));
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }

        #endregion Properties
    }
}