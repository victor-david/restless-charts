namespace Application.Sample
{
    /// <summary>
    /// Represents a readonly row in a category lookup table.
    /// </summary>
    public class CategoryRow
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRow"/> class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        public CategoryRow(long id, string name, double amount)
        {
            Id = id;
            Name = !string.IsNullOrEmpty(name) ? name : "Unknown";
            Amount = amount;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the category id.
        /// </summary>
        public long Id
        {
            get;
        }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Gets the amount associated with the category.
        /// </summary>
        public double Amount
        {
            get;
        }
        #endregion
    }
}
