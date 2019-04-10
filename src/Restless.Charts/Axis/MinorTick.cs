namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a single minor tick on an axis.
    /// </summary>
    public class MinorTick
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MinorTick"/> class.
        /// </summary>
        /// <param name="value">The tick value.</param>
        /// <param name="coordinate">The screen coordinate that corresponds to <paramref name="value"/>.</param>
        internal MinorTick(double value, double coordinate)
        {
            Value = value;
            Coordinate = coordinate;
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the tick value.
        /// </summary>
        public double Value
        {
            get;
        }

        /// <summary>
        /// Gets the calculated coordinate.
        /// </summary>
        public double Coordinate
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a boolean value that indicates if <see cref="Coordinate"/> falls
        /// between <paramref name="minValue"/> and <paramref name="maxValue"/>, inclusive.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>
        /// true if <see cref="Coordinate"/> falls between <paramref name="minValue"/> and <paramref name="maxValue"/>, inclusive; otherwise, false.
        /// </returns>
        public bool IsCoordinateWithin(double minValue, double maxValue)
        {
            return Coordinate >= minValue && Coordinate <= maxValue;
        }

        /// <summary>
        /// Gets a boolean value that indicates if <see cref="Coordinate"/> falls
        /// between zero and <paramref name="maxValue"/>, inclusive.
        /// </summary>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if <see cref="Coordinate"/> falls between zero and <paramref name="maxValue"/>, inclusive; otherwise, false.</returns>
        public bool IsCoordinateWithin(double maxValue)
        {
            return IsCoordinateWithin(0.0, maxValue);
        }

        /// <summary>
        /// Gets a string representation of this instance.
        /// </summary>
        /// <returns>A string that describes this instance.</returns>
        public override string ToString()
        {
            return $"Value:{Value} Coordinate:{Coordinate}";
        }
        #endregion
    }
}
