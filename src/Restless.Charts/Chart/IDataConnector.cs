namespace Restless.Controls.Chart
{
    /// <summary>
    /// Defines interface methods for components that need to be notified of changes to data.
    /// </summary>
    public interface IDataConnector
    {
        /// <summary>
        /// Called when the chart data changes.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void OnDataSeriesChanged(ChartBase chart);
    }
}
