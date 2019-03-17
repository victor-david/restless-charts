using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a single data point.
    /// </summary>
    public class DataPoint
    {
        internal DataPoint(double xValue, double yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }

        /// <summary>
        /// Gets the X value for this data point.
        /// </summary>
        public double XValue
        {
            get;
        }

        /// <summary>
        /// Gets the Y value for this data point.
        /// </summary>
        public double YValue
        {
            get;
        }
    }
}
