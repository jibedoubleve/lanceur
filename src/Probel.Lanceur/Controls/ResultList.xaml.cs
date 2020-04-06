using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Events;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Probel.Lanceur.Controls
{
    /// <summary>
    /// Interaction logic for ResultList.xaml
    /// </summary>
    public partial class ResultList : UserControl
    {
        #region Fields

        public static DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath",
                typeof(string),
                typeof(ResultList),
                new PropertyMetadata(null, OnDisplayMemberPathChanged));

        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource",
                typeof(IEnumerable),
                typeof(ResultList),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public static DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate",
                typeof(DataTemplate),
                typeof(ResultList),
                new PropertyMetadata(null, OnItemTemplateChenged));

        public static DependencyProperty SelectedItemProperty =
                    DependencyProperty.Register("SelectedItem",
                typeof(object),
                typeof(ResultList),
                new PropertyMetadata(null, OnSelectedItemChanged));

        #endregion Fields

        #region Constructors

        public ResultList()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Events

        public event EventHandler<AliasTextEventArgs> AliasClicked;

        public event EventHandler<AliasTextEventArgs> SelectionChanged;

        #endregion Events

        #region Properties

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            private set => SetValue(SelectedItemProperty, value);
        }

        #endregion Properties

        #region Methods

        public void MoveSelection(int offset)
        {
            var n = Results?.Items?.Count ?? 0;
            var index = Results.SelectedIndex + offset;

            if (n > 0) { index = (n + index) % n; }
            else { index = -1; }

            Results.SelectedIndex = index;
            if (index >= 0 && Results != null)
            {
                Results.SelectedItem = Results?.Items[index];
                Results.ScrollIntoView(Results.SelectedItem);
            }
        }

        public void SelectFirst()
        {
            if (Results.Items.Count > 0)
            {
                Results.SelectedIndex = 0;
                Results.SelectedItem = Results.Items[0];
            }
        }

        public void SelectNextItem() => MoveSelection(-1);

        public void SelectPreviousItem() => MoveSelection(1);

        private static void OnDisplayMemberPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string str && sender is ResultList rl)
            {
                rl.Results.DisplayMemberPath = str;
            }
        }

        private static void OnItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IEnumerable items && sender is ResultList rl)
            {
                rl.Results.ItemsSource = items;
            }
        }

        private static void OnItemTemplateChenged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DataTemplate dt && sender is ResultList rl)
            {
                rl.Results.ItemTemplate = dt;
            }
        }

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ResultList rl && e.NewValue != null)
            {
                rl.SelectedItem = rl.Results.SelectedItem;
            }
        }

        private void OnAliasClicked()
        {
            var alias = Results.SelectedItem as AliasText ?? new AliasText();
            AliasClicked?.Invoke(this, new AliasTextEventArgs(alias));
        }

        private void OnResultsMouseClick(object sender, MouseButtonEventArgs e) => OnAliasClicked();

        private void OnResultsSelectionChanged(object sender, SelectionChangedEventArgs e) => SelectedItem = Results.SelectedItem;

        #endregion Methods
    }
}