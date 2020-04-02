using Caliburn.Micro;
using Humanizer;
using NCalc;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin.Calculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Probel.Lanceur.Plugin.Calculator.ViewModels
{
    public class ResultViewModel : Screen
    {
        #region Fields

        private ObservableCollection<ValueItem> _items = new ObservableCollection<ValueItem>();

        #endregion Fields

        #region Properties

        public ObservableCollection<ValueItem> Items
        {
            get => _items;
            set => Set(ref _items, value, nameof(Items));
        }

        public ILogService Log { get; internal set; }

        #endregion Properties

        #region Methods

        internal void Process(string parameters)
        {
            try
            {
                var left = GetLastResult();
                var right = parameters.Transform(To.LowerCase, To.TitleCase);

                var formula = left + right;
                var expression = new Expression(formula);
                var result = expression.Evaluate().ToString();

                Log.Debug($"Evaluating {formula} -  Result: {result}");

                ResulsAsReadonly();
                Items.Add(ValueItem.Result(result));
                Items.Add(ValueItem.Calculation());
            }
            catch (Exception ex) { Log.Warning($"An error occured while executing a calculation. Ex: {ex.Message}", ex); }
        }

        private string GetLastResult()
        {
            var res = (from i in Items
                       where i.IsResult
                       select i.Expression).LastOrDefault() ?? string.Empty;

            if (float.TryParse(res, out var value)) { return value.ToString("0.0", CultureInfo.CreateSpecificCulture("en")); }
            else { return res; }
        }

        private void ResulsAsReadonly()
        {
            var list = new List<ValueItem>();
            foreach (var item in Items)
            {
                item.IsReadOnly = true;
                Log.Trace($"IsResult: {item.IsResult} - Expression: {item.Expression}");
                list.Add(item);
            }
            Items = new ObservableCollection<ValueItem>(list);
        }

        #endregion Methods
    }
}