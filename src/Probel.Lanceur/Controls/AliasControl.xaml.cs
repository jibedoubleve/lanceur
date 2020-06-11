using MahApps.Metro.IconPacks;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Probel.Lanceur.Controls
{
    /// <summary>
    /// Interaction logic for AliasControl.xaml
    /// </summary>
    public partial class AliasControl : UserControl
    {
        #region Fields

        public static DependencyProperty AliasFileNameProperty =
            DependencyProperty.Register("AliasFileName",
                typeof(string),
                typeof(AliasControl),
                new PropertyMetadata(null, OnAliasFileNameChanged));

        public static DependencyProperty AliasNameProperty =
            DependencyProperty.Register("AliasName",
                typeof(string),
                typeof(AliasControl),
                new PropertyMetadata(null, OnAliasNameChanged));

        public static DependencyProperty ExecutionCountProperty =
            DependencyProperty.Register("ExecutionCount",
                typeof(long),
                typeof(AliasControl),
                new PropertyMetadata(0L, OnExecutionCountChanged));

        public static DependencyProperty ImageProperty =
            DependencyProperty.Register("Image",
                typeof(ImageSource),
                typeof(AliasControl),
                new PropertyMetadata(null, OnImageChanged));

        public static DependencyProperty KindProperty =
            DependencyProperty.Register("Kind",
                typeof(string),
                typeof(AliasControl),
                new PropertyMetadata(null, OnKindChanged));

        #endregion Fields

        #region Constructors

        public AliasControl()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string AliasFileName
        {
            get => (string)GetValue(AliasFileNameProperty);
            set => SetValue(AliasFileNameProperty, value);
        }

        public string AliasName
        {
            get => (string)GetValue(AliasNameProperty);
            set => SetValue(AliasNameProperty, value);
        }

        public string ExecutionCount
        {
            get => (string)GetValue(ExecutionCountProperty);
            set => SetValue(ExecutionCountProperty, value);
        }

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

        #endregion Properties

        #region Methods

        private static void OnAliasFileNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is AliasControl ctrl && e.NewValue is string str)
            {
                ctrl.CtrlFileName.Text = str;
            }
        }

        private static void OnAliasNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is AliasControl ctrl && e.NewValue is string str)
            {
                ctrl.CtrlName.Text = str;
            }
        }

        private static void OnExecutionCountChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is AliasControl ctrl)
            {
                if (e.NewValue is long ct)
                {
                    if (ct == 0) { ctrl.CtrlCounter.Visibility = Visibility.Collapsed; }
                    else
                    {
                        ctrl.CtrlCount.Text = ct.ToString();
                        ctrl.CtrlCounter.Visibility = Visibility.Visible;
                    }
                }
                else { ctrl.CtrlCounter.Visibility = Visibility.Collapsed; }
            }
        }

        private static void OnImageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is AliasControl ctrl)
            {
                if (e.NewValue is ImageSource src && e.NewValue != null)
                {
                    ctrl.CtrlImage.Source = src;
                    ShowImage(ctrl);
                }
                else { ShowIcon(ctrl); }
            }
        }

        private static void OnKindChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is AliasControl ctrl)
            {
                if (e.NewValue is string str)
                {


                    if (ctrl.CtrlImage.Source != null)
                    {
                        ShowImage(ctrl);
                    }
                    else {

                        var kind = (from k in Enum.GetValues(typeof(PackIconMaterialKind)).Cast<PackIconMaterialKind>()
                                    where k.ToString().ToLower() == str.ToLower()
                                    select k).ToList();

                        ctrl.CtrlIcon.Kind = kind.Any() ? kind[0] : PackIconMaterialKind.None;

                        ShowIcon(ctrl); }
                }
            }
        }

        private static void ShowIcon(AliasControl ctrl)
        {
            ctrl.CtrlImage.Visibility = Visibility.Collapsed;
            ctrl.CtrlIcon.Visibility = Visibility.Visible;
        }
        private static void ShowImage(AliasControl ctrl)
        {
            ctrl.CtrlImage.Visibility = Visibility.Visible;
            ctrl.CtrlIcon.Visibility = Visibility.Collapsed;
        }

        #endregion Methods
    }
}