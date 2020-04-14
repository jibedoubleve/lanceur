using Dapper;
using Probel.Lanceur.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.SQLiteDb.Services
{
	public partial class SQLiteDatabaseService
	{
		#region Methods

		public Alias GetAlias(string name, long sessionId)
		{
			if (_keywordService.IsReserved(name)) { return Alias.Reserved(name); }

			var sql = @"
                select
                    n.Name        as Name,
                    s.id          as Id,
                    s.id_session  as IdSession,
                    s.arguments   as Arguments,
                    s.file_name   as FileName,
                    s.notes       as Notes,
                    s.run_as      as RunAs,
                    s.start_mode  as StartMode,
                    s.working_dir as WorkingDirectory
                from
                    alias s
                    inner join alias_name n on s.id = n.id_alias
                where
                    n.name like @name
                    and s.id_session = @sessionId";

			using (var c = BuildConnection())
			{
				var result = c.Query<Alias>(sql, new { name, sessionId })
							  .FirstOrDefault();
				return result ?? Alias.Empty(name);
			}
		}

		public IEnumerable<Alias> GetAliases(long sessionId)
		{
			var sql = @"
                select n.Name       as Name
                     , s.id         as Id
                     , s.arguments  as Arguments
                     , s.file_name  as FileName
                     , s.notes      as Notes
                     , s.run_as     as RunAs
                     , s.start_mode as StartMode
                     , s.working_dir as WorkingDirectory
                from alias s
                left join alias_name n on s.id = n.id_alias
                where s.id_session = @sessionId
                order by n.name";

			using (var c = BuildConnection())
			{
				var result = c.Query<Alias>(sql, new { sessionId });
				return result ?? new List<Alias>();
			}
		}

		public IEnumerable<AliasText> GetAliasNames(long sessionId)
		{
			var sql = @"
                select
                	sn.Name      as Name,
                	c.exec_count as ExecutionCount,
                	s.file_name  as FileName,
                    'Rocket'     as Kind
                from
                    alias_name sn
                    inner join alias s on s.id = sn.id_alias
                    left join stat_execution_count_v c on c.id_keyword = s.id
                where
                    s.id_session = @sessionId
                order by
                    exec_count desc,
                    name       asc";

			using (var c = BuildConnection())
			{
				var result = c.Query<AliasText>(sql, new { sessionId }).ToList();

				if (result != null)
				{
					result.AddRange(_reservedKeywordService.GetKeywords());

					var items = (from i in _pluginManager.GetKeywords()
								 select (AliasText)i);

					result.AddRange(items);
				}
				else { result = new List<AliasText>(); }

				return result.OrderByDescending(e => e.ExecutionCount)
							 .ThenBy(e => e.Name);
			}
		}

		public IEnumerable<Doubloon> GetDoubloons(long idSession)
		{
			var sql = @"
                select
	                id          as Id,
	                id_session  as IdSession,
	                keywords    as Keywords,
	                file_name   as FileName,
	                arguments   as Arguments,
	                run_as      as RunAs,
	                working_dir as WorkingDir
                from data_doubloons_v
                where id_session = @idSession";

			using (var db = BuildConnection())
			{
				return db.Query<Doubloon>(sql, new { idSession });
			}
		}

		public IEnumerable<AliasName> GetNamesOf(Alias alias)
		{
			using (var c = BuildConnection())
			{
				var sql = @"
                    select id          as Id
                         , name        as Name
                         , id_alias as IdAlias
                    from alias_name
                    where id_alias = @idAlias";
				var result = c.Query<AliasName>(sql, new { IdAlias = alias.Id });
				return result;
			}
		}

		public AliasSession GetSession(long sessionId)
		{
			var sql = @"
                select id    as id
                     , name  as name
                     , notes as notes
                from alias_session
                where id = @sessionId";
			using (var c = BuildConnection())
			{
				try
				{
					var result = c.Query<AliasSession>(sql, new { sessionId }).Single();
					return result;
				}
				catch (InvalidOperationException ex) { throw new InvalidOperationException($"There's no session with ID '{sessionId}'", ex); }
			}
		}

		public AliasSession GetSession(string sessionName)
		{
			var sql = @"
                select
	                id    as id,
	                name  as name,
	                notes as notes
                from alias_session
                where lower(name) = @name";

			using (var db = BuildConnection())
			{
				return db.Query<AliasSession>(sql, new { name = sessionName.ToLower() }).FirstOrDefault();
			}
		}

		public IEnumerable<AliasSession> GetSessions()
		{
			var sql = @"
                select id    as id
                     , name  as name
                     , notes as notes
                from alias_session ";
			using (var c = BuildConnection())
			{
				var result = c.Query<AliasSession>(sql);
				return result.OrderBy(e => e.Name);
			}
		}

		#endregion Methods
	}
}