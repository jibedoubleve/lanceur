using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Controls
{
    /// <summary>
    /// Interaction logic for HeaderSlider.xaml
    /// </summary>
    public partial class HeaderSlider : UserControl
    {
        #region Fields

        public static DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(string),
            typeof(HeaderSlider),
            null);

        public static DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof(int),
            typeof(HeaderSlider),
            null);

        public static DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof(int),
            typeof(HeaderSlider),
            null);

        public static DependencyProperty TickFrequencyProperty = DependencyProperty.Register(
            "TickFrequency",
            typeof(int),
            typeof(HeaderSlider),
            null);

        public static DependencyProperty UnitProperty = DependencyProperty.Register(
            "Unit",
            typeof(string),
            typeof(HeaderSlider),
            null);

        public static DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(int),
            typeof(HeaderSlider),
            null);

        #endregion Fields

        #region Constructors

        public HeaderSlider()
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

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public int Minimum
        {
            get => (int)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public int TickFrequency
        {
            get => (int)GetValue(TickFrequencyProperty);
            set => SetValue(TickFrequencyProperty, value);
        }

        public string Unit
        {
            get => (string)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        #endregion Properties
    }
}