using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Helpers
{
    /// <remarks>https://stackoverflow.com/questions/21041875/textboxhelper-buttoncommand-binding-using-caliburn</remarks>
    public static class MahappsHelper
    {
        #region Fields

        /// <summary>
        /// Attach Caliburn cal:Message.Attach for the Mahapps TextBoxHelper.Button
        /// </summary>
        public static readonly DependencyProperty ButtonMessageProperty =
            DependencyProperty.RegisterAttached("ButtonMessage", typeof(string), typeof(MahappsHelper), new PropertyMetadata(null, ButtonMessageChanged));

        #endregion Fields

        #region Methods

        private static void ButtonMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement textBox = d as TextBox;
            if (d == null) { textBox = d as PasswordBox; }
            if (d == null) { textBox = d as ComboBox; }
            if (textBox == null) { throw new ArgumentException("ButtonMessage does not work with control " + d.GetType()); }

            textBox.Loaded -= ButtonMessageTextBox_Loaded;

            var button = GetTextBoxButton(textBox);
            if (button != null)
            {
                SetButtonMessage(textBox, button);
                return;
            }

            // cannot get button, try it in the Loaded event
            textBox.Loaded += ButtonMessageTextBox_Loaded;
        }

        private static void ButtonMessageTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox = (FrameworkElement)sender;
            textBox.Loaded -= ButtonMessageTextBox_Loaded;

            var button = GetTextBoxButton(textBox);
            SetButtonMessage(textBox, button);
        }

        private static Button GetTextBoxButton(FrameworkElement textBox) => TreeHelper.FindChild<Button>(textBox, "PART_ClearText");

        /// <summary>
        /// Set Caliburn Message to the TextBox button.
        /// </summary>
        private static void SetButtonMessage(FrameworkElement textBox, Button button)
        {
            if (button == null) { return; }

            var message = GetButtonMessage(textBox);
            Caliburn.Micro.Message.SetAttach(button, message);
        }

        public static string GetButtonMessage(DependencyObject obj) => (string)obj.GetValue(ButtonMessageProperty);

        public static void SetButtonMessage(DependencyObject obj, string value) => obj.SetValue(ButtonMessageProperty, value);

        #endregion Methods
    }
}