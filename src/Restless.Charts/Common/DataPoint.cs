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
        internal DataPoint(double xValue)
        {
            XValue = xValue;
            YValues = new DataSequence();
        }

        /// <summary>
        /// Gets the X value for this data point.
        /// </summary>
        public double XValue
        {
            get;
        }

        /// <summary>
        /// Gets the data sequence for the Y values
        /// </summary>
        public DataSequence YValues
        {
            get;
        }

        internal void AddY(double y)
        {
            YValues.Add(y);
        }
    }
}
