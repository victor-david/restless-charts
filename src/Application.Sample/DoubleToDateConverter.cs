using Restless.Controls.Chart;
using System;

namespace Application.Sample
{
    public class DoubleToDateConverter : IDoubleConverter
    {
        /// <summary>
        /// Converts the specified value into a string representation of a date
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="format">The format</param>
        /// <returns>A date string</returns>
        public string Convert(double value, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "dd-MMM-yyyy";
            }
            long ticks = (long)value;
            if (ticks >= DateTime.MinValue.Ticks && ticks <= DateTime.MaxValue.Ticks)
            {
                DateTime dt = new DateTime((long)value);
                return dt.ToString(format);
            }
            return "--";
        }
    }
}
