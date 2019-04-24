using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a collection of <see cref="DataPointY"/> objects.
    /// </summary>
    public class DataPointYCollection : ObservableCollection<DataPointY>
    {
        #region Private
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointYCollection"/> class.
        /// </summary>
        /// <param name="capacity">The maximum capacity of the collection.</param>
        internal DataPointYCollection(int capacity)
        {
            Capacity = capacity;
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the maximum capacity of the collection
        /// </summary>
        public int Capacity
        {
            get;
        }

        /// <summary>
        /// Gets the sum of all values in the data sequence.
        /// </summary>
        public double Sum
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Adds a value to the underlying storage.
        /// </summary>
        /// <param name="value">The value to add.</param>
        internal void Add(double value)
        {
            Add(new DataPointY(Count, value));
            Sum += value;
        }

        internal List<int> GetSmallestValueIndices(int maxToGet)
        {
            List<int> result = new List<int>();


            foreach (DataPointY point in this.OrderBy((p) => p.Value))
            {
                if (result.Count < maxToGet)
                {
                    result.Add(IndexOf(point));
                }
            }

            return result;
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Inserts the specified item if the collection has fewer items than <see cref="Capacity"/>.
        /// </summary>
        /// <param name="index">The item index.</param>
        /// <param name="item">The item.</param>
        protected override void InsertItem(int index, DataPointY item)
        {
            if (Count < Capacity)
            {
                base.InsertItem(index, item);
            }
        }

        protected override void SetItem(int index, DataPointY item)
        {
            base.SetItem(index, item);
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            Sum = 0;
        }
        #endregion


    }
}
