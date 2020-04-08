using Probel.Lanceur.Models;
using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Templating
{
    public class SessionTemplateSelector : DataTemplateSelector
    {
        #region Properties

        public DataTemplate EditSessionTemplate { get; set; }
        public DataTemplate NewSessionTemplate { get; set; }

        #endregion Properties

        #region Methods

        public override DataTemplate SelectTemplate(object item, DependencyObject container) => (item is NewAliasSessionModel) ? NewSessionTemplate : EditSessionTemplate;

        #endregion Methods
    }
}