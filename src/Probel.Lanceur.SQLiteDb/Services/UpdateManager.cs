using Dapper;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Probel.Lanceur.SQLiteDb.Services
{
    public class UpdateManager
    {
        #region Fields

        public const string Pattern = "Probel.Lanceur.SQLiteDb.Assets.Scripts.";
        private readonly string _connectionString;
        private readonly ILogService _logger;
        private EmbeddedResourceManager _resManager = new EmbeddedResourceManager();

        #endregion Fields

        #region Constructors

        public UpdateManager(string connectionString, ILogService logger)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        #endregion Constructors

        #region Methods

        public void Update()
        {
            var cur = GetCurrentVersion();
                    _logger.Trace($"Current version is {cur}");

            using (var c = BuildConnection())
            {
                foreach (var res in GetResources())
                {
                    if (cur < res.Key)
                    {
                        _logger.Info($"Updating database. Current version is {cur}. Executing script version '{res.Key}'");
                        Execute(res.Value, c);
                        SetVersion(res.Key, c);
                    }
                }
            }
        }

        private void SetVersion(Version key, DbConnection c)
        {
            var sql = "update settings set s_value = @value where s_key = s_key";
            c.Execute(sql, new { value = key.ToString() });
            _logger.Info($"Update database version to '{key}'");
        }

        private void Execute(string value, DbConnection conn)
        {
            _resManager.ReadResourceAsString(value, sql => conn.Execute(sql));
        }

        private DbConnection BuildConnection() => new SQLiteConnection(_connectionString);

        private Version GetCurrentVersion()
        {
            try
            {
                var sql = "select s_value from settings where s_key = 'db_version'";
                using (var c = BuildConnection())
                {
                    var r = (string)c.ExecuteScalar(sql);
                    return Version.Parse(r);
                }
            }
            catch (Exception ex)
            {
                var msg = $"Impossible to retrieve the version of the database. Check inner exception for further information.";
                throw new InvalidOperationException(msg, ex);
            }
        }

        private IDictionary<Version, string> GetResources()
        {
            var dico = new Dictionary<Version, string>();
            var resources = _resManager.ListResources(Pattern);
            var regex = new Regex(@"Probel\.Lanceur\.SQLiteDb\.Assets\.Scripts\.update-(?<version>\d{1,2}\.\d{1,2})\.sql");

            foreach (var r in resources)
            {
                var v = Version.Parse(regex.Match(r)?.Groups["version"]?.Value ?? string.Empty);
                dico.Add(v, r);
            }

            return dico;
        }

        #endregion Methods
    }
}