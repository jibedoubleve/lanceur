using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Controls
{
    /// <summary>
    /// Interaction logic for HeaderTextBox.xaml
    /// </summary>
    public partial class HeaderTextBox : UserControl
    {
        #region Fields

        public static DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(string),
            typeof(HeaderTextBox),
            null);

        public static DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(HeaderTextBox),
            null);

        #endregion Fields

        #region Constructors

        public HeaderTextBox()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion Properties
    }
}