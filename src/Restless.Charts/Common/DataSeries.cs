using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a data series
    /// </summary>
    public class DataSeries : IEnumerable<DataPoint>
    {
        #region Private
        private readonly DataSeriesCollection owner;
        private readonly List<DataPoint> storage;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeries"/> class.
        /// </summary>
        internal DataSeries(DataSeriesCollection owner)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
            storage = new List<DataPoint>();
            Brush = Brushes.SteelBlue;
            PrimaryTextBrush = Brushes.White;
            SecondaryTextBrush = Brushes.Black;
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the data point at the specified index.
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The data point.</returns>
        /// <exception cref="ArgumentNullException">An attempt was made to set a null value</exception>
        public DataPoint this[int index]
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
        /// Gets or sets the brush that is used for this data series.
        /// </summary>
        public Brush Brush
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the primary text brush used for this series.
        /// This property is used when <see cref="ChartBase.DisplayValues"/> is true.
        /// </summary>
        public Brush PrimaryTextBrush
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the secondary text brush used for this series.
        /// This property is used by certain charts when <see cref="ChartBase.DisplayValues"/> is true.
        /// </summary>
        public Brush SecondaryTextBrush
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Adds a new data point.
        /// </summary>
        /// <param name="xValue">The x value.</param>
        /// <param name="yValue">The y value</param>
        /// <exception cref="ArgumentNullException">An attempt was made to add a null item.</exception>
        public void Add(double xValue, double yValue)
        {
            owner.DataRange.Include(xValue, yValue);
            storage.Add(new DataPoint(xValue, yValue));
        }

        /// <summary>
        /// Clears all data points.
        /// </summary>
        public void Clear()
        {
            storage.Clear();
            owner.RecalculateDataRange();
        }
        #endregion

        /************************************************************************/

        #region IEnumerable implementation
        /// <summary>
        /// Gets the generic enumerator. This enumerator
        /// returns <see cref="DataPoint"/> objects in ascending
        /// order of <see cref="DataPoint.XValue"/>.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<DataPoint> GetEnumerator()
        {
            foreach (DataPoint point in storage.OrderBy((dp) => dp.XValue))
            {
                yield return point;
            }
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

        #region Private methods
        #endregion
    }
}
