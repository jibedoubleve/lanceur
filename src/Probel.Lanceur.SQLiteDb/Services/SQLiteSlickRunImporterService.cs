using Dapper;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Infrastructure;
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

        private int _percentage = 0;

        #endregion Fields

        #region Constructors

        public SQLiteSlickRunImporterService(ISlickRunExtractor extractor, ILogService log, IConnectionStringManager csm)
        {
            _log = log;
            _extractor = extractor;
            _connectionString = csm.Get();
        }

        #endregion Constructors

        #region Events

        public event EventHandler<ImportUpdatedEventArg> ImportUpdated;

        #endregion Events

        #region Properties

        public ILogService _log { get; }

        #endregion Properties

        #region Methods

        public long Import(string sessionName = null, string fileName = null)
        {
            _percentage = 0;

            var n = DateTime.Now;

            if (string.IsNullOrEmpty(sessionName)) { sessionName = $"SlickRun-Import_{n.Year}-{n.Month}-{n.Day}"; }

            using (var c = new SQLiteConnection(_connectionString))
            {
                var idSession = InsertSession(c, sessionName);
                var aliases = _extractor.Extract(fileName);
                var sql = @"
                    insert into alias (
                        arguments,
                        file_name,
                        notes,
                        run_as,
                        start_mode,
                        working_dir,
                        id_session
                    ) values (
                        @Arguments,
                        @FileName,
                        @Notes,
                        @RunAs,
                        @StartMode,
                        @WorkingDirectory,
                        @idSession
                    );
                    select last_insert_rowid() from alias;";

                var counter = 0d;
                var count = aliases.Count();
                foreach (var s in aliases)
                {
                    var id = c.Query<long>(sql, new { Arguments = s.Arguments.Trim('"', ' '), FileName = s.FileName.Trim('"', ' '), s.Notes, s.RunAs, s.StartMode, s.WorkingDirectory, idSession }).ToList();

                    _percentage = (int)((++counter / count) * 100);
                    InsertNames(c, id[0], s.Names);
                }

                OnImportUpdated(100, $"All {aliases.Count()} keyword(s) imported!");
                return idSession;
            }
        }

        private void InsertNames(SQLiteConnection c, long id, IEnumerable<string> names)
        {
            var sql = @"insert into alias_name (id_alias, name) values (@id, @name)";
            foreach (var name in names)
            {
                c.Execute(sql, new { id, name = name.Trim('"', ' ') });
                OnImportUpdated(_percentage, $"New name: '{name}'");
            }
        }

        private long InsertSession(SQLiteConnection c, string sessionName)
        {
            var sql = @"
                    insert into alias_session (name, notes) values(@sessionName,'Imported from SlickRun');
                    select last_insert_rowid() from alias_session;";

            var id = c.Query<long>(sql, new { sessionName }).ToList();
            return id[0];
        }

        private void OnImportUpdated(int progress, string output)
        {
            ImportUpdated?.Invoke(this, new ImportUpdatedEventArg(progress, output));
        }

        #endregion Methods
    }
}