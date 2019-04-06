using System;
using System.Windows;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a single major axis tick.
    /// </summary>
    public class MajorTick
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MajorTick"/> class.
        /// </summary>
        /// <param name="value">The tick value.</param>
        /// <param name="text">The label element for the tick.</param>
        internal MajorTick(double value, double coordinate, TickText text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Value = value;
            Coordinate = coordinate; // Math.Round(coordinate);
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the tick value for the major tick.
        /// </summary>
        public double Value
        {
            get;
        }

        /// <summary>
        /// Gets the calculated coordinate. This value is rounded to zero decimal places.
        /// </summary>
        public double Coordinate
        {
            get;
        }

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

        #region Public methods
        /// <summary>
        /// Gets a boolean value that indicates if <see cref="Coordinate"/> falls
        /// between zero and <paramref name="maxValue"/>, inclusive.
        /// </summary>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if <see cref="Coordinate"/> falls between zero and <paramref name="maxValue"/>, inclusive; otherwise, false.</returns>
        public bool IsCoordinateWithin(double maxValue)
        {
            return Coordinate >= 0.0 && Coordinate <= maxValue;
        }

        /// <summary>
        /// Gets a string representation of this instance.
        /// </summary>
        /// <returns>A string that describes this instance.</returns>
        public override string ToString()
        {
            return $"Value:{Value} Text:{Text.Text}";
        }
        #endregion
    }
}
