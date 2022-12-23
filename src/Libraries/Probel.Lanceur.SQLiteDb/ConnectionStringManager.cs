using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Services;
using System;
using System.IO;

namespace Probel.Lanceur.SQLiteDb
{
    public class ConnectionStringManager : ConnectionStringManagerBase
    {
        #region Fields

        private readonly EmbeddedResourceManager resource = new EmbeddedResourceManager();

        #endregion Fields

        #region Constructors

        public ConnectionStringManager(ISettingsService settings) : base(settings)
        {
        }

        #endregion Constructors

        #region Methods

        public override string Get()
        {
            if (ConnectionString == null)
            {
                if (!FileExist())
                {
                    resource.CopyTo($"Probel.Lanceur.SQLiteDb.Assets.data.db", DbPath);
                }
            }
            return ConnectionString;
        }

        private bool FileExist() => File.Exists(DbPath);

        #endregion Methods
    }

    public abstract class ConnectionStringManagerBase : IConnectionStringManager
    {
        #region Fields

        private const string CSTRING_PATTERN = "Data Source={0};Version=3;";

        private static string _connectionString = null;

        private static string _dbPath = null;

        private readonly ISettingsService _settings;

        private AppSettings _appSettings;

        #endregion Fields

        #region Constructors

        protected ConnectionStringManagerBase(ISettingsService settings)
        {
            _settings = settings;
        }

        #endregion Constructors

        #region Properties

        protected AppSettings AppSettings
        {
            get
            {
                if (_appSettings == null) { _appSettings = _settings.Get(); }
                return _appSettings;
            }
        }

        protected string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    var path = DbPath;
                    _connectionString = string.Format(CSTRING_PATTERN, path);
                }
                return _connectionString;
            }
        }

        protected string DbPath
        {
            get
            {
                if (string.IsNullOrEmpty(_dbPath))
                {
                    ;
                    var path = AppSettings.DatabasePath;
                    _dbPath = Environment.ExpandEnvironmentVariables(path);
                }
                return _dbPath;
            }
        }

        #endregion Properties

        #region Methods

        public abstract string Get();

        #endregion Methods
    }
}