using System.Windows;

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

        #endregion Methods
    }
}