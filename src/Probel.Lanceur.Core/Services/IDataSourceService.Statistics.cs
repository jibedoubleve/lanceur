using Probel.Lanceur.Core.Entities;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public partial interface IDataSourceService
    {
        #region Methods

        IEnumerable<ChartPoint<DateTime, int>> GetChartPerDay(long idSession);

        IEnumerable<ChartPoint<string, int>> GetChartPerDayOfWeek(long idSession);

        IEnumerable<ChartPoint<string, int>> GetChartPerExecutionCount(long idSession);

        IEnumerable<ChartPoint<DateTime, int>> GetChartPerHourInDay(long idSession);

        IEnumerable<ChartPoint<DateTime, int>> GetChartPerMonth(long idSession);

        #endregion Methods
    }
}