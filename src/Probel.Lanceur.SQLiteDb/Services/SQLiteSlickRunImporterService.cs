﻿using Dapper;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Probel.Lanceur.SQLiteDb.Services
{
    public class SQLiteSlickRunImporterService : ISlickRunImporterService
    {
        #region Fields

        private readonly string _connectionString;
        private readonly ISlickRunExtractor _extractor;

        #endregion Fields

        #region Constructors

        public SQLiteSlickRunImporterService(ISlickRunExtractor extractor, ILogService log)
        {
            _log = log;
            _extractor = extractor;
            _connectionString = new ConnectionStringManager().Get();
        }

        #endregion Constructors

        #region Properties

        public ILogService _log { get; }

        #endregion Properties

        #region Methods

        private long GetIdSession(SQLiteConnection c, string sessionName)
        {
            var sql = @"
                    insert into shortcut_session (name, notes) values(@sessionName,'Imported from SlickRun');
                    select last_insert_rowid() from shortcut_session;";

            var id = c.Query<long>(sql, new { sessionName }).ToList();
            return id[0];
        }

        private void InsertNames(SQLiteConnection c, long id, IEnumerable<string> names)
        {
            var sql = @"insert into shortcut_name (id_shortcut, name) values (@id, @name)";
            foreach (var name in names)
            {
                c.Execute(sql, new { id, name = name.Trim('"', ' ') });
                _log.Debug($"New name: '{name}'");
            }
        }

        public void Import(string sessionName = null, string fileName = null)
        {
            var n = DateTime.Now;

            if (string.IsNullOrEmpty(sessionName)) { sessionName = $"SlickRun-Import_{n.Year}-{n.Month}-{n.Day}"; }

            using (var c = new SQLiteConnection(_connectionString))
            {
                var idSession = GetIdSession(c, sessionName);
                var shortcuts = _extractor.Extract(fileName);
                var sql = @"
                    insert into shortcut (
                        arguments,
                        file_name,
                        notes,
                        run_as,
                        start_mode,
                        id_session
                    ) values (
                        @Arguments,
                        @FileName,
                        @Notes,
                        @RunAs,
                        @StartMode,
                        @idSession
                    );
                    select last_insert_rowid() from shortcut;";

                foreach (var s in shortcuts)
                {
                    var id = c.Query<long>(sql, new { Arguments = s.Arguments.Trim('"', ' '), FileName = s.FileName.Trim('"', ' '), s.Notes, s.RunAs, s.StartMode, idSession }).ToList();

                    InsertNames(c, id[0], s.Names);
                }
                _log.Debug($"All {shortcuts.Count()} keyword(s) imported!");
            }
        }

        #endregion Methods
    }
}