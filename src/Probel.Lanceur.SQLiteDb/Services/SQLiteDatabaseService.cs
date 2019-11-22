using Dapper;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;

namespace Probel.Lanceur.SQLiteDb.Services
{
    public class SQLiteDatabaseService : IDatabaseService
    {
        #region Fields

        private readonly string _connectionString;
        private readonly IKeywordService _keywordService;
        private readonly ILogService _log;
        private readonly IReservedKeywordService _reservedKeywordService;

        #endregion Fields

        #region Constructors

        public SQLiteDatabaseService(IKeywordService keywordService, ILogService log, IReservedKeywordService reservedKeywordService)
        {
            _reservedKeywordService = reservedKeywordService;
            _log = log;
            _connectionString = new ConnectionStringManager().Get();
            _keywordService = keywordService;
        }

        #endregion Constructors

        #region Methods

        private DbConnection BuildConnectionString() => new SQLiteConnection(_connectionString);

        public void Clear()
        {
            using (var c = BuildConnectionString())
            {
                var sql = @"
                    delete from shortcut;
                    delete from shortcut_name;
                    delete from shortcut_usage;
                    delete from shortcut_session where name = 'SlickRun';";
                c.Execute(sql);
            }
        }

        public void Create(Shortcut s)
        {
            var sql = @"
                insert into shortcut (
                    arguments,
                    file_name,
                    notes,
                    run_as,
                    start_mode,
                    id_session
                ) values (
                    @arguments,
                    @fileName,
                    @notes,
                    @runAs,
                    @startMode,
                    @idSession
                );
                select last_insert_rowid() from shortcut;";
            var sql2 = @"insert into shortcut_name(id_shortcut, name) values(@idShortcut, @name)";
            using (var c = BuildConnectionString())
            {
                var lastId = c.Query<long>(sql, new { s.Arguments, s.FileName, s.Notes, s.RunAs, s.StartMode, s.IdSession }).FirstOrDefault();
                c.Execute(sql2, new { s.Name, IdShortcut = lastId });
            }
        }

        public void Delete(Shortcut shortcut)
        {
            var sql = @"delete from shortcut_name where id_shortcut = @id";
            var sql2 = @"delete from shortcut where id = @id";
            using (var c = BuildConnectionString())
            {
                c.Execute(sql, new { shortcut.Id });
                c.Execute(sql2, new { shortcut.Id });
            }
        }

        public void Delete(ShortcutSession session)
        {
            using (var c = BuildConnectionString())
            {
                var queries = new string[]
                {
                    @"delete from shortcut_session where id = @id",
                    @"delete from shortcut where id_session = @id"
                };
                foreach (var sql in queries) { c.Execute(sql, new { session.Id }); }

            }
        }

        public IEnumerable<ShortcutName> GetNamesOf(Shortcut shortcut)
        {
            using (var c = BuildConnectionString())
            {
                var sql = @"
                    select id          as Id
                         , name        as Name
                         , id_shortcut as IdShortcut
                    from shortcut_name
                    where id_shortcut = @idShortcut";
                var result = c.Query<ShortcutName>(sql, new { IdShortcut = shortcut.Id });
                return result;
            }
        }

        public ShortcutSession GetSession(long sessionId)
        {
            var sql = @"
                select id    as id
                     , name  as name
                     , notes as notes
                from shortcut_session
                where id = @sessionId";
            using (var c = BuildConnectionString())
            {
                try
                {
                    var result = c.Query<ShortcutSession>(sql, new { sessionId }).Single();
                    return result;
                }
                catch (InvalidOperationException ex) { throw new InvalidOperationException($"There's no session with ID '{sessionId}'", ex); }
            }
        }

        public IEnumerable<ShortcutSession> GetSessions()
        {
            var sql = @"
                select id    as id
                     , name  as name
                     , notes as notes
                from shortcut_session ";
            using (var c = BuildConnectionString())
            {
                var result = c.Query<ShortcutSession>(sql);
                return result.OrderBy(e => e.Name);
            }
        }

        public Shortcut GetShortcut(string name)
        {
            if (_keywordService.IsReserved(name)) { return Shortcut.Empty(name); }

            var sql = @"
                select n.Name       as Name
                     , s.id         as Id
                     , s.arguments  as Arguments
                     , s.file_name  as FileName
                     , s.notes      as Notes
                     , s.run_as     as RunAs
                     , s.start_mode as StartMode
                from shortcut s
                inner join shortcut_name n on s.id = n.id_shortcut
                where n.name = @name";

            using (var c = BuildConnectionString())
            {
                var result = c.Query<Shortcut>(sql, new { name })
                              .FirstOrDefault();
                return result ?? Shortcut.Empty(name);
            }
        }

        public IEnumerable<string> GetShortcutNames(long sessionId)
        {
            var sql = @"
                select sn.Name as Name
                from shortcut_name sn
                inner join shortcut s on s.id = sn.id_shortcut
                where s.id_session = @sessionId
                order by name";

            using (var c = BuildConnectionString())
            {
                var result = c.Query<string>(sql, new { sessionId }).ToList();

                if (result != null) { result.AddRange(_reservedKeywordService.GetReservedKeywords()); }
                else { result = new List<string>(); }

                return result.OrderBy(e => e);
            }
        }

        public IEnumerable<Shortcut> GetShortcuts(long sessionId)
        {
            var sql = @"
                select n.Name       as Name
                     , s.id         as Id
                     , s.arguments  as Arguments
                     , s.file_name  as FileName
                     , s.notes      as Notes
                     , s.run_as     as RunAs
                     , s.start_mode as StartMode
                from shortcut s
                inner join shortcut_name n on s.id = n.id_shortcut
                where s.id_session = @sessionId
                order by n.name      ";

            using (var c = BuildConnectionString())
            {
                var result = c.Query<Shortcut>(sql, new { sessionId });
                return result ?? new List<Shortcut>();
            }
        }

        public void SetUsage(Shortcut shortcut) => SetUsage(shortcut.Id);

        public void SetUsage(long idShortcut)
        {
            using (var c = BuildConnectionString())
            {
                var sql = @"insert into shortcut_usage (id_shortcut, time_stamp) values (@idShortcut, @now)";
                c.Execute(sql, new { idShortcut, now = DateTime.Now });
            }
        }

        public void Update(Shortcut shortcut)
        {
            var sql = @"
                update shortcut
                set
                    arguments  = @arguments,
                    file_name  = @fileName,
                    notes      = @notes,
                    run_as     = @runAs,
                    start_mode = @startMode
                where id = @id;";
            var sql2 = @"
                update shortcut_name
                set
                    name = @name
                where id_shortcut = @id";
            using (var c = BuildConnectionString())
            {
                c.Execute(sql, new { shortcut.Arguments, shortcut.FileName, shortcut.Notes, shortcut.RunAs, shortcut.StartMode, shortcut.Id });
                c.Execute(sql2, new { shortcut.Name, shortcut.Id });
            }
        }

        public void Update(IEnumerable<ShortcutName> names)
        {
            using (var c = BuildConnectionString())
            {
                var sqlInsert = @"insert into shortcut_name (name, shortcut_id) values (@name, @shortcutId)";
                var sqlUpdate = @"update shortcut_name set name = @name where id = @id";
                foreach (var name in names)
                {
                    if (name.Id == 0)
                    {
                        _log.Debug($"Insert new. id_shortcut: {name.IdShortcut} - name: {name.Name} - id: {name.Id}");
                        c.Execute(sqlInsert, new { name.Name, name.IdShortcut });
                    }
                    else
                    {
                        _log.Debug($"Update. id_shortcut: {name.IdShortcut} - name: {name.Name} - id: {name.Id}");
                        c.Execute(sqlUpdate, new { name.Name, name.Id });
                    }
                }
            }
        }

        public void Update(ShortcutSession session)
        {
            var sql = @"
                update shortcut_session
                set
                    name  = @name,
                    notes = @notes
                where id = @id";
            using (var c = BuildConnectionString())
            {
                c.Execute(sql, new { session.Id, session.Name, session.Notes });
            }
        }

        #endregion Methods
    }
}