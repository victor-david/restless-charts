namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides an enumeration that describes the visual aspect of a <see cref="LineChart"/>.
    /// </summary>
    public enum LineChartStyle
    {
        /// <summary>
        /// The line chart is displayed with lines only.
        /// </summary>
        Standard,
        /// <summary>
        /// The line chart is displayed with lines and data points as circles.
        /// </summary>
        StandardCirclePoint,
        /// <summary>
        /// The line chart is displayed with lines and data points as squares.
        /// </summary>
        StandardSquarePoint,
        /// <summary>
        /// The line chart is displayed as a filled shape.
        /// </summary>
        Filled,
    }
}
