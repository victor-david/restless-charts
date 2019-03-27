using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a fixed size brush collection.
    /// </summary>
    public class BrushCollection : IEnumerable<Brush>
    {
        #region Private
        private readonly List<Brush> storage;
        #endregion

        /************************************************************************/

        #region Constructor
        internal BrushCollection(int capacity, Brush defaultBrush)
        {
            storage = new List<Brush>();
            for (int k = 0; k < capacity; k++)
            {
                storage.Add(defaultBrush);
            }
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the brush at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The brush.</returns>
        /// <exception cref="IndexOutOfRangeException">An attempt was made to use an index that is out of range.</exception>
        public Brush this[int index]
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
        /// Sets the brush at the specified index.
        /// </summary>
        /// <param name="index">The zero based index.</param>
        /// <param name="brush">The brush.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        public void SetBrush(int index, Brush brush)
        {
            ValidateIndex(index);
            storage[index] = brush;
        }

        /// <summary>
        /// Gets the brush at the specified index.
        /// </summary>
        /// <param name="index">The zero based index.</param>
        /// <returns>The brush at the specified index.</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        public Brush GetBrush(int index)
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
        public IEnumerator<Brush> GetEnumerator()
        {
            return ((IEnumerable<Brush>)storage).GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Brush>)storage).GetEnumerator();
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
