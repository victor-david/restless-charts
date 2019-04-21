using System.Windows;
using System.Windows.Controls;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a chart legend.
    /// </summary>
    public class ChartLegend : ContentControl
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartLegend"/> class.
        /// </summary>
        public ChartLegend()
        {

        }
        #endregion

        /************************************************************************/

        #region Data
        /// <summary>
        /// Gets or sets the data series for the legend.
        /// </summary>
        public DataSeries Data
        {
            get => (DataSeries)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Data"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register
            (
                nameof(Data), typeof(DataSeries), typeof(ChartLegend), new PropertyMetadata(null, OnDataChanged)
            );

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartLegend c && c.Data != null)
            {
                c.CreateLegend();
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Creates the legend from the data.
        /// </summary>
        private void CreateLegend()
        {
            double size = 24;

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(size, GridUnitType.Pixel)
            });
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            int rowIdx = 0;
            foreach (DataSeriesInfo info in Data.DataInfo)
            {
                grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(size, GridUnitType.Pixel)
                });
                Border b = new Border()
                {
                    Background = info.DataBrush,
                    Margin = new Thickness(2),
                    CornerRadius = new CornerRadius(2)
                };
                b.SetValue(Grid.RowProperty, rowIdx);
                grid.Children.Add(b);

                TextBlock text = new TextBlock()
                {
                    Text = info.Name,
                    Margin = new Thickness(2),
                    VerticalAlignment = VerticalAlignment.Center,
                };
                text.SetValue(Grid.RowProperty, rowIdx);
                text.SetValue(Grid.ColumnProperty, 1);
                grid.Children.Add(text);
                rowIdx++;
            }
            Content = grid;
        }
        #endregion
    }
}
