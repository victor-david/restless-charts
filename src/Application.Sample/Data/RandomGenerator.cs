using System;
using System.Collections.Generic;

namespace Application.Sample
{
    /// <summary>
    /// Provides a random number generator that can avoid duplicates.
    /// </summary>
    public class RandomGenerator
    {
        #region Private
        private readonly Random rand;
        private readonly List<int> generated;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="RandomGenerator"/> class.
        /// </summary>
        /// <param name="min">The minimum value inclusive.</param>
        /// <param name="max">The maximum value inclusive.</param>
        public RandomGenerator(int min, int max)
        {
            Min = min;
            Max = max;
            if (min > max)
            {
                Min = max;
                Max = min;
            }

            rand = new Random();
            generated = new List<int>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the minimum value for this random generator.
        /// </summary>
        public int Min
        {
            get;
        }

        /// <summary>
        /// Gets the maximum value for this random generator.
        /// </summary>
        public int Max
        {
            get;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Clears the list of generated numbers used to detect duplicates.
        /// </summary>
        public void Reset()
        {
            generated.Clear();
        }

        /// <summary>
        /// Gets a value between <see cref="Min"/> and <see cref="Max"/>. Duplicates are allowed.
        /// </summary>
        /// <returns>A random value.</returns>
        public int GetValue()
        {
            return GetValue(true);
        }

        /// <summary>
        /// Gets a value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="allowDuplicate">true to allow duplicates; false to disallow duplicates.</param>
        /// <returns>A random value.</returns>
        public int GetValue(bool allowDuplicate)
        {
            int value = rand.Next(Min, Max + 1);

            if (!allowDuplicate)
            {
                int tryCount = 0;
                while (generated.Contains(value) && tryCount < 40)
                {
                    value = rand.Next(Min, Max + 1);
                    tryCount++;
                }
                generated.Add(value);
            }
            return value;
        }
        #endregion
    }
}
