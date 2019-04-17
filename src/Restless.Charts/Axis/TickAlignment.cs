namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides an enumeration that describes how ticks are aligned on an axis.
    /// </summary>
    public enum TickAlignment
    {
        /// <summary>
        /// Ticks are aligned according to range of values.
        /// </summary>
        Range,
        /// <summary>
        /// Ticks are aligned according to actual values.
        /// </summary>
        Values,
        /// <summary>
        /// The default tick alignment.
        /// </summary>
        Default = Range,
    }
}
