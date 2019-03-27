using System;
using System.Collections;
using System.Collections.Generic;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents an enumerable series of double values.
    /// </summary>
    public class DataSequence : IEnumerable<double>
    {
        #region Private
        private readonly List<double> storage;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSequence"/> class.
        /// </summary>
        internal DataSequence()
        {
            storage = new List<double>();
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the value at the specified index.
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The value.</returns>
        /// <exception cref="IndexOutOfRangeException">An attempt was made to access a value out of range.</exception>
        public double this[int index]
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
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Adds a value to the underlying storage.
        /// </summary>
        /// <param name="value">The value to add.</param>
        internal void Add(double value)
        {
            storage.Add(value);
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
        public IEnumerator<double> GetEnumerator()
        {
            foreach (double value in storage)
            {
                yield return value;
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
    }
}
