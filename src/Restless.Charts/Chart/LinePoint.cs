namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides an enumeration that describes how data points are represented on a <see cref="LineChart"/>.
    /// </summary>
    public enum LinePoint
    {
        /// <summary>
        /// None. No data points are created, only lines.
        /// </summary>
        None,
        /// <summary>
        /// Data points are circles.
        /// </summary>
        Circle,
        /// <summary>
        /// Date points are squares.
        /// </summary>
        Square,
    }
}
