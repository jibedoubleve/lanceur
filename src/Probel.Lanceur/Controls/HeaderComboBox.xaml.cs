using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Controls
{
    /// <summary>
    /// Interaction logic for HeaderComboBox.xaml
    /// </summary>
    public partial class HeaderComboBox : UserControl
    {
        #region Fields

        public static DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
            "DisplayMemberPath",
            typeof(string),
            typeof(HeaderComboBox),
            null);

        public static DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(string),
            typeof(HeaderComboBox),
            null);

        public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(HeaderComboBox),
            null);

        public static DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(HeaderComboBox),
            null);

        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(object),
            typeof(HeaderComboBox),
            null);

        #endregion Fields

        #region Constructors

        public HeaderComboBox()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (string)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        #endregion Properties
    }
}