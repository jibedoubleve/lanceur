using Caliburn.Micro;
using LiveCharts;
using LiveCharts.Configurations;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Lanceur.ViewModels
{
    public class StatisticsViewModel : Screen
    {
        #region Fields

        private readonly ILogService _log;
        private readonly IDataSourceService _service;

        private ChartValues<ChartPoint<DateTime, int>> _chartPerDay;
        private ChartValues<int> _chartPerDayOfWeek;
        private ObservableCollection<ChartPoint<string, int>> _aliasPerExecutionCount;
        private ChartValues<int> _chartPerHourInDay;
        private ChartValues<int> _chartPerMonth;

        #endregion Fields

        #region Constructors

        public StatisticsViewModel(ILogService log, IDataSourceService service)
        {
            _log = log;
            _service = service;

            FormatterDay = m => new DateTime((long)m).ToString("dd MMM yyyy");
            FormatterMonth = m => new DateTime((long)m).ToString("MMM yyyy");
            FormatterHour = m => new DateTime((long)m).ToString("HH:00");

            FormatterCount = m => $"{m} hit(s)";

            XyDateTimeMapper = Mappers.Xy<ChartPoint<DateTime, int>>()
                .X(p => p.X.Ticks)
                .Y(p => p.Y);
        }

        #endregion Constructors

        #region Properties

        public ChartValues<ChartPoint<DateTime, int>> ChartPerDay
        {
            get => _chartPerDay;
            set => Set(ref _chartPerDay, value, nameof(ChartPerDay));
        }

        public ChartValues<int> ChartPerDayOfWeek
        {
            get => _chartPerDayOfWeek;
            set => Set(ref _chartPerDayOfWeek, value, nameof(ChartPerDayOfWeek));
        }

        public ObservableCollection<ChartPoint<string, int>> AliasPerExecutionCount
        {
            get => _aliasPerExecutionCount;
            set => Set(ref _aliasPerExecutionCount, value, nameof(AliasPerExecutionCount));
        }

        public ChartValues<int> ChartPerHourInDay
        {
            get => _chartPerHourInDay;
            set => Set(ref _chartPerHourInDay, value, nameof(ChartPerHourInDay));
        }

        public ChartValues<int> ChartPerMonth
        {
            get => _chartPerMonth;
            set => Set(ref _chartPerMonth, value, nameof(ChartPerMonth));
        }

        public Func<int, string> FormatterCount { get; }

        public Func<double, string> FormatterMonth { get; }

        public Func<double, string> FormatterHour { get; }

        public Func<double, string> FormatterDay { get; }

        public ObservableCollection<string> LabelsDayOfWeek { get; set; }

        public ObservableCollection<string> LabelsHour { get; set; }

        public ObservableCollection<string> LabelsMonths { get; set; }

        public object XyDateTimeMapper { get; }

        public object XyStringMapper { get; }

        #endregion Properties

        #region Methods

        protected override void OnActivate()
        {
            _log.Trace("Loading statistics");
            var t1 = Task.Run(() => _service.GetChartPerDay());
            var t2 = Task.Run(() => _service.GetChartPerHourInDay());
            var t3 = Task.Run(() => _service.GetChartPerMonth());
            var t4 = Task.Run(() => _service.GetChartPerDayOfWeek());
            var t5 = Task.Run(() => _service.GetChartPerExecutionCount().OrderBy(e => e.Y));

            Task.WaitAll(t1, t2, t3, t4, t5);

            ChartPerDay = new ChartValues<ChartPoint<DateTime, int>>(t1.Result);

            ChartPerMonth = new ChartValues<int>(t3.Result.Select(e => e.Y));
            LabelsMonths = new ObservableCollection<string>(t3.Result.Select(e => e.X.ToString("MMM yyyy")));

            ChartPerHourInDay = new ChartValues<int>(t2.Result.Select(e => e.Y));
            LabelsHour = new ObservableCollection<string>(t2.Result.Select(e => e.X.ToString("HH:00")));


            ChartPerDayOfWeek = new ChartValues<int>(t4.Result.Select(e => e.Y));
            LabelsDayOfWeek = new ObservableCollection<string>(t4.Result.Select(e => e.X));

            AliasPerExecutionCount = new ObservableCollection<ChartPoint<string, int>>(t5.Result.OrderByDescending(e => e.Y));
        }

        #endregion Methods
    }
}