﻿using Dapper;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.SharedKernel.Logs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Probel.Lanceur.SQLiteDb.Services
{
    public partial class SQLiteDatabaseService : IDataSourceService
    {
        #region Fields

        private readonly string _connectionString;
        private readonly IKeywordService _keywordService;
        private readonly ILogService _log;

        #endregion Fields

        #region Constructors

        public SQLiteDatabaseService(
            IKeywordService keywordService,
            ILogService log,
            IConnectionStringManager csm
            )
        {
            _log = log;
            _connectionString = csm.Get();
            _keywordService = keywordService;
        }

        #endregion Constructors

        #region Methods

        private SQLiteConnection BuildConnection()
        {
            var conn = new SQLiteConnection(_connectionString);
            conn.Open();
            return conn;
        }

        private void UpdateNames(IDbConnection c, long idAlias, IEnumerable<string> names)
        {
            var sqlDelete = "delete from alias_name where id_alias = @idAlias";
            var sqlInsert = "insert into alias_name(id_alias, name) values(@idAlias, @name)";
            _log.Trace($"Removing names for alias {idAlias}");
            c.Execute(sqlDelete, new { idAlias });
            foreach (var name in names)
            {
                _log.Trace($"Adding name '{name}' to alias {idAlias}");
                c.Execute(sqlInsert, new { idAlias, name });
            }
        }

        public void Clear()
        {
            using (var c = BuildConnection())
            {
                var sql = @"
                    delete from alias;
                    delete from alias_name;
                    delete from alias_usage;
                    delete from alias_session where name = 'SlickRun';";
                c.Execute(sql);
            }
        }

        public void Create(Alias s, IEnumerable<string> names = null)
        {
            if (names == null && string.IsNullOrEmpty(s.Name)) { throw new NotSupportedException($"Cannot create a new alias without name."); }

            s.Normalise();
            var sql = @"
                insert into alias (
                    arguments,
                    file_name,
                    notes,
                    run_as,
                    start_mode,
                    id_session,
                    icon
                ) values (
                    @arguments,
                    @fileName,
                    @notes,
                    @runAs,
                    @startMode,
                    @idSession,
                    @icon
                );";
            var sql2 = @"insert into alias_name(id_alias, name) values(@idAlias, @name)";

            using (var c = BuildConnection())
            using (var t = c.BeginTransaction())
            {
                try
                {
                    c.Execute(sql, new
                    {
                        s.Arguments,
                        s.FileName,
                        s.Notes,
                        s.RunAs,
                        s.StartMode,
                        s.IdSession,
                        s.Icon
                    });

                    var lastId = c.LastInsertRowId;
                    var nameList = new List<string>(names);

                    if (!string.IsNullOrEmpty(s.Name)) { nameList.Add(s.Name); }
                    foreach (var name in nameList)
                    {
                        _log.Trace($"Insert alias name '{name}' with idAdlias '{lastId}'");
                        c.Execute(sql2, new { name, IdAlias = lastId });
                    }
                    t.Commit();
                }
                catch (Exception ex)
                {
                    t.Rollback();
                    throw new Exception(
                        $"An error occured while creating alias and alias name. See inner for further information",
                        ex
                    );
                }
            }
        }

        public void Create(AliasSession session)
        {
            if (session == null) { return; }

            var sql = @"
                insert into alias_session (
                    name,
                    notes
                ) values (
                    @name,
                    @notes
                )";
            using (var c = BuildConnection())
            {
                c.Execute(sql, new { name = session.Name, notes = session.Notes });
            }
        }

        public void Delete(Alias alias)
        {
            var sql = @"delete from alias_name where id_alias = @id";
            var sql2 = @"delete from alias where id = @id";
            using (var c = BuildConnection())
            using (var t = c.BeginTransaction())
            {
                try
                {
                    c.Execute(sql, new { alias.Id });
                    c.Execute(sql2, new { alias.Id });
                    t.Commit();
                }
                catch (Exception ex)
                {
                    t.Rollback();
                    throw new Exception($"An error occured while deleteing an alias {(alias.Name ?? "NULL")}", ex);
                }
            }
        }

        public void Delete(AliasSession session)
        {
            using (var c = BuildConnection())
            {
                var queries = new string[]
                {
                    @"delete from alias_session where id = @id",
                    @"delete from alias where id_session = @id"
                };
                foreach (var sql in queries) { c.Execute(sql, new { session.Id }); }
            }
        }

        public void SetUsage(Alias alias)
        {
            if (alias.Id == 0 || alias.IdSession == 0)
            {
                _log.Trace($"Try to set usage to unsupported Alias with this name'{(alias?.Name ?? "N.A.")}'");
            }
            else
            {
                using (var c = BuildConnection())
                {
                    var sql = @"
                    insert into alias_usage (
                        id_alias,
                        id_session,
                        time_stamp

                    ) values (
                        @idAlias,
                        @idSession,
                        @now
                    )";
                    c.Execute(sql, new { idAlias = alias.Id, idSession = alias.IdSession, now = DateTime.Now });
                }
            }
        }

        public void Update(Alias alias, IEnumerable<string> names = null)
        {
            alias.Normalise();

            var sql = @"
                update alias
                set
                    arguments   = @arguments,
                    file_name   = @fileName,
                    notes       = @notes,
                    run_as      = @runAs,
                    start_mode  = @startMode,
                    working_dir = @WorkingDirectory,
                    icon        = @Icon
                where id = @id;";

            using (var c = BuildConnection())
            using (var t = c.BeginTransaction())
            {
                try
                {
                    c.Execute(sql, new
                    {
                        alias.Arguments,
                        alias.FileName,
                        alias.Notes,
                        alias.RunAs,
                        alias.StartMode,
                        alias.Id,
                        alias.WorkingDirectory,
                        alias.Icon
                    });

                    if (names == null || names.Count() == 0) { names = new List<string> { alias.Name }; };

                    _log.Debug($"Updating alias {alias.Id}");
                    UpdateNames(c, alias.Id, names);

                    t.Commit();
                }
                catch (Exception ex)
                {
                    t.Rollback();
                    throw new Exception(
                        $"An error occured while updating alias and alias name. See inner for further information",
                        ex
                    );
                }
            }
        }

        public void Update(AliasSession session)
        {
            var sql = @"
                update alias_session
                set
                    name  = @name,
                    notes = @notes
                where id = @id";
            using (var c = BuildConnection())
            {
                c.Execute(sql, new { session.Id, session.Name, session.Notes });
            }
        }

        #endregion Methods
    }
}