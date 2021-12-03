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
            if (item is Query) { return AliasTemplate; }
            else if (item is AliasTextModel) { return AliasTemplate; }
            else if (item is ResultItem) { return ReadOnlyTemplate; }
            else { throw new NotSupportedException($"There is no template for {item.GetType()}"); }
        }

        #endregion Methods
    }
}