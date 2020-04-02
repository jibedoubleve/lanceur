using Probel.Lanceur.Core.Entities;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public partial interface IDataSourceService
    {
        #region Methods

        IEnumerable<ChartPoint<DateTime, int>> GetChartPerDay();

        IEnumerable<ChartPoint<string, int>> GetChartPerDayOfWeek();

        IEnumerable<ChartPoint<string, int>> GetChartPerExecutionCount();

        IEnumerable<ChartPoint<DateTime, int>> GetChartPerHourInDay();

        IEnumerable<ChartPoint<DateTime, int>> GetChartPerMonth();

        #endregion Methods
    }
}