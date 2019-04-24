using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a grid control used to communicate between children that implement <see cref="IDataConnector"/>.
    /// </summary>
    public class ChartConnectorGrid : Grid
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ChartConnectorGrid"/> class
        /// </summary>
        public ChartConnectorGrid()
        {
            AddHandler(ChartBase.DataChangedEvent, new RoutedEventHandler(DataPropertyChanged));
        }

        private void DataPropertyChanged(object sender, RoutedEventArgs e)
        {
            if (e.Source is ChartBase chart)
            {
                foreach (var child in Children.OfType<IDataConnector>())
                {
                    child.OnDataSeriesChanged(chart);
                }
            }
            e.Handled = true;
        }
    }
}
