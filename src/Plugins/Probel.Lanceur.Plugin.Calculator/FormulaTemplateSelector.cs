using Probel.Lanceur.Plugin.Calculator.Models;
using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Plugin.Calculator
{
    public class FormulaTemplateSelector : DataTemplateSelector
    {
        private const string ResultTemplate = nameof(ResultTemplate);
        private const string CalculationTemplate = nameof(CalculationTemplate);
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;
            if (element != null && item != null && item is ValueItem vi)
            {
                return (vi.IsResult)
                    ? element.FindResource(ResultTemplate) as DataTemplate
                    : element.FindResource(CalculationTemplate) as DataTemplate;
            }
            else { return null; }
        }
    }
}