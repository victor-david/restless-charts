using System;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides a converter that divides the value by a specified amount. See remarks for more.
    /// </summary>
    /// <remarks>
    /// This converter can be used in scenarios where you want an axis to be displayed with decimal places.
    /// For instance, if you want values on the Y axis to be displayed as 19.0, 19.5, 20.0, 20.5, etc., you
    /// can multiply the Y values that go to the <see cref="DataSeries"/> by 10, and use a <see cref="DivisionConverter"/>
    /// on the Y axis to display thos values divided by 10.
    /// </remarks>
    public class DivisionConverter : IDoubleConverter
    {
        #region Private
        private readonly double divisor;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MappedValueConverter"/> class.
        /// </summary>
        /// <param name="divisor">The amount to divide the value to be converted by.</param>
        public DivisionConverter(double divisor)
        {
            if (divisor == 0) throw new ArgumentException("Divisor cannot be zero");
            this.divisor = divisor;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Converts the specified value to its mapped string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="format">The format to value after it has been divided by the divisor.</param>
        /// <returns>The formatted string.</returns>
        public string Convert(double value, string format)
        {
            value = value / divisor;
            return value.ToString(format);
        }
        #endregion
    }
}
