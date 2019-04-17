using System;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Extends <see cref="MinorTick"/> to represents a single major tick on an axis.
    /// </summary>
    public class MajorTick : MinorTick
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MajorTick"/> class.
        /// </summary>
        /// <param name="value">The tick value.</param>
        /// <param name="coordinate">The screen coordinate that corresponds to <paramref name="value"/>.</param>
        /// <param name="text">The label element for the tick.</param>
        internal MajorTick(double value, double coordinate, TickText text)
            : base(value, coordinate)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the corresponding text for the major tick.
        /// </summary>
        public TickText Text
        {
            get;
        }

        /// <summary>
        /// Gets the desired width of <see cref="Text"/>.
        /// This is a shorthand property for Text.DesiredSize.Width.
        /// </summary>
        public double TextWidth
        {
            get => Text.DesiredSize.Width;
        }

        /// <summary>
        /// Gets the desired height of <see cref="Text"/>.
        /// This is a shorthand property for Text.DesiredSize.Height.
        /// </summary>
        public double TextHeight
        {
            get => Text.DesiredSize.Height;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a string representation of this instance.
        /// </summary>
        /// <returns>A string that describes this instance.</returns>
        public override string ToString()
        {
            return $"{base.ToString()} Text: {Text.Text}";
        }
        #endregion
    }
}
