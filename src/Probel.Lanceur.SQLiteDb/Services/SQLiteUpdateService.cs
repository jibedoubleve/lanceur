using Dapper;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Reflection;

namespace Probel.Lanceur.SQLiteDb.Services
{
    public class SQLiteUpdateService : IUpdateService
    {
        #region Fields

        private readonly string _connectionString;
        private readonly ILogService _logger;

        #endregion Fields

        #region Constructors

        public SQLiteUpdateService(ILogService logger)
        {
            _connectionString = new ConnectionStringManager().Get();
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public bool DoNeedUpdate()
        {
            var rm = new EmbeddedResourceManager();
            if (HasTableSettings())
            {
                new UpdateManager(_connectionString, _logger).Update();
                return true;
            }
            else
            {
                var script = "Probel.Lanceur.SQLiteDb.Assets.Scripts.update-0.1.sql";
                _logger.Trace($"Do not has table 'settings'. Applying script '{script}'");

                rm.ReadResourceAsString(script, content => ExecuteScript(content));

                return false;
            }
        }

        public void Update() => throw new NotImplementedException();

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

        #endregion Methods
    }
}