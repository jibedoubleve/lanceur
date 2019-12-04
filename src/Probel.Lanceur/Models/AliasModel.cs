namespace Probel.Lanceur.Models
{
    public class AliasModel : BaseAliasModel
    {
        #region Fields

        private long _idSession;
        private string _name;

        private string _workingDirectory;

        #endregion Fields

        #region Properties

        public long IdSession
        {
            get => _idSession;
            set => Set(ref _idSession, value, nameof(IdSession));
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }

        public string WorkingDirectory
        {
            get => _workingDirectory;
            set => Set(ref _workingDirectory, value, nameof(WorkingDirectory));
        }

        #endregion Properties
    }
}