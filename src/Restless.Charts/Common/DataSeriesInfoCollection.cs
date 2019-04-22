using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a fixed size collection of <see cref="DataSeriesInfo"/> objects.
    /// </summary>
    public class DataSeriesInfoCollection : IEnumerable<DataSeriesInfo>
    {
        #region Private
        private readonly List<DataSeriesInfo> storage;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeriesInfoCollection"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="defaultBrush">The default brush to assign.</param>
        internal DataSeriesInfoCollection(int capacity, Brush defaultBrush)
        {
            storage = new List<DataSeriesInfo>();
            for (int idx = 0; idx < capacity; idx++)
            {
                storage.Add(new DataSeriesInfo(idx, $"Series {idx+1}", defaultBrush));
            }
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the <see cref="DataSeriesInfo"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The brush.</returns>
        /// <exception cref="IndexOutOfRangeException">An attempt was made to use an index that is out of range.</exception>
        public DataSeriesInfo this[int index]
        {
            get
            {
                ValidateIndex(index);
                return storage[index];
            }
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Sets information for data series at the specified index.
        /// This overload sets <see cref="DataSeriesInfo.Name"/> to an auto value.
        /// </summary>
        /// <param name="index">The zero based index.</param>
        /// <param name="dataBrush">The brush used for the series data.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        /// <remarks>
        /// This overload sets the <see cref="DataSeriesInfo.Name"/> property to a value of "Series [N]".
        /// This can be useful if you aren't going to display the series info in a legend.
        /// </remarks>
        public void SetInfo(int index, Brush dataBrush)
        {
            SetInfo(index, null, dataBrush);
        }

        /// <summary>
        /// Sets information for data series at the specified index.
        /// </summary>
        /// <param name="index">The zero based index.</param>
        /// <param name="name">The name of the data series.</param>
        /// <param name="dataBrush">The brush used for the series data.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        public void SetInfo(int index, string name, Brush dataBrush)
        {
            ValidateIndex(index);
            if (string.IsNullOrEmpty(name)) name = $"Series {index+1}";
            storage[index] = new DataSeriesInfo(index, name, dataBrush);
        }

        /// <summary>
        /// Gets the brush at the specified index.
        /// </summary>
        /// <param name="index">The zero based index.</param>
        /// <returns>The brush at the specified index.</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        public DataSeriesInfo GetLegend(int index)
        {
            ValidateIndex(index);
            return storage[index];
        }
        #endregion

        /************************************************************************/

        #region IEnumerable implementation
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<DataSeriesInfo> GetEnumerator()
        {
            return ((IEnumerable<DataSeriesInfo>)storage).GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<DataSeriesInfo>)storage).GetEnumerator();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Throws an <see cref="IndexOutOfRangeException"/> if <paramref name="index"/> is within the range of storage.
        /// </summary>
        /// <param name="index">The index.</param>
        private void ValidateIndex(int index)
        {
            if (index <0 || index >= storage.Count)
            {
                throw new IndexOutOfRangeException();
            }
        }
        #endregion
    }
}
