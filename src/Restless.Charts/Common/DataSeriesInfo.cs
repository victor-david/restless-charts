namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents information for a single data series.
    /// </summary>
    public class DataSeriesInfo
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeriesInfo"/> class.
        /// </summary>
        /// <param name="index">The index of data series.</param>
        /// <param name="name">The name of the data series.</param>
        internal DataSeriesInfo(int index, string name)
        {
            Index = index;
            Name = string.IsNullOrEmpty(name) ? "Unnamed" : name;
            Visual = new DataSeriesVisual();
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the index position of this series info.
        /// </summary>
        public int Index
        {
            get;
        }

        /// <summary>
        /// Gets or sets the name of the data series.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the visual components of the data series.
        /// </summary>
        public DataSeriesVisual Visual
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a string representation of this object.
        /// </summary>
        /// <returns>A string that describes the state of this object.</returns>
        public override string ToString()
        {
            return $"Index: {Index} Name: {Name}";
        }
        #endregion
    }
}
