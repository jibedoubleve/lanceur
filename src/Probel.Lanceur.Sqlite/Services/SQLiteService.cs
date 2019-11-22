using Probel.Lanceur.Core.Models;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probel.Lanceur.Sqlite.Services
{
    public class SQLiteService : IDatabaseService
    {
        private readonly string _connectionString;

        public SQLiteService()
        {
            _connectionString = new ConnectionStringManager().Get();
        }
        public Shortcut GetShortcut(string name)
        {
            var sql = @"
                select s.id
                     , s.arguments
                     , s.file_name
                     , s.notes
                     , s.run_as
                     , s.start_mode
                     , s.id_session
                from shortcut s
                inner join shortcut_name n on s.id = n.id_shortcut
                where n.name = @name";
            using (new SQLiteDbConnection(_connectionString))
            {

            }
            return new Shortcut();
        }
        public IEnumerable<Shortcut> GetShortcuts() => throw new NotImplementedException();
    }
}
