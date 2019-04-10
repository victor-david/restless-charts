using System;
using System.Collections.Generic;
using System.Windows;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a collection of <see cref="MajorTick"/> objects.
    /// </summary>
    public class MajorTickCollection : List<MajorTick>
    {
        #region Public methods
        /// <summary>
        /// Gets a boolean value that indicates if the specified
        /// tick value is present in the collection.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>true if a <see cref="MajorTick"/> with <paramref name="value"/> exists; otherwise, false.</returns>
        public bool Contains(double value)
        {
            foreach (MajorTick tick in this)
            {
                if (tick.Value == value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a size struct that represents the maximum width and maximum height of all major tick labels.
        /// </summary>
        /// <returns>A size structure.</returns>
        public Size GetMaxTextSize()
        {
            Size size = new Size();
            foreach (MajorTick tick in this)
            {
                size.Width = Math.Max(size.Width, tick.TextWidth);
                size.Height = Math.Max(size.Height, tick.TextHeight);
            }
            return size;
        }

        /// <summary>
        /// Gets the total size used by all major tick labels including <paramref name="spacingBetweenText"/>.
        /// </summary>
        /// <param name="measureSize">The size to pass to the text's Measure method.</param>
        /// <param name="spacingBetweenText">The spacing between text to use in the calculations.</param>
        /// <returns>A size structure.</returns>
        public Size GetTotalTextSize(Size measureSize, double spacingBetweenText)
        {
            spacingBetweenText = Math.Abs(spacingBetweenText);
            Size size = new Size();
            foreach (MajorTick tick in this)
            {
                tick.Text.Measure(measureSize);
                size.Width += tick.TextWidth;
                size.Height += tick.TextHeight;
            }
            if (Count > 1)
            {
                size.Width += (Count - 1) * spacingBetweenText;
                size.Height += (Count - 1) * spacingBetweenText;
            }
            return size;
        }

        /// <summary>
        /// Measures text lables and returns the smallest spacing between any of them.
        /// </summary>
        /// <param name="measureSize">The size to pass to the text's Measure method.</param>
        /// <param name="useWidth">
        /// true to measure spacing using the width of the text labels, 
        /// false to measure spacing using the height.
        /// </param>
        /// <returns>The smallest text spacing. Value can be negative indicating that the text labels overlap.</returns>
        public double GetMinimumTextSpacing(Size measureSize, bool useWidth)
        {
            double result = double.MaxValue;

            foreach (MajorTick tick in this)
            {
                tick.Text.Measure(measureSize);
            }

            for (int idx = 0; idx < Count; idx++)
            {
                if (idx < Count - 1)
                {
                    MajorTick thisTick = this[idx];
                    MajorTick nextTick = this[idx + 1];

                    double thisTickTextDimension = useWidth ? thisTick.TextWidth : thisTick.TextHeight;
                    double nextTickTextDimension = useWidth ? nextTick.TextWidth : nextTick.TextHeight;

                    double distance = Math.Abs(nextTick.Coordinate - thisTick.Coordinate) - (nextTickTextDimension / 2.0) - (thisTickTextDimension / 2.0);
                    result = Math.Min(result, distance);
                }
            }
            return result;
        }

        /// <summary>
        /// Provides an enumerable that enumerates the values of <see cref="MinorTick.Coordinate"/>.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerable<double> EnumerateTickCoordinates()
        {
            foreach (MajorTick tick in this)
            {
                yield return tick.Coordinate;
            }
        }
        #endregion
    }
}