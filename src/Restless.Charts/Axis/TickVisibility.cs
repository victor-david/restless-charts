namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides an enumeration that describes which ticks are rendered on an axis.
    /// </summary>
    public enum TickVisibility
    {
        /// <summary>
        /// No ticks are rendered.
        /// </summary>
        None,
        /// <summary>
        /// Only major ticks are rendered.
        /// </summary>
        Major,
        /// <summary>
        /// Major and minor ticks are rendered.
        /// </summary>
        MajorMinor,
        /// <summary>
        /// Major and minor ticks are rendered. Minor edge ticks (if any are needed) are rendered.
        /// </summary>
        MajorMinorEdge,
        /// <summary>
        /// The default visibility setting.
        /// </summary>
        Default = MajorMinorEdge,
    }
}
