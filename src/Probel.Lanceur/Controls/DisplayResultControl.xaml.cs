using MahApps.Metro.IconPacks;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Probel.Lanceur.Controls
{
    /// <summary>
    /// Interaction logic for DisplayResultControl.xaml
    /// </summary>
    public partial class DisplayResultControl : UserControl
    {
        #region Fields

        public static DependencyProperty ImageProperty =
            DependencyProperty.Register("Image",
                typeof(ImageSource),
                typeof(DisplayResultControl),
                new PropertyMetadata(null, OnImageChanged));

        public static DependencyProperty KindProperty =
            DependencyProperty.Register("Kind",
                typeof(string),
                typeof(DisplayResultControl),
                new PropertyMetadata(null, OnKindChanged));

        public static DependencyProperty SubtitleProperty =
            DependencyProperty.Register("Subtitle",
                typeof(string),
                typeof(DisplayResultControl),
                new PropertyMetadata(null, OnSubtitleChanged));

        public static DependencyProperty TitleProperty =
                            DependencyProperty.Register("Title",
                typeof(string),
                typeof(DisplayResultControl),
                new PropertyMetadata(null, OnTitleChanged));

        #endregion Fields

        #region Constructors

        public DisplayResultControl()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public string Kind
        {
            get => (string)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }

        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        #endregion Properties

        #region Methods

        private static void OnImageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DisplayResultControl ctrl)
            {
                if (e.NewValue is ImageSource src && e.NewValue != null)
                {
                    ctrl.CtrlImage.Source = src;
                    ctrl.CtrlImage.Visibility = Visibility.Visible;
                }
                else { ctrl.CtrlImage.Visibility = Visibility.Collapsed; }
            }
        }

        private static void OnKindChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DisplayResultControl ctrl)
            {
                if (e.NewValue is string str && !string.IsNullOrEmpty(str))
                {
                    var kind = (from k in Enum.GetValues(typeof(PackIconMaterialKind)).Cast<PackIconMaterialKind>()
                                where k.ToString().ToLower() == str.ToLower()
                                select k).ToList();
                    ctrl.CtrlIcon.Kind = kind.Any() ? kind[0] : PackIconMaterialKind.None;
                    ctrl.CtrlIcon.Visibility = Visibility.Visible;
                }
                else { ctrl.CtrlIcon.Visibility = Visibility.Collapsed; }
            }
        }

        private static void OnSubtitleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DisplayResultControl ctrl && e.NewValue is string str)
            {
                ctrl.CtrlSubtitle.Text = str;
            }
        }

        private static void OnTitleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DisplayResultControl ctrl && e.NewValue is string str)
            {
                ctrl.CtrlTitle.Text = str;
            }
        }

        #endregion Methods
    }
}