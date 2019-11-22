using System;
using System.Configuration;

namespace Probel.Lanceur.Sqlite.Services
{
    public class ConnectionStringManager
    {
        #region Fields

        private readonly string _dbPath;
        private string _connectionString = null;
        private const string CSTRING_PATTERN = "Data Source={0};Version=3;";
        #endregion Fields

        #region Constructors

        public ConnectionStringManager()
        {
            var path = ConfigurationManager.AppSettings["dbPath"];
            _dbPath = Environment.ExpandEnvironmentVariables(path);
        }

        public string Get()
        {
            if (_connectionString == null)
            {
                _connectionString = string.Format(CSTRING_PATTERN, _dbPath);
            }
            return _connectionString;
        }
        #endregion Constructors
    }
}