namespace Restless.Controls.Chart
{
    /// <summary>
    /// Specifies placement and orientation for axes.
    /// </summary>
    public enum AxisPlacement
    {
        /// <summary>
        /// Axis will be vertical and ticks will be aligned to right.
        /// </summary>
        Left,
        /// <summary>
        /// Axis will be vertical and ticks will be aligned to left.
        /// </summary>
        Right,
        /// <summary>
        /// Axis will be horizontal and ticks will be aligned to bottom.
        /// </summary>
        Top,
        /// <summary>
        /// Axis will be horizontal and ticks will be aligned to top.
        /// </summary>
        Bottom,
        /// <summary>
        /// Default placement for X axis, bottom.
        /// </summary>
        DefaultX = Bottom,
        /// <summary>
        /// Default placement for Y axis, left.
        /// </summary>
        DefaultY = Left,
    }
}
