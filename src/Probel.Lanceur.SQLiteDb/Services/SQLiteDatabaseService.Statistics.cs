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

        public IEnumerable<ChartPoint<DateTime, int>> GetChartPerDay()
        {
            var sql = @"
                select
                    day        as X,
                    exec_count as Y
                from stat_usage_per_day_v";

            return GetChart<DateTime>(sql);
        }

        public IEnumerable<ChartPoint<string, int>> GetChartPerDayOfWeek()
        {
            var sql = @"
                select
                   day_name   as X,
                   exec_count as Y
                from stat_usage_per_day_of_week_v";

            return GetChart<string>(sql);
        }

        public IEnumerable<ChartPoint<string, int>> GetChartPerExecutionCount()
        {
            var sql = @"
                select
                   keywords   as X,
                   exec_count as Y
                from stat_execution_count_v";

            return GetChart<string>(sql);
        }

        public IEnumerable<ChartPoint<DateTime, int>> GetChartPerHourInDay()
        {
            var sql = @"
                select
                   hour_in_day as X,
                    exec_count as Y
                from stat_usage_per_hour_in_day_v  ";
            return GetChart<DateTime>(sql);
        }

        public IEnumerable<ChartPoint<DateTime, int>> GetChartPerMonth()
        {
            var sql = @"
                select
                    month      as X,
                    exec_count as Y
                from stat_usage_per_month_v";

            return GetChart<DateTime>(sql);
        }

        private IEnumerable<ChartPoint<Ty, int>> GetChart<Ty>(string sql)
        {
            using (var c = BuildConnection())
            {
                var result = c.Query<ChartPoint<Ty, int>>(sql).ToList();
                return result;
            }
        }

        #endregion Methods
    }
}