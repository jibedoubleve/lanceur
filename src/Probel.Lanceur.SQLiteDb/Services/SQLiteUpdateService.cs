using Dapper;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Infrastructure;
using System;
using System.Data.Common;
using System.Data.SQLite;

namespace Probel.Lanceur.SQLiteDb.Services
{
    public class SQLiteUpdateService : IUpdateService
    {
        #region Fields

        private readonly string _connectionString;
        private readonly ILogService _logger;

        #endregion Fields

        #region Constructors

        public SQLiteUpdateService(ILogService logger, IConnectionStringManager csm)
        {
            _connectionString = csm.Get();
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public void UpdateDatabase()
        {
            var rm = new EmbeddedResourceManager();
            if (IsEmptyDatabase())
            {
                //Build first tables
                var script = "Probel.Lanceur.SQLiteDb.Assets.Scripts.ddl.sql";
                _logger.Info($"Create first tables. Applying script '{script}'");

                rm.ReadResourceAsString(script, content => ExecuteScript(content));
            }
            if (!HasTableSettings())
            {
                var script = "Probel.Lanceur.SQLiteDb.Assets.Scripts.update-0.1.sql";
                _logger.Info($"Table 'settings' does not exist. Applying script '{script}'");

                rm.ReadResourceAsString(script, content => ExecuteScript(content));
            }

            new UpdateManager(_connectionString, _logger).Update();
        }

        private DbConnection BuildConnection() => new SQLiteConnection(_connectionString);

        private void ExecuteScript(string content)
        {
            try
            {
                using (var c = BuildConnection())
                {
                    c.Execute(content);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occured while updating database. {ex.Message}", ex);
            }
        }

        private bool HasTableSettings()
        {
            using (var c = BuildConnection())
            {
                var sql = "select count(name) from sqlite_master where type = 'table' and name = 'settings';";
                return (long)c.ExecuteScalar(sql) > 0;
            }
        }

        private bool IsEmptyDatabase()
        {
            using (var c = BuildConnection())
            {
                var sql = "select count(name) from sqlite_master where type = 'table';";
                return (long)c.ExecuteScalar(sql) == 0;
            }
        }

        #endregion Methods
    }
}