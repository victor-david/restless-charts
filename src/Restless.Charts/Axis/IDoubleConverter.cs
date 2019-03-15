using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Defines a method for converting a double value to a string.
    /// </summary>
    public interface IDoubleConverter
    {
        /// <summary>
        /// Converts the specified double value to a string.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="format">The string format.</param>
        /// <returns>A string representation of the value</returns>
        string Convert(double value, string format);
    }
}
