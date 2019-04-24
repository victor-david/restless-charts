namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a single data point on the X axis.
    /// </summary>
    public class DataPointX
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointX"/> class.
        /// </summary>
        /// <param name="maxY">The maximum allowed for <see cref="YValues"/>.</param>
        /// <param name="value">The value for the Y data.</param>
        internal DataPointX(int maxY, double value)
        {
            Value = value;
            YValues = new DataPointYCollection(maxY);
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the X value for this data point.
        /// </summary>
        public double Value
        {
            get;
        }

        /// <summary>
        /// Gets the data sequence for the Y values
        /// </summary>
        public DataPointYCollection YValues
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
            return $"X: {Value} Y Capacity:{YValues.Capacity}";
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        internal void AddY(double y)
        {
            YValues.Add(y);
        }
        #endregion
    }
}