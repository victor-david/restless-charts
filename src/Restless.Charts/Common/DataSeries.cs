using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a data series that contains X values
    /// and one or more Y values associated with each X value.
    /// </summary>
    /// <remarks>
    /// The <see cref="DataSeries"/> class is esentially a matrix.
    /// It stores X values which each have a corresponding <see cref="DataPointYCollection"/>
    /// of Y values. X values cannot be duplicated.
    /// </remarks>
    public class DataSeries : IEnumerable<DataPointX>
    {
        #region Private
        private readonly HashSet<double> xValues;
        private readonly List<DataPointX> storage;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Creates a <see cref="DataSeries"/> object with a single Y series.
        /// </summary>
        /// <returns>A DataSeries object that supports a single Y series.</returns>
        public static DataSeries Create()
        {
            return Create(1);
        }

        /// <summary>
        /// Creates a <see cref="DataSeries"/> object with the specified number of Y series.
        /// </summary>
        /// <param name="maxYSeries">The number of Y series.</param>
        /// <returns>A DataSeries object thst supports the specified number of Y series.</returns>
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
            xValues = new HashSet<double>();
            storage = new List<DataPointX>();
            DataRange = DataRange.EmptyDataRange();
            MinXValue = double.NaN;
            MaxXValue = double.NaN;
            DataInfo = new DataSeriesInfoCollection(MaxYSeries);
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
        /// Gets the minimum X value of all <see cref="DataPointX"/> objects that have been added to the series.
        /// </summary>
        public double MinXValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the maximum X value of all <see cref="DataPointX"/> objects that have been added to the series.
        /// </summary>
        public double MaxXValue
        {
            get;
            private set;
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
        public DataPointX this[int index]
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
        /// Gets a projected count of items. See remarks for more.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If all X values in this series are evenly spaced (0, 1 , 2, 3, 4, etc.),
        /// this property returns the same value as <see cref="Count"/>.
        /// </para>
        /// <para>
        /// If X values are not evenly spaced, this property returns a value that
        /// represents what the count would be if all X values were evenly spaced.
        /// </para>
        /// </remarks>
        public int ProjectedCount
        {
            get => GetProjectedCount();
        }

        /// <summary>
        /// Gets the collection of <see cref="DataSeriesInfo"/> objects used for this series.
        /// </summary>
        public DataSeriesInfoCollection DataInfo
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
        /// <exception cref="ArgumentException">Either <paramref name="xValue"/> or <paramref name="yValue"/> is not finite.</exception>
        public void Add(double xValue, double yValue)
        {
            if (!xValue.IsFinite() || !yValue.IsFinite())
            {
                throw new ArgumentException("Values must be finite");
            }

            DataRange.Include(xValue, yValue);
            MinXValue = double.IsNaN(MinXValue) ? xValue : Math.Min(MinXValue, xValue);
            MaxXValue = double.IsNaN(MaxXValue) ? xValue : Math.Max(MaxXValue, xValue);
            DataPointX point = GetDataPointAt(xValue);
            if (point.YValues.Count < MaxYSeries)
            {
                point.AddY(yValue);
            }
            // XValues must remain sorted. A BarChart uses the enumerator
            // (which uses an OrderBy<> to return in XValue order), but a LineChart
            // uses indices so it can look ahead to the next data point for the
            // connecting line.
            storage.Sort(SortByXValue);
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
            xValues.Clear();
            DataRange.X.MakeEmpty();
            DataRange.Y.MakeEmpty();
            MinXValue = double.NaN;
            MaxXValue = double.NaN;
        }
        #endregion

        /************************************************************************/

        #region IEnumerable implementation
        /// <summary>
        /// Gets the generic enumerator. This enumerator
        /// returns <see cref="DataPointX"/> objects in ascending
        /// order of <see cref="DataPointX.Value"/>.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<DataPointX> GetEnumerator()
        {
            foreach (DataPointX point in storage.OrderBy((dp) => dp.Value))
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

        #region Other enumerables
        /// <summary>
        /// Gets an enumerable of X double values. The values
        /// are returned in ascending order.
        /// </summary>
        /// <returns>The enumerable.</returns>
        public IEnumerable<double> EnumerateX()
        {
            foreach (DataPointX point in this)
            {
                yield return point.Value;
            }
        }

        /// <summary>
        /// Gets an enumerable of Y double values.
        /// </summary>
        /// <param name="ySeriesIndex">The Y series index.</param>
        /// <returns>The enumerable</returns>
        /// <remarks>
        /// This method gets an enumerable that for each X value gets the Y value at the specfied index.
        /// For example, EnumerateY(1) returns: X[0].Y[1], X[1].Y[1], X[2].Y[1], X[3].Y[1], etc.
        /// </remarks>
        public IEnumerable<double> EnumerateY(int ySeriesIndex)
        {
            if (ySeriesIndex < 0 || ySeriesIndex > MaxYSeries - 1)
            {
                throw new IndexOutOfRangeException($"Y series index {ySeriesIndex} is out of range");
            }
            foreach (DataPointX point in this)
            {
                yield return point.YValues[ySeriesIndex].Value;
            }
        }

        /// <summary>
        /// Gets an enumerable of Y double values.
        /// </summary>
        /// <returns>The enumerable.</returns>
        /// <remarks>
        /// This method gets an enumerable that returns all Y values.
        /// First, each Y value in Y series zero is returned, then in Y series 1
        /// (if it exists), etc. until all Y values have been enumerated.
        /// </remarks>
        public IEnumerable<double> EnumerateY()
        {
            for (int yIdx = 0; yIdx < MaxYSeries; yIdx++)
            {
                foreach (DataPointX point in this)
                {
                    yield return point.YValues[yIdx].Value;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Called during <see cref="Add(double, double)"/> to keep X values sorted.
        /// </summary>
        private int SortByXValue(DataPointX dp1, DataPointX dp2)
        {
            return dp1.Value.CompareTo(dp2.Value);
        }

        /// <summary>
        /// Gets the data point object with the specified X value.
        /// If it doesn't yet exist, first creates it and adds it to storage.
        /// </summary>
        /// <param name="xValue">The x value</param>
        /// <returns>A DataPoint, either already in storage or freshly added.</returns>
        private DataPointX GetDataPointAt(double xValue)
        {
            if (xValues.Contains(xValue))
            {
                return GetDataPointFromStorage(xValue);
            }

            DataPointX newPoint = new DataPointX(MaxYSeries, xValue);
            storage.Add(newPoint);
            xValues.Add(xValue);
            return newPoint;
        }

        /// <summary>
        /// Gets the data point object with the specified X value.
        /// If not found, throws an exception. Exception here indicates programmer error.
        /// </summary>
        /// <param name="xValue">The value.</param>
        /// <returns>A DataPoint.</returns>
        private DataPointX GetDataPointFromStorage(double xValue)
        {
            foreach (DataPointX point in this)
            {
                if (point.Value == xValue) return point;
            }
            throw new ArgumentOutOfRangeException(nameof(xValue), xValue, "Internal error");
        }

        private double GetMinYValue()
        {
            double min = double.MaxValue;

            foreach (DataPointX pointX in this)
            {
                foreach (DataPointY pointY in pointX.YValues)
                {
                    min = Math.Min(min, pointY.Value);
                }
            }
            return min;
        }

        private int GetProjectedCount()
        {
            if (Count <= 2) return Count;

            double minDistance = GetMinimumXDistance();
            int projCount = (int)(Math.Ceiling(MaxXValue) / minDistance) + 1;
            return projCount;
        }

        private double GetMinimumXDistance()
        {
            double result = double.MaxValue;

            for (int idx = 0; idx < Count; idx++)
            {
                if (idx < Count - 1)
                {
                    double distance = storage[idx + 1].Value - storage[idx].Value;
                    result = Math.Min(result, distance);
                }
            }

            return result;
        }
        #endregion
    }
}
