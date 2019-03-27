using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a data series that contains X values
    /// and one or more Y values associated with each X value.
    /// </summary>
    public class DataSeries : IEnumerable<DataPoint>
    {
        #region Private
        private readonly List<DataPoint> storage;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Creates a <see cref="DataSeries"/> object with a single Y series.
        /// </summary>
        /// <returns></returns>
        public static DataSeries Create()
        {
            return Create(1);
        }

        /// <summary>
        /// Creates a <see cref="DataSeries"/> object with the specified number of Y series.
        /// </summary>
        /// <param name="maxYSeries">The number of Y series.</param>
        /// <returns>A DataSeries object.</returns>
        public static DataSeries Create(int maxYSeries)
        {
            return new DataSeries(maxYSeries);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeries"/> class.
        /// </summary>
        private DataSeries(int maxYSeries)
        {
            MaxYSeries = Math.Max(1, maxYSeries);
            storage = new List<DataPoint>();
            DataRange = DataRange.EmptyDataRange();

            DataBrushes = new BrushCollection(MaxYSeries, Brushes.Black);
            PrimaryTextBrushes = new BrushCollection(MaxYSeries, Brushes.White);
            SecondaryTextBrushes = new BrushCollection(MaxYSeries, Brushes.Black);
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the max count of Y series for this data series.
        /// </summary>
        public int MaxYSeries
        {
            get;
        }

        /// <summary>
        /// Gets the data range for this data series collection.
        /// </summary>
        public DataRange DataRange
        {
            get;
        }

        /// <summary>
        /// Gets the data point at the specified index.
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The data point.</returns>
        /// <exception cref="IndexOutOfRangeException">An attempt was made to use an index that is out of range.</exception>
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
        /// Gets the brush collection used for the Y data of this series.
        /// </summary>
        public BrushCollection DataBrushes
        {
            get;
        }

        /// <summary>
        /// Gets the brush collection used for the primary text brush of the Y data series.
        /// This property is used when <see cref="ChartBase.DisplayValues"/> is true.
        /// </summary>
        public BrushCollection PrimaryTextBrushes
        {
            get;
        }

        /// <summary>
        /// Gets the brush collection used for the secondary text brush of the Y data series.
        /// This property is used when <see cref="ChartBase.DisplayValues"/> is true.
        /// </summary>
        public BrushCollection SecondaryTextBrushes
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Adds a new data point.
        /// </summary>
        /// <param name="xValue">The x value.</param>
        /// <param name="yValue">The y value</param>
        public void Add(double xValue, double yValue)
        {
            DataRange.Include(xValue, yValue);
            DataPoint point = GetDataPointAt(xValue);
            if (point.YValues.Count < MaxYSeries)
            {
                point.AddY(yValue);
            }
        }

        /// <summary>
        /// Examines Y data and adjusts <see cref="DataRange"/> accordingly.
        /// If Y includes negative numbers, makes Y axis centered on zero.
        /// If not, includes zero on the Y axis.
        /// </summary>
        public void MakeYAutoZero()
        {
            double min = GetMinYValue();
            if (min > 0) DataRange.Y.Include(0);
            if (min < 0) DataRange.Y.MakeZeroCentered();
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

        /// <summary>
        /// Clears all data points.
        /// </summary>
        public void Clear()
        {
            storage.Clear();
            DataRange.X.MakeEmpty();
            DataRange.Y.MakeEmpty();
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

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Gets the data point object with the specified X value.
        /// If it doesn't yet exist, first creates it and adds it to storage.
        /// </summary>
        /// <param name="xValue">The x value</param>
        /// <returns>A DataPoint, either already in storage or freshly added.</returns>
        private DataPoint GetDataPointAt(double xValue)
        {
            foreach (DataPoint point in this)
            {
                if (point.XValue == xValue) return point;
                if (point.XValue > xValue) break;
            }

            DataPoint newPoint = new DataPoint(xValue);
            storage.Add(newPoint);
            return newPoint;
        }

        private double GetMinYValue()
        {
            double min = double.MaxValue;

            foreach (DataPoint point in this)
            {
                foreach (double value in point.YValues)
                {
                    min = Math.Min(min, value);
                }
            }
            return min;
        }
        #endregion
    }
}
