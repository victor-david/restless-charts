using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a collection of <see cref="DataSeries"/> objects
    /// </summary>
    public class DataSeriesCollection : IEnumerable<DataSeries>
    {
        #region Private
        private readonly List<DataSeries> storage;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeriesCollection"/> class.
        /// </summary>
        public DataSeriesCollection()
        {
            storage = new List<DataSeries>();
            DataRange = DataRange.EmptyDataRange();
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the data series at the specified index.
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The data series</returns>
        /// <exception cref="IndexOutOfRangeException">The parameter <paramref name="index"/> is out of bounds</exception>
        public DataSeries this[int index]
        {
            get => storage[index];
        }

        /// <summary>
        /// Gets the count of items.
        /// </summary>
        public int Count
        {
            get => storage.Count;
        }

        /// <summary>
        /// Gets the data range for this data series collection.
        /// </summary>
        public DataRange DataRange
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Adds a new data series to the collection.
        /// </summary>
        /// <returns>The newly added series.</returns>
        public DataSeries Add()
        {
            DataSeries series = new DataSeries(this);
            storage.Add(series);
            return series;
        }

        /// <summary>
        /// Clears all data series from the collection.
        /// </summary>
        public void Clear()
        {
            storage.Clear();
            DataRange.X.MakeEmpty();
            DataRange.Y.MakeEmpty();
        }

        /// <summary>
        /// Modifies the aggragated data range of this collection
        /// so that the X range is centered on zero.
        /// </summary>
        public void MakeXZeroCentered()
        {
            DataRange.X.MakeZeroCentered();
        }

        /// <summary>
        /// Modifies the aggragated data range of this collection
        /// so that the Y range is centered on zero.
        /// </summary>
        public void MakeYZeroCentered()
        {
            DataRange.Y.MakeZeroCentered();
        }

        /// <summary>
        /// Modifies the aggragated data range of this collection
        /// so that both X range and Y range are centered on zero.
        /// </summary>
        public void MakeZeroCentered()
        {
            DataRange.Y.MakeZeroCentered();
            DataRange.X.MakeZeroCentered();
        }

        /// <summary>
        /// Expands the X range by both increasing Max and decreasing Min by the specified amount.
        /// </summary>
        /// <param name="amount">The amount to expand.</param>
        public void ExpandX(double amount)
        {
            double min = DataRange.X.Min - Math.Abs(amount);
            double max = DataRange.X.Max + Math.Abs(amount);
            DataRange.X.Include(min);
            DataRange.X.Include(max);
        }

        /// <summary>
        /// Expands the Y range by both increasing Max and decreasing Min by the specified amount.
        /// </summary>
        /// <param name="amount">The amount to expand.</param>
        public void ExpandY(double amount)
        {
            double min = DataRange.Y.Min - Math.Abs(amount);
            double max = DataRange.Y.Max + Math.Abs(amount);
            DataRange.Y.Include(min);
            DataRange.Y.Include(max);
        }
        #endregion

        #region Internal methods
        /// <summary>
        /// Resets and reclaculates the range of data values in the collection.
        /// </summary>
        internal void RecalculateDataRange()
        {
            DataRange.X.MakeEmpty();
            DataRange.Y.MakeEmpty();

            foreach (DataSeries series in this)
            {
                foreach (DataPoint point in series)
                {
                    DataRange.Include(point.XValue, point.YValue);
                }
            }
        }
        #endregion

        /************************************************************************/

        #region IEnumerable implementation
        /// <summary>
        /// Gets the generic enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<DataSeries> GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
