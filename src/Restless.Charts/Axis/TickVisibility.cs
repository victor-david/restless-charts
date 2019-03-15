using System;

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
        /// Major ticks are rendered.
        /// </summary>
        MajorOnly,
        /// <summary>
        /// Major and minor ticks are rendered
        /// </summary>
        MajorAndMinor,
        /// <summary>
        /// The default visibility setting.
        /// </summary>
        Default = MajorAndMinor,
    }
}
