using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a grid control used to communicate between children that implement <see cref="IDataConnector"/>.
    /// </summary>
    public class ChartConnectorGrid : Grid
    {
        public ChartConnectorGrid()
        {
            AddHandler(ChartBase.DataChangedEvent, new RoutedEventHandler(DataPropertyChanged));
        }

        private void DataPropertyChanged(object sender, RoutedEventArgs e)
        {
            if (e.Source is ChartBase chart)
            {
                DebugHelper.DisplayRoutedEvent(sender, e);
                foreach (var child in Children.OfType<IDataConnector>())
                {
                    child.OnDataSeriesChanged(chart);
                }
            }
            e.Handled = true;
        }
    }
}
