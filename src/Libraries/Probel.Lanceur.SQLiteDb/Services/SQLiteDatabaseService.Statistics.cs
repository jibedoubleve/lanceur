using Dapper;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.SQLiteDb.Services
{
    public partial class SQLiteDatabaseService : IDataSourceService
    {
        #region Methods

        public IEnumerable<ChartPoint<DateTime, int>> GetChartPerDay(long idSession)
        {
            var sql = @"
                select
                    day        as X,
                    exec_count as Y
                from stat_usage_per_day_v
                where id_session = @idSession";

            return GetChart<DateTime>(sql, new { idSession });
        }

        public IEnumerable<ChartPoint<string, int>> GetChartPerDayOfWeek(long idSession)
        {
            var sql = @"
                select
                   day_name   as X,
                   exec_count as Y
                from stat_usage_per_day_of_week_v
                where id_session = @idSession";

            return GetChart<string>(sql, new { idSession });
        }

        public IEnumerable<ChartPoint<string, int>> GetChartPerExecutionCount(long idSession)
        {
            var sql = @"
                select
                   keywords   as X,
                   exec_count as Y
                from stat_execution_count_v
                where id_session = @idSession";

            return GetChart<string>(sql, new { idSession });
        }

        public IEnumerable<ChartPoint<DateTime, int>> GetChartPerHourInDay(long idSession)
        {
            var sql = @"
                select
                   hour_in_day as X,
                    exec_count as Y
                from stat_usage_per_hour_in_day_v 
                where id_session = @idSession ";
            return GetChart<DateTime>(sql, new { idSession });
        }

        public IEnumerable<ChartPoint<DateTime, int>> GetChartPerMonth(long idSession)
        {
            var sql = @"
                select
                    month      as X,
                    exec_count as Y
                from stat_usage_per_month_v
                where id_session = @idSession";
            
            return GetChart<DateTime>(sql, new { idSession });
        }

        private IEnumerable<ChartPoint<Ty, int>> GetChart<Ty>(string sql, object parameters = null)
        {
            using (var c = BuildConnection())
            {
                var result = (parameters == null)
                    ? c.Query<ChartPoint<Ty, int>>(sql).ToList()
                    : c.Query<ChartPoint<Ty, int>>(sql, parameters).ToList();
                return result;
            }
        }        

        #endregion Methods
    }
}