using Caliburn.Micro;

namespace Probel.Lanceur.Models.Settings
{
    public class DatabaseSettingsModel : PropertyChangedBase
    {
        #region Fields

        private string _dbName;
        private string _dbPath;

        #endregion Fields

        #region Properties

        public string DatabaseName
        {
            get => _dbName;
            set => Set(ref _dbName, value, nameof(DatabaseName));
        }

        public string DatabasePath
        {
            get => _dbPath;
            set => Set(ref _dbPath, value, nameof(DatabasePath));
        }

        #endregion Properties
    }
}