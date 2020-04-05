using Probel.Lanceur.Core.Plugins;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Probel.Lanceur.Controls
{
    /// <summary>
    /// Interaction logic for PluginConfigControl.xaml
    /// </summary>
    public partial class PluginConfigControl : UserControl
    {
        #region Fields

        public static DependencyProperty PluginConfigProperty =
            DependencyProperty.Register("PluginConfig",
                typeof(PluginConfig),
                typeof(PluginConfigControl),
                null);

        //private static void OnPluginConfigChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if(sender is PluginConfigControl ctrl)
        //    {
        //        ctrl.
        //    }
        //}

        #endregion Fields

        #region Constructors

        public PluginConfigControl()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public PluginConfig PluginConfig
        {
            get => (PluginConfig)GetValue(PluginConfigProperty);
            set => SetValue(PluginConfigProperty, value);
        }

        #endregion Properties
    }
}