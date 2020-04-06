using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Plugin.Calculator
{
    public static class FocusOnLastBehavior
    {
        #region Fields

        public static readonly DependencyProperty FocusProperty =
            DependencyProperty.RegisterAttached(
            "Focus",
            typeof(bool),
            typeof(FocusOnLastBehavior),
            new UIPropertyMetadata(false, OnElementFocused));

        #endregion Fields

        #region Methods

        public static bool GetFocus(DependencyObject element) => (bool)element.GetValue(FocusProperty);

        public static void SetFocus(DependencyObject element, bool value) => element.SetValue(FocusProperty, value);

        private static void OnElementFocused(
            DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var element = depObj as FrameworkElement;
            if (element == null) { return; }

            element.Focus();
        }
        //static void OnElementFocused(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        //{
        //    ItemsControl itemsControl = depObj as ItemsControl;
        //    if (itemsControl == null) { return; }
        //    itemsControl.
        //    itemsControl.Loaded += (object sender, RoutedEventArgs args) =>
        //    {
        //        // get the content presented for the first listbox element
        //        var contentPresenter = (ContentPresenter)itemsControl.ItemContainerGenerator.ContainerFromIndex(0);

        //        // get the textbox and give it focus
        //        var textbox = contentPresenter.ContentTemplate.FindName("myTextBox", contentPresenter) as TextBox;
        //        textbox.Focus();
        //    };
        //}

        #endregion Methods
    }
}