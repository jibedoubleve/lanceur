using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.DataTemplateSelectors
{
    public class ResultTemplateSelector : DataTemplateSelector
    {
        #region Properties

        public DataTemplate AliasTemplate { get; set; }
        public DataTemplate ReadOnlyTemplate { get; set; }

        #endregion Properties

        #region Methods

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                AliasTextModel => AliasTemplate,
                Query => AliasTemplate,
                ResultItem => ReadOnlyTemplate,
                _ => throw new NotSupportedException($"There is no template for {item.GetType()}")
            };
        }

        #endregion Methods
    }
}