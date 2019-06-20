using System.Collections.Generic;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides a converter that can map abitrary values to a string.
    /// </summary>
    public class MappedValueConverter : IDoubleConverter
    {
        #region Private
        private readonly Dictionary<double, string> map;
        private readonly string notMapped;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MappedValueConverter"/> class.
        /// </summary>
        /// <param name="notMapped">The string to return during convert if the value to convert isn't mapped.</param>
        public MappedValueConverter(string notMapped)
        {
            map = new Dictionary<double, string>();
            this.notMapped = notMapped;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedValueConverter"/> class using the default
        /// value for the string that is returned when the value to convert isn't mapped.
        /// </summary>
        public MappedValueConverter() : this("--")
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Converts the specified value to its mapped string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="format">The format. This parameter satisfies the <see cref="IDoubleConverter"/> interface, but is not used.</param>
        /// <returns>The mapped string.</returns>
        public string Convert(double value, string format)
        {
            if (map.ContainsKey(value)) return map[value];
            return notMapped;
        }

        /// <summary>
        /// Clears all values from the map.
        /// </summary>
        public void ClearMap()
        {
            map.Clear();
        }

        /// <summary>
        /// Adds a value / text pair to the map.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="text">The text to map to <paramref name="value"/>.</param>
        public void AddToMap(double value, string text)
        {
            if (map.ContainsKey(value))
            {
                map[value] = text;
            }
            else
            {
                map.Add(value, text);
            }
        }
        #endregion
    }
}
