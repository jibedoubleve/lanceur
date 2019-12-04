using System;
using System.Configuration;
using System.IO;

namespace Probel.Lanceur.SQLiteDb
{
    public class ConnectionStringManager
    {
        #region Fields

        private static readonly string _dbPath;
        private static string _connectionString = null;
        private const string CSTRING_PATTERN = "Data Source={0};Version=3;";
        private readonly EmbeddedResourceManager resource = new EmbeddedResourceManager();
        #endregion Fields

        #region Constructors

        static ConnectionStringManager()
        {
            var path = ConfigurationManager.AppSettings["dbPath"];
            var db = ConfigurationManager.AppSettings["dbName"];

            path = Path.Combine(path, db);

            _dbPath = Environment.ExpandEnvironmentVariables(path);
        }

        public string Get()
        {
            if (_connectionString == null)
            {
                if (!FileExist())
                {
                    var db = ConfigurationManager.AppSettings["dbName"];
                    resource.CopyTo($"Probel.Lanceur.SQLiteDb.Assets.{db}", _dbPath);
                }
                _connectionString = string.Format(CSTRING_PATTERN, _dbPath);
            }
            return _connectionString;
        }

        private bool FileExist() => File.Exists(_dbPath);
        #endregion Constructors
    }
}