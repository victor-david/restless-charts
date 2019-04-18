using System.Collections.Generic;

namespace Application.Sample
{
    /// <summary>
    /// Represents a faker category table. This data structure self populates.
    /// </summary>
    public class CategoryTable : List<CategoryRow>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryTable"/> class.
        /// This "table" is self populating.
        /// </summary>
        public CategoryTable()
        {
            Populate();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Gets the category row associated the specified id, or null if it doesn't exist.
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>The <see cref="CategoryRow"/> object, or null if doesn't exist.</returns>
        public CategoryRow GetCategoryRow(long id)
        {
            foreach (CategoryRow cat in this)
            {
                if (cat.Id == id) return cat;
            }
            return null;
        }
        #endregion

        #region Private methods
        private void Populate()
        {
            Add(1, "Art", 100);
            Add(2, "Food", 88);
            Add(3, "Home", 160);
            Add(4, "Auto", 77);
            Add(5, "Taxes", 109);
            Add(6, "Gas", 133);
            Add(7, "Water", 43);
            Add(8, "Rent", 420);
            Add(9, "Atm", 349);
            Add(10, "Electric", 87);
            Add(11, "Cats", 29);
            Add(12, "Dogs", 91);
            Add(13, "Computer", 227);
            Add(14, "TV", 36);
        }

        private void Add(long id, string name, double value)
        {
            Add(new CategoryRow(id, name, value));
        }
        #endregion
    }
}
