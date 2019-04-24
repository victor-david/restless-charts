using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a single data point on the Y axis
    /// </summary>
    public class DataPointY
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointY"/> class.
        /// </summary>
        /// <param name="seriesIndex">The series index.</param>
        /// <param name="value">The value for the Y data.</param>
        internal DataPointY(int seriesIndex, double value)
        {
            SeriesIndex = seriesIndex;
            Value = value;
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the series index for this Y value.
        /// </summary>
        public int SeriesIndex
        {
            get;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public double Value
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a string representation of this object.
        /// </summary>
        /// <returns>A string that describes this object.</returns>
        public override string ToString()
        {
            return $"Index: {SeriesIndex} Value: {Value}";
        }
        #endregion
    }
}
