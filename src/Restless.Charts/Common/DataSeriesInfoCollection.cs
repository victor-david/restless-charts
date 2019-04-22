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
        internal DataSeriesInfoCollection(int capacity)
        {
            storage = new List<DataSeriesInfo>();
            for (int idx = 0; idx < capacity; idx++)
            {
                storage.Add(new DataSeriesInfo(idx, $"Series {idx+1}"));
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
        /// </summary>
        /// <param name="index">The zero based index.</param>
        /// <param name="name">The name of the data series. If null or empty, does not set the name.</param>
        /// <param name="data">The brush used for the series data. If null, does not set the brush.</param>
        /// <param name="primaryText">The brush used for the primary text. If null, does not set the brush.</param>
        /// <param name="secondaryText">The brush used for the secondary text. If null, does not set the brush.</param>
        /// <param name="border">The brush used for the border. If null, does not set the brush.</param>
        /// <param name="borderThickness">The border thickness. If less than zero, does not set the thickness.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        /// <remarks>
        /// <para>
        /// Brush parameters to this method have a default value of null. If you use the default value, the brush
        /// will not be set. To specifically set a brush to null, set the visual property directly, 
        /// ex: data.DataInfo[index].Visual.Border = null;
        /// </para>
        /// <para>
        /// <paramref name="borderThickness"/> works the same. Its default value of -1 means it will remain unchanged.
        /// </para>
        /// </remarks>
        public void SetInfo(int index, string name, Brush data = null, Brush primaryText = null, Brush secondaryText = null, Brush border = null, double borderThickness = -1)
        {
            ValidateIndex(index);
            DataSeriesInfo info = storage[index];

            if (!string.IsNullOrEmpty(name)) info.Name = name;
            if (data != null) info.Visual.Data = data;
            if (primaryText != null) info.Visual.PrimaryText = primaryText;
            if (secondaryText != null) info.Visual.SecondaryText = secondaryText;
            if (border != null) info.Visual.Border = border;
            if (borderThickness >= 0.0) info.Visual.BorderThickness = borderThickness;
        }

        /// <summary>
        /// Sets border brush and border thickness for all series
        /// </summary>
        /// <param name="border">The border brush.</param>
        /// <param name="borderThickness">The border thickness.</param>
        public void SetInfo(Brush border, double borderThickness)
        {
            foreach (DataSeriesInfo info in this)
            {
                info.Visual.Border = border;
                info.Visual.BorderThickness = borderThickness;
            }
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
