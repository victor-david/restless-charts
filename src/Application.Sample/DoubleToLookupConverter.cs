using Restless.Controls.Chart;

namespace Application.Sample
{
    /// <summary>
    /// Represents a converter to provide an axis label via a lookup table.
    /// </summary>
    public class DoubleToLookupConverter : IDoubleConverter
    {
        #region Private
        private readonly CategoryTable catTable;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleToLookupConverter"/> class.
        /// </summary>
        public DoubleToLookupConverter()
        {
            catTable = new CategoryTable();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Converts the specified value into a string via a table lookup.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="format">The format (in this case, ignored)</param>
        /// <returns>A string</returns>
        public string Convert(double value, string format)
        {
            long id = (long)value;
            // Use the id to lookup a corresponding category in our faker table.
            CategoryRow catRow = catTable.GetCategoryRow(id);
            return (catRow != null) ? catRow.Name : $"Cat {id}";
        }
        #endregion
    }
}
