using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a chart legend.
    /// </summary>
    public class ChartLegend : ListBox, IDataConnector
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartLegend"/> class.
        /// </summary>
        public ChartLegend()
        {
            var b = new Binding(nameof(IsInteractive))
            {
                Source = this,
                Mode = BindingMode.TwoWay,
            };
            SetBinding(IsEnabledProperty, b);

            CommandBindings.Add(new CommandBinding(ClearLegendSelectionCommand, ExecuteClearLegendSelectionCommand));
        }

        static ChartLegend()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartLegend), new FrameworkPropertyMetadata(typeof(ChartLegend)));
        }
        #endregion

        /************************************************************************/

        #region HeaderText
        /// <summary>
        /// Gets or sets the header text
        /// </summary>
        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HeaderText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register
            (
                nameof(HeaderText), typeof(string), typeof(ChartLegend), new PropertyMetadata(null, OnHeaderTextChanged)
            );

        private static void OnHeaderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartLegend c)
            {
                c.HeaderTextVisibility = string.IsNullOrEmpty(c.HeaderText) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// Gets or sets the foreground brush for <see cref="HeaderText"/>.
        /// </summary>
        public Brush HeaderTextForeground
        {
            get => (Brush)GetValue(HeaderTextForegroundProperty);
            set => SetValue(HeaderTextForegroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HeaderTextForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTextForegroundProperty = DependencyProperty.Register
            (
                nameof(HeaderTextForeground), typeof(Brush), typeof(ChartLegend), new PropertyMetadata(Brushes.Black)
            );

        /// <summary>
        /// Gets the margin that is applied to <see cref="HeaderText"/>
        /// </summary>
        public Visibility HeaderTextVisibility
        {
            get => (Visibility)GetValue(HeaderTextVisibilityProperty);
            private set => SetValue(HeaderTextVisibilityPropertyKey, value);
        }

        private static readonly DependencyPropertyKey HeaderTextVisibilityPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(HeaderTextVisibility), typeof(Visibility), typeof(ChartLegend), new PropertyMetadata(Visibility.Collapsed)
            );

        /// <summary>
        /// Identifies the <see cref="HeaderTextVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTextVisibilityProperty = HeaderTextVisibilityPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/
        #region IsInteractive
        /// <summary>
        /// Gets or sets a value that determines if the legend is interactive.
        /// </summary>
        public bool IsInteractive
        {
            get => (bool)GetValue(IsInteractiveProperty);
            set => SetValue(IsInteractiveProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsInteractive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInteractiveProperty = DependencyProperty.Register
            (
                nameof(IsInteractive), typeof(bool), typeof(ChartLegend), new PropertyMetadata(true, OnIsInteractiveChanged)
            );

        private static void OnIsInteractiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Currently, IsInteractive is synonymous with IsEnabled. The two are bound together.
            // However, IsInteractive is more intuitive in the client's XAML, and we might want to
            // perform some other tasks or functionality in the future around IsInteractive.
            if (d is ChartLegend c)
            {
            }
        }
        #endregion

        /************************************************************************/

        #region ClearLegendSelection
        /// <summary>
        /// Gets the command used to clear the legend selection.
        /// </summary>
        public static readonly RoutedCommand ClearLegendSelectionCommand = new RoutedCommand();

        private void ExecuteClearLegendSelectionCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SelectedItem = null;
        }


        #endregion

        /************************************************************************/

        #region IDataConnector
        /// <summary>
        /// Called when <see cref="DataSeries"/> has changed.
        /// </summary>
        /// <param name="chart">The chart</param>
        public void OnDataSeriesChanged(ChartBase chart)
        {
            if (chart != null && chart.Data != null)
            {
                if (chart is PieChart pie)
                {
                    pie.AdjustDataSeriesIf();
                }
                ItemsSource = chart.Data.DataInfo;
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        #endregion
    }
}
